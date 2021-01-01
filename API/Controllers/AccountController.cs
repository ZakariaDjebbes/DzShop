using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signIn;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signIn,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signIn = signIn;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByClaimsAsync(HttpContext.User);

            return new UserDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("userExists")]
        public async Task<ActionResult<bool>> CheckUserExist([FromQuery] string username)
        {
            return await _userManager.FindByNameAsync(username) != null;
        }

        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindByClaimsWithAddressAsync(HttpContext.User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [HttpPut("updatePassword")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var user = await _userManager.FindByClaimsWithAddressAsync(HttpContext.User);
            var result = await _userManager.ChangePasswordAsync(user, updatePasswordDto.OldPassword, updatePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            return Ok(true);
        }

        [HttpPut("updateProfile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateUser(RegisterDto registerDto)
        {
            var user = await _userManager.FindByClaimsWithAddressAsync(HttpContext.User);
            var signIn = await _signIn.CheckPasswordSignInAsync(user, registerDto.Password, false);

            if (!signIn.Succeeded)
                return Unauthorized(new ApiResponse(401));

            user.UserName = registerDto.UserName;
            user.Email = registerDto.Email;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }

            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = result.Errors.Select(x => x.Description)
            });
        }

        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var user = await _userManager.FindByClaimsWithAddressAsync(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(address);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Problem updating the user"));
            else
                return Ok(_mapper.Map<Address, AddressDto>(user.Address));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized(new ApiResponse(401));

            var result = await _signIn.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (!result.Succeeded)
            {
                if(result.IsLockedOut)
                    return Unauthorized(new ApiResponse(401, "Too many failed attemps to login, please try again later"));
                else
                    return Unauthorized(new ApiResponse(401));
            }

            return new UserDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExist(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    {
                        "Email already in use"
                    }
                });
            }

            if (CheckUserExist(registerDto.UserName).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    {
                        "Username already in use"
                    }
                });
            }

            var user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }
    }
}
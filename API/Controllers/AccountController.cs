using System.Text;
using System.Linq;
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
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signIn;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSender;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signIn,
            ITokenService tokenService,
            IMapper mapper,
            IEmailSenderService emailSender)
        {
            _userManager = userManager;
            _signIn = signIn;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailSender = emailSender;
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
        public async Task<ActionResult<bool>> CheckEmailExist([Required][FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("userExists")]
        public async Task<ActionResult<bool>> CheckUserExist([FromQuery][Required] string username)
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
                if (result.IsLockedOut)
                    return Unauthorized(new ApiResponse(401, "Too many failed attemps to login, please try again later"));
                else if (result.IsNotAllowed)
                {
                    return Unauthorized(new ApiResponse(401, "Your email address isn't verfied"));
                }
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

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] [Required] [EmailAddress] string email, [FromQuery] [Required] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiResponse(400, "This email address doesn't exist"));
            
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            var resultToken = Encoding.UTF8.GetString(decodedToken);
            var result = await _userManager.ConfirmEmailAsync(user, resultToken);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            return Ok(true);
        }

        [HttpGet("requestConfirmationEmail")] 
        public async Task<IActionResult> RequestEmailConfirmation([FromQuery] [Required] [EmailAddress] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
                return BadRequest(new ApiResponse(400, "This email address doesn't exist"));

            if (user.EmailConfirmed)
                return BadRequest(new ApiResponse(400, "This email address is already confirmed"));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
            var encodedToken = WebEncoders.Base64UrlEncode(tokenBytes);
           
            await _emailSender.SendConfirmationEmailAsync(user.Email, encodedToken, user.UserName);

            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = result.Errors.Select(x => x.Description)
                });
            }
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
            var encodedToken = WebEncoders.Base64UrlEncode(tokenBytes);
           
            await _emailSender.SendConfirmationEmailAsync(user.Email, encodedToken, user.UserName);

            return Ok(true);
        }
    }
}
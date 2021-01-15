using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdministrationController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mappaer;

        public AdministrationController(UserManager<AppUser> userManager, IMapper mappaer)
        {
            _mappaer = mappaer;
            _userManager = userManager;

        }

        [HttpGet("users")]
        [Authorize(Policy = "RequireAdministration")]
        public async Task<ActionResult<ICollection<UserWithRolesDto>>> GetUsers()
        {
            var users = await _userManager.Users.OrderBy(x => x.UserName)
            .Include(x => x.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();
            var mappedUsers = _mappaer.Map<ICollection<AppUser>, ICollection<UserWithRolesDto>>(users);
            return Ok(mappedUsers);
        }

        [HttpPost("updateRoles/{username}")]
        [Authorize(Policy = "RequireAdministration")]
        public async Task<ActionResult> UpdateRoles([Required][FromRoute] string username, [FromBody] UpdateRolesDto updateRolesDto)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(user == null)
                return BadRequest(new ApiResponse(400));

            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = updateRolesDto.Roles ?? new string[] { };

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = result.Errors.Select(x => x.Description)
                });
            }

            return NoContent();
        }
    }
}
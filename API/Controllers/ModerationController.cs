using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ModerationController : BaseApiController
    {
        public ModerationController()
        {
            
        }
               
        // [HttpGet("reviews")]
        // [Authorize(Policy = "RequireModeration")]
        // public async Task<IActionResult> GetReviews()
        // {
        //     return Ok("Moderators & Administrators");
        // }
    }
}
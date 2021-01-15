using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UpdateRolesDto
    {
        [Required]
        public ICollection<string> Roles { get; set; }
    }
}
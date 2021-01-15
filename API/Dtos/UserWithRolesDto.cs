using System.Collections.Generic;

namespace API.Dtos
{
    public class UserWithRolesDto
    {
		public string Id { get; set; }
        public string UserName { get; set; }
        public ICollection<string> UserRoles { get; set; }
    }
}
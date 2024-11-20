using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class UserDto
    {
        public int Id { get; set; } // Unique identifier for the user
        public string Username { get; set; } // Username for the user
        public string Email { get; set; } // Email of the user
        public string Role { get; set; } // Role (e.g., Admin, User)
    }
}

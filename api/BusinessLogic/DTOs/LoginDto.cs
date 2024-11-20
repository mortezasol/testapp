using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } // Username provided by the user

        [Required]
        public string Password { get; set; } // Password provided by the user
    }
}

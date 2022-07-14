using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.API.Contracts.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "The username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.API.Contracts.Response
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string FinChatterUrl { get; set; }
        public string AvatarUrl { get; set; }
    }
}

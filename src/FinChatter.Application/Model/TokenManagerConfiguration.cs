using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Application.Model
{
    public class TokenManagerConfiguration
    {
        public  string Issuer { get; set; }
        public  string Audience { get; set; }
        public string IssuerSigningKey { get; set; }
    }
}

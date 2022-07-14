
namespace FinChatter.Application.Model
{
    public class TokenManagerConfiguration
    {
        public  string Issuer { get; set; }
        public  string Audience { get; set; }
        public string IssuerSigningKey { get; set; }
        public double ExpirationInMinutes { get; set; }
    }
}

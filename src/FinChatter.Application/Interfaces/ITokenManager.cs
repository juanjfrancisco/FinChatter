using FinChatter.Application.Model;

namespace FinChatter.Application.Interfaces
{
    public  interface ITokenManager
    {
        string IssuerSigningKey { get; }
        double ExpirationInMinutes { get; }
        Task<Result<string>> GenerateToken(string secret, double expirationMinutes, Dictionary<string, string> claims);
        Task<Result<Dictionary<string, string>>> ValidateToken(string token, string secret);
    }
}

using FinChatter.Application.Interfaces;
using FinChatter.Application.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Infrastructure.Services
{
    internal class TokenManager : ITokenManager
    {
        private readonly string Issuer;
        private readonly string Audience;
        private readonly string SecAlgorithms;

        public string IssuerSigningKey { get; private set; }

        public double ExpirationInMinutes { get; private set; }

        public TokenManager(IOptions<TokenManagerConfiguration> tokenOptions)
        {
            SecAlgorithms = SecurityAlgorithms.HmacSha512Signature;
            Issuer = tokenOptions.Value.Issuer;
            Audience = tokenOptions.Value.Audience;
            IssuerSigningKey = tokenOptions.Value.IssuerSigningKey;
            ExpirationInMinutes = tokenOptions.Value.ExpirationInMinutes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="expirationMinutes"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task<Result<string>> GenerateToken(string secret, double expirationMinutes, Dictionary<string, string> claims)
        {
            var result = new Result<string>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecAlgorithms),
                NotBefore = DateTime.Now,
                Issuer = Issuer,
                Audience = Audience,
            };

            if (claims != null && claims.Count() > 0)
            {
                tokenDescriptor.Subject = new ClaimsIdentity();
                claims.ToList()
                .ForEach(claim =>
                {
                    tokenDescriptor.Subject.AddClaim(new Claim(claim.Key, claim.Value));
                });
            }

            tokenDescriptor.Expires = DateTime.Now.AddMinutes(expirationMinutes);

            var secToken = tokenHandler.CreateToken(tokenDescriptor);
            result.Data = tokenHandler.WriteToken(secToken);
            result.IsSuccess = !string.IsNullOrEmpty(result.Data);
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public async Task<Result<Dictionary<string, string>>> ValidateToken(string token, string secret)
        {
            var result = new Result<Dictionary<string, string>>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var creds = new SigningCredentials(key, SecAlgorithms);

            var validator = new JwtSecurityTokenHandler();

            // These need to match the values used to generate the token
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidIssuer = Issuer;
            validationParameters.ValidAudience = Audience;
            validationParameters.IssuerSigningKey = key;
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidateAudience = true;

            if (validator.CanReadToken(token))
            {
                //ClaimsPrincipal principal;
                try
                {
                    // This line throws if invalid
                    var tokenResult = await validator.ValidateTokenAsync(token, validationParameters); // out validatedToken

                    // Validar claims
                    if (tokenResult.Claims != null && tokenResult.Claims.Count() > 0)
                    {
                        result.Data = new Dictionary<string, string>();
                        tokenResult.Claims.ToList()
                            .ForEach(claim =>
                            {
                                result.Data.Add(claim.Key, claim.Value.ToString());
                            });
                    }
                    //Todo: CommonMessage 
                    //else
                    //    CommonMessage.SetMessage(CommonMessage.ERROR_INVALID_TOKEN, ref result);
                }
                catch (Exception e)
                {
                    //CommonMessage.SetMessage(CommonMessage.ERROR_EXCEPTION, ref result, e.Message);
                }
            }
            else
            {
                //CommonMessage.SetMessage(CommonMessage.ERROR_INVALID_TOKEN, ref result);
            }

            return result;
        }
    }
}

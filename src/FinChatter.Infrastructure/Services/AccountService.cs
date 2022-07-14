using FinChatter.API.Contracts.Request;
using FinChatter.API.Contracts.Response;
using FinChatter.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Infrastructure.Services
{
    internal class AccountService : IAccountService
    {
        private readonly ITokenManager _tokenManager;
        public AccountService(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }
        public async Task<ClientResponse<LoginResponse>> Login(LoginRequest request)
        {
            var result = new ClientResponse<LoginResponse>();

            if (!await ValidateUser(request))
                return GetUnauthorizedMessage(result);

            var claims = new Dictionary<string, string>() { { "username", request.UserName } };

            var tokenResponse = await _tokenManager.GenerateToken(_tokenManager.IssuerSigningKey, _tokenManager.ExpirationInMinutes, claims);
            if (tokenResponse == null || !tokenResponse.IsSuccess || string.IsNullOrEmpty(tokenResponse.Data))
                return GetUnauthorizedMessage(result);

            result.Data = new LoginResponse
            {
                UserName = request.UserName,
                AvatarUrl = "/images/demo.png",
                Token = tokenResponse.Data
            };

            return result;
        }

        public async Task<ClientResponse<RegisterResponse>> RegisterUser(RegisterRequest request)
        {
            return new ClientResponse<RegisterResponse>();
        }

        private static ClientResponse<LoginResponse> GetUnauthorizedMessage(ClientResponse<LoginResponse> result)
        {
            result.StatusCode = 401;
            result.Code = 401;
            result.Message = "Unauthorized";
            return result;
        }

        private async Task<bool> ValidateUser(LoginRequest request)
        {
            return true;
        }

    }
}

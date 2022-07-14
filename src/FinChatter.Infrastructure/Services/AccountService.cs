using FinChatter.API.Contracts.Request;
using FinChatter.API.Contracts.Response;
using FinChatter.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Infrastructure.Services
{
    internal class AccountService : IAccountService
    {
        private readonly ITokenManager _tokenManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountService(ITokenManager tokenManager, UserManager<IdentityUser> userManager)
        {
            _tokenManager = tokenManager;
            _userManager = userManager;
        }

        public async Task<ClientResponse<RegisterResponse>> RegisterUser(RegisterRequest request)
        {
            ClientResponse<RegisterResponse> result = ValidateRegisterUser(request);
            if (!result.IsSuccess)
                return result;
                
            var user = new IdentityUser { 
                UserName = request.UserName, 
                Email = request.EmailAddress,  
            };

            var createResponse = await _userManager.CreateAsync(user, request.Password);
            if (!createResponse.Succeeded)
            {
                var errors = createResponse.Errors.Select(e => e.Description);
                result.IsSuccess = false;
                result.Message = "Sorry, there were validation errors. See detail";
                result.ValidationError = errors;
            }

            return result;
        }

        private ClientResponse<RegisterResponse> ValidateRegisterUser(RegisterRequest request)
        {
            var result = new ClientResponse<RegisterResponse>();
            if (request == null)
            {
                result.IsSuccess = false;
                result.StatusCode = 400;
                result.Message = "Invalid request";
                return result;
            }
            return result; 
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

        private static ClientResponse<LoginResponse> GetUnauthorizedMessage(ClientResponse<LoginResponse> result)
        {
            result.StatusCode = 401;
            result.Code = 401;
            result.Message = "Invalid username or password. Please try again.";
            result.IsSuccess = false;
            return result;
        }

        private async Task<bool> ValidateUser(LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserName) 
                || string.IsNullOrEmpty(request.Password))
                return false;

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null &&
                await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return true;
            }

            return false;
        }

    }
}

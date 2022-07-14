using FinChatter.API.Contracts;
using FinChatter.API.Contracts.Request;
using FinChatter.API.Contracts.Response;
using FinChatter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FinChatter.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        public AccountController(IConfiguration configuration, IAccountService accountService)
        {
            _configuration = configuration;
            _accountService = accountService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ClientResponse<RegisterResponse>> RegisterUser(RegisterRequest request)
        {
            var response = await _accountService.RegisterUser(request);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ClientResponse<LoginResponse>> Login(LoginRequest request)
        {
            var response = await _accountService.Login(request);

            if(response.IsSuccess && response.Data != null)
                response.Data.FinChatterUrl = GetHubUrl();

            return response;
        }

        private string GetHubUrl()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var hubUrl = $"{baseUrl}{_configuration.GetValue<string>("FinChatterHub")}";
            return hubUrl;
        }
    }
}

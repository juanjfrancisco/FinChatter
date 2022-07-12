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
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ClientResponse<RegisterResponse>> RegisterUser(RegisterRequest request)
        {
            
            Response.StatusCode = (int)System.Net.HttpStatusCode.Created;
            return new ClientResponse<RegisterResponse> (System.Net.HttpStatusCode.Created) { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ClientResponse<LoginResponse>> Login(LoginRequest request)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var hubUrl = $"{baseUrl}{_configuration.GetValue<string>("FinChatterHub")}";
            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            return new ClientResponse<LoginResponse>(System.Net.HttpStatusCode.OK) { 
                Data = new LoginResponse
                {
                    AvatarUrl = "/images/demo.png",
                    FinChatterUrl = hubUrl,
                    Token = "",
                    UserName = request.UserName
                }
            };
        }
    }
}

using FinChatter.API.Contracts.Request;
using FinChatter.API.Contracts.Response;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.API.Client
{
    public interface IFinChatterApiClient
    {
        [Post("/api/account/register")]
        Task<ClientResponse<RegisterResponse>> Register([Body] RegisterRequest request);


        [Post("/api/account/login")]
        Task<ClientResponse<LoginResponse>> Login([Body] LoginRequest request);
    }
}

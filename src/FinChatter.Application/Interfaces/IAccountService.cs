using FinChatter.API.Contracts.Request;
using FinChatter.API.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Application.Interfaces
{
    public  interface IAccountService
    {
        Task<ClientResponse<RegisterResponse>> RegisterUser(RegisterRequest request);
        Task<ClientResponse<LoginResponse>> Login(LoginRequest request);
    }
}

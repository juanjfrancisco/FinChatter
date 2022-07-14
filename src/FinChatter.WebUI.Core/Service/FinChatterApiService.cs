using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinChatter.API.Client;
using FinChatter.API.Contracts.Request;
using FinChatter.API.Contracts.Response;

namespace FinChatter.WebUI.Core.Service
{
    public class FinChatterApiService
    {
        private readonly IFinChatterApiClient _apiClient;

        public FinChatterApiService(IFinChatterApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ClientResponse<RegisterResponse>> Register(RegisterRequest request) => await _apiClient.Register(request);
        public async Task<ClientResponse<LoginResponse>> Login(LoginRequest request) => await _apiClient.Login(request);
    }
}

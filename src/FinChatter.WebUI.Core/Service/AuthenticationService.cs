using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using FinChatter.API.Client;
using FinChatter.API.Contracts.Request;
using FinChatter.API.Contracts.Response;
using FinChatter.WebUI.Core.AuthProviders;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinChatter.WebUI.Core.Service
{
    public class AuthenticationService
    {
        private readonly HttpClient _client;
        private readonly IFinChatterApiClient _apiClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(IFinChatterApiClient apiClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, HttpClient client)
        {
            _apiClient = apiClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _client = client;
        }

        //todo: add try catch
        public async Task<ClientResponse<RegisterResponse>> Register(RegisterRequest request)
        {
            return await _apiClient.Register(request);
        }
        public async Task<ClientResponse<LoginResponse>> Login(LoginRequest request)
        {
            var result = await _apiClient.Login(request);
            if (!result.IsSuccess)
                return result;

            await _localStorage.SetItemAsync("authToken", result.Data.Token);
            await _localStorage.SetItemAsync("finChatterUrl", result.Data.FinChatterUrl);
            await _localStorage.SetItemAsync("avatarUrl", result.Data.AvatarUrl);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(request.UserName);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Data.Token);

            return result;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("finChatterUrl");
            await _localStorage.RemoveItemAsync("avatarUrl");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}

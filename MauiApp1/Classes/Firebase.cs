using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiApp1.Classes
{
    public class Firebase
    {
        private static Firebase instance;

        private const string ApiKey = "AIzaSyDLzy3mB2vWXGECc184LGdR9cKPQJar84w";
        private const string projectId = "capstone-fb6b0";
        private const string collection = "settings";
        private readonly HttpClient _httpClient = new();

        private const string SignUpUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}";
        private const string SignInUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}";
        private const string CollectionUrl = $"https://firestore.googleapis.com/v1/projects/{projectId}/databases/(default)/documents/{collection}?key={ApiKey}";

        //This is needed to prevent outside instantiation since its a singleton. So please don't delete
        private Firebase() { }

        public static Firebase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Firebase();
                }
                return instance;
            }
        }

        private class AuthResponse
        {
            public string idToken { get; set; }
            public string email { get; set; }
        }

        public async Task<bool> HandleAuthResponse(HttpResponseMessage response)
        {

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<AuthResponse>(content);

                if (jsonResponse?.idToken != null)
                {
                    await SecureStorage.SetAsync("firebase_token", jsonResponse.idToken);
                    await SecureStorage.SetAsync("firebase_user", jsonResponse.email);
                }
                return true;

            }
            return false;
        }

        public async Task<bool> SignUpAsync(string email, string password)
        {
            var payload = new {email, password, returnSecureToken = true};
            var json = JsonSerializer.Serialize(payload);
            var response = await _httpClient.PostAsync(SignUpUrl, new StringContent(json, Encoding.UTF8, "application/json"));

            return await HandleAuthResponse(response);
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            var payload = new { email, password, returnSecureToken = true };
            var json = JsonSerializer.Serialize(payload);
            var response = await _httpClient.PostAsync(SignInUrl, new StringContent(json, Encoding.UTF8, "application/json"));

            return await HandleAuthResponse(response);
        }

        public async Task<bool> IsUserSignedInAsync()
        {
            var token = await SecureStorage.GetAsync("firebase_token");
            return !string.IsNullOrEmpty(token);
        }
        public static void Logout()
        {
            SecureStorage.Remove("firebase_token");
            SecureStorage.Remove("firebase_user");
        }

        public async Task<bool> AddUserSetting()
        {
            var userId = await SecureStorage.GetAsync("firebase_token");

            var data = new
            {
                fields = new
                {
                    name = new
                    {
                        stringValue = "test"
                    },
                    time = new
                    {
                        integerValue = Globals.DeviceSeconds
                    },
                    strength = new
                    {
                        integerValue = Globals.DeviceStrength
                    },
                    userId = new
                    {
                        stringValue = userId
                    }
                }
            };

            string json = JsonSerializer.Serialize(data);
            var request = new HttpRequestMessage(HttpMethod.Post, CollectionUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userId);
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;

        }


    }
}

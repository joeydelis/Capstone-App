using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        private const string RefreshUrl = $"https://securetoken.googleapis.com/v1/token?key={ApiKey}";

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
            public string localId { get; set; }
            public string refreshToken { get; set; }
        }

        public class PresetData
        {
            public string Name { get; set; }
            public int Time { get; set; }
            public int Strength { get; set; }
            public string UserId { get; set; }
            public string Id { get; set; }
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
                    await SecureStorage.SetAsync("firebase_uuid", jsonResponse.localId);
                    await SecureStorage.SetAsync("firebase_refresh", jsonResponse.refreshToken);
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
            var token = await SecureStorage.GetAsync("firebase_uuid");
            return !string.IsNullOrEmpty(token);
        }
        public static void Logout()
        {
            SecureStorage.Remove("firebase_token");
            SecureStorage.Remove("firebase_uuid");
            SecureStorage.Remove("firebase_refresh");
        }

        public async Task<bool> AddUserSetting(string name)
        {

            /*
             * This chunk of code is in a couple functions because an auth token is required in the header to do crud operations.
             * However, the idToken given on signIn/signUp expires so a new one must be retrieved with the refresh token occasionally.
             */
            var token = await SecureStorage.GetAsync("firebase_token");
            bool isExpired = await IsIdTokenExpired(token);
            if (isExpired)
            {
                bool isRefreshed = await RefreshIdTokenAsync();
                if (isRefreshed)
                {
                    token = await SecureStorage.GetAsync("firebase_token");
                }
                else
                {
                    return false;
                }
            }
            var uuid = await SecureStorage.GetAsync("firebase_uuid");


            var data = new
            {
                fields = new
                {
                    name = new
                    {
                        stringValue = name
                    },
                    time = new
                    {
                        //Time is stored as just minutes for simplicity
                        integerValue = (Globals.DeviceMinutes * 60) + Globals.DeviceSeconds
                    },
                    strength = new
                    {
                        integerValue = Globals.DeviceStrength
                    },
                    userId = new
                    {
                        stringValue = uuid
                    }
                }
            };

            string json = JsonSerializer.Serialize(data);
            var request = new HttpRequestMessage(HttpMethod.Post, CollectionUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;

        }

        public async Task<List<PresetData>> GetUserPresetsAsync()
        {
            var token = await SecureStorage.GetAsync("firebase_token");
            bool isExpired = await IsIdTokenExpired(token);
            if (isExpired)
            {
                bool isRefreshed = await RefreshIdTokenAsync();
                if (isRefreshed)
                {
                    token = await SecureStorage.GetAsync("firebase_token");
                } else
                {
                    return null;
                }
            }
            var uuid = await SecureStorage.GetAsync("firebase_uuid");

            var request = new HttpRequestMessage(HttpMethod.Get, CollectionUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var presets = new List<PresetData>();

                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    if (doc.RootElement.TryGetProperty("documents", out JsonElement documents))
                    {
                        foreach(JsonElement document in documents.EnumerateArray())
                        {
                            var fields = document.GetProperty("fields");

                            if (fields.GetProperty("userId").GetProperty("stringValue").GetString() == uuid)
                            {
                                presets.Add(new PresetData
                                {
                                    Id = document.GetProperty("name").GetString()?.Split('/').Last(),
                                    Name = fields.GetProperty("name").GetProperty("stringValue").GetString(),
                                    Time = int.Parse(fields.GetProperty("time").GetProperty("integerValue").GetString()),
                                    Strength = int.Parse(fields.GetProperty("strength").GetProperty("integerValue").GetString()),
                                    UserId = fields.GetProperty("userId").GetProperty("stringValue").GetString()
                                });
                            }
                        }
                    }
                }
                return presets;
            } else
            {
                return null;
            }

        }

        public async Task<bool> IsIdTokenExpired(string idToken)
        {
            //idTokens are json web tokens and can be parsed into JSON with a time field that can be retrieved and used to check if token is expired.
            try
            {
                var payload = idToken.Split('.')[1];
                var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(payload + new string('=', (4 - payload.Length % 4) % 4)));

                using (JsonDocument doc = JsonDocument.Parse(payloadJson))
                {
                    var root = doc.RootElement;
                    long exp = root.GetProperty("exp").GetInt64();
                    long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    return currentUnixTime >= exp;
                }
            }
            catch
            {
                return true;
            }
        }

        public async Task<bool> DeleteUserPresetAsync(string documentId)
        {
            var token = await SecureStorage.GetAsync("firebase_token");
            bool isExpired = await IsIdTokenExpired(token);
            if (isExpired)
            {
                bool isRefreshed = await RefreshIdTokenAsync();
                if (isRefreshed)
                {
                    token = await SecureStorage.GetAsync("firebase_token");
                }
                else
                {
                    return false;
                }
            }

            //Url is document dependent so easier to have here instead of top of class with other urls.
            string deleteUrl = $"https://firestore.googleapis.com/v1/projects/{projectId}/databases/(default)/documents/{collection}/{documentId}?key={ApiKey}";

            var request = new HttpRequestMessage(HttpMethod.Delete, deleteUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;

        }

        public async Task<bool> RefreshIdTokenAsync()
        {
            var refreshToken = await SecureStorage.GetAsync("firebase_refresh");

            var payload = new
            {
                grant_type = "refresh_token",
                refresh_token = refreshToken
            };

            var json = JsonSerializer.Serialize(payload);
            var response = await _httpClient.PostAsync(RefreshUrl, new StringContent(json, Encoding.UTF8, "application/json"));

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                using(JsonDocument doc = JsonDocument.Parse(content))
                {
                    var root = doc.RootElement;
                    string newIdToken = root.GetProperty("id_token").GetString();
                    string newRefreshToken = root.GetProperty("refresh_token").GetString();

                    await SecureStorage.SetAsync("firebase_token", newIdToken);
                    await SecureStorage.SetAsync("firebase_refresh", newRefreshToken);
                }

                return true;
            } else
            {
                return false;
            }
        }

        public void LoadPreset(int time, int strength)
        {
            Globals.DeviceStrength = strength;
            Globals.DeviceMinutes = time / 60;
            Globals.DeviceSeconds = time % 60;
        }

    }
}

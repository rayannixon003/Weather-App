using Supabase;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Frontend.Models;

namespace WeatherApp.Frontend.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabaseClient;

        public AuthService(string supabaseUrl, string supabaseApiKey)
        {
            _supabaseClient = new Supabase.Client(supabaseUrl, supabaseApiKey, new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            });
        }

        // Register User (Sign-Up)
        public async Task<Session?> RegisterUser(string email, string password, string phoneNumber, string name)
        {
            try
            {
                var options = new SignUpOptions
                {
                    Data = new Dictionary<string, object>
                    {
                        { "phone", phoneNumber },
                        { "name", name }
                    }
                };

                var session = await _supabaseClient.Auth.SignUp(email, password, options);
                return session;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration Error: {ex.Message}");
                return null;
            }
        }

        // Login User
        public async Task<Session?> LoginUser(string email, string password)
        {
            try
            {
                var session = await _supabaseClient.Auth.SignIn(email, password);
                return session;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.Message}");
                return null;
            }
        }

        // ✅ FIXED: Fetch User Data using `CurrentSession`
        public async Task<UserModel?> GetUserData()
        {
            try
            {
                var session = _supabaseClient.Auth.CurrentSession; // ✅ Correct way to get session
                if (session != null && session.AccessToken != null)
                {
                    var user = await _supabaseClient.Auth.GetUser(session.AccessToken);
                    if (user != null)
                    {
                        return new UserModel
                        {
                            Id = user.Id,
                            Email = user.Email,
                            Name = user.UserMetadata.ContainsKey("name") ? user.UserMetadata["name"].ToString()! : "Unknown",
                            Phone = user.UserMetadata.ContainsKey("phone") ? user.UserMetadata["phone"].ToString()! : "Not Provided"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user data: {ex.Message}");
            }
            return null;
        }

        // Logout User
        public async Task LogoutUser()
        {
            await _supabaseClient.Auth.SignOut();
        }
    }
}

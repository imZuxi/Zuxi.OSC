// /*
//  *
//  * Zuxi.OSC - VRChatAPIClient.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

/*
 * Zuxi.OSC - VRChatAPIClient.cs
 * Copyright 2023 - 2024 Zuxi and contributors
 * https://zuxi.dev
 *
 */

using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Zuxi.OSC.Modules.FriendRequest.Json;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Modules.FriendRequests;

/// <summary>
/// A client for interacting with the VRChat API, providing methods for authentication, user data retrieval,
/// notifications, and other API functionalities.
/// </summary>


internal class VRChatAPIClient
{
    private static VRChatAPIClient VrChatApiClient { get; set; }
    internal HttpClient _httpClient { get; set; }
    private const string _VRChatBaseEndpoint = "https://api.vrchat.cloud/api/1/";
    internal HttpClientHandler httpClientHandler = null;
    /// <summary>
    /// Initializes a new instance of the <see cref="VRChatAPIClient"/> class with the necessary HTTP headers and cookies.
    /// </summary>
    public VRChatAPIClient()
    {
        httpClientHandler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer(),
            UseCookies = true
        };

        _httpClient = new HttpClient(httpClientHandler);
        httpClientHandler.CookieContainer.Add(new Cookie("auth", Config.GetInstance().AuthCookie)


        { Domain = "api.vrchat.cloud", Path = "/" });
        httpClientHandler.CookieContainer.Add(new Cookie("twoFactorAuth", Config.GetInstance().twoFactorAuthCookie)
        { Domain = "api.vrchat.cloud", Path = "/" });
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ZuxiJapi%2F4.0.0%20vrchat%40mail.imzuxi.com");
        _httpClient.BaseAddress = new Uri(_VRChatBaseEndpoint);
        VrChatApiClient = this;
    }

    /// <summary>
    /// Retrieves the singleton instance of the <see cref="VRChatAPIClient"/> class.
    /// </summary>
    /// <returns>The single instance of <see cref="VRChatAPIClient"/>.</returns>
    internal static VRChatAPIClient GetInstance()
    {
        if (VrChatApiClient is null) VrChatApiClient = new VRChatAPIClient();

        return VrChatApiClient;
    }

    /// <summary>
    /// Checks the current authentication status with the VRChat API.
    /// </summary>
    /// <returns>A JSON string indicating the authentication status.</returns>
    public string CheckAuthStatus()
    {
        return MakeAPIGetRequest("auth");
    }

    /// <summary>
    /// Retrieves the authenticated user's information.
    /// </summary>
    /// <returns>A <see cref="VRCUser"/> object containing the authenticated user's details.</returns>
    public VRCUser GetLocalUser()
    {
        return new VRCUser(MakeAPIGetRequest("auth/user"));
    }

    /// <summary>
    /// Retrieves notifications for the authenticated user from the VRChat API.
    /// </summary>
    /// <returns>A JSON string containing user notifications.</returns>
    public string GetUserNotis()
    {
        return MakeAPIGetRequest("auth/user/notifications");
    }

    /// <summary>
    /// Fetches detailed information about a specific user by their user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve.</param>
    /// <returns>A <see cref="VRCPlayer"/> object containing user details.</returns>
    public VRCPlayer GetVRCUserByID(string userId)
    {
        return new VRCPlayer(MakeAPIGetRequest($"users/{userId}"));
    }

    /// <summary>
    /// Accepts a friend request using the provided friend request ID.
    /// </summary>
    /// <param name="frid">The friend request ID to accept.</param>
    /// <returns>True if the friend request was successfully accepted, otherwise false.</returns>
    public bool AcceptRequest(string frid)
    {
        if (!frid.ToLower().Contains("frq_"))
            return false;

        if (MakeAPIPutRequest($"auth/user/notifications/{frid}/accept", null) != "[]")
            return true;

        return false;
    }

    /// <summary>
    /// Sends a GET request to the specified VRChat API endpoint and returns the response.
    /// </summary>
    /// <param name="apiendpoint">The API endpoint to query.</param>
    /// <returns>A JSON string with the API response.</returns>
    public string MakeAPIGetRequest(string apiendpoint)
    {

        var response = _httpClient.GetAsync(apiendpoint).Result;

        if (response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully Made Request to: {0} ", apiendpoint);
            Console.ForegroundColor = ConsoleColor.Cyan;
            return content;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Request to: {0} failed with status code {1} ", apiendpoint, response.StatusCode);
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Response content: " + content);
            Console.ForegroundColor = ConsoleColor.Cyan;
            return content;
        }
    }

    /// <summary>
    /// Sends a PUT request to the specified VRChat API endpoint with the provided data payload.
    /// </summary>
    /// <param name="apiendpoint">The API endpoint to query.</param>
    /// <param name="data">The JSON-formatted payload to include in the PUT request.</param>
    /// <returns>A JSON string with the API response.</returns>
    public string MakeAPIPutRequest(string apiendpoint, string data)
    {
        var response = _httpClient.PutAsync(apiendpoint,
            new StringContent(string.IsNullOrEmpty(data) ? "" : data, Encoding.UTF8, "application/json")).Result;

        if (response.IsSuccessStatusCode)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully Made Request to: {0} ", apiendpoint);
            Console.ForegroundColor = ConsoleColor.Cyan;
            return response.Content.ReadAsStringAsync().Result;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Request to: {0} failed with status code {1} ", apiendpoint, response.StatusCode);
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Response content: " + content);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        return "[]";
    }

    // Note: This class is intentionally separate and only references VRChatAPIClient as a parameter to set the VRChatAPIClient Auth Values. 
    // It is meant exclusively for authentication and its flow. Use this method only when logging in 
    // without an auth cookie. However, for better security and reliability, always use an auth cookie when possible. - Zuxi

    /// <summary>
    /// Handles the authentication flow for VRChat, including basic and two-factor authentication. 
    /// </summary>
    public class VRChatAuthenticationFlow
    {
        /// <summary>
        /// Executes the authentication flow for the VRChat API client.
        /// </summary>
        /// <param name="APIClient">The VRChat API client.<see cref="VRChatAPIClient"/></param>
        /// <returns>True if authentication succeeds, otherwise false.</returns>
        public static bool DoAuthFlow(VRChatAPIClient APIClient)
        {
            try
            {
                // Wait for other modules to initialize.
                Task.Delay(5000).Wait();

                string authValue = Config.GetInstance().VRCAuthValue;
                Console.WriteLine(authValue ?? "No saved authentication value found.");

                if (string.IsNullOrEmpty(authValue))
                {
                    Console.WriteLine("No auth token found. Starting the authentication flow...");
                    authValue = GetAuthString();
                }

                APIClient._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                // Authenticate and retrieve the local user.
                Console.WriteLine("Authenticating...");
                var response = APIClient._httpClient.GetAsync("auth/user").Result;

                if (response.IsSuccessStatusCode)
                {
                    string vrcResponse = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(vrcResponse);

                    if (vrcResponse.Contains("requiresTwoFactorAuth") && string.IsNullOrEmpty(Config.GetInstance().twoFactorAuthCookie))
                    {
                        if (!DoTwoFactorFlow(APIClient))
                        {
                            Console.WriteLine("Two-factor authentication failed.");
                            return false;
                        }
                    }

                    Console.WriteLine("Authentication successful!");
                    APIClient._httpClient.DefaultRequestHeaders.Authorization = null;
                    Config.GetInstance().VRCAuthValue = authValue;
                    SaveCreds(APIClient);
                    Config.GetInstance().Save();
                    return true;
                }
                else
                {
                    HandleFailedAuthentication(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during authentication: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Handles two-factor authentication for the VRChat API client.
        /// </summary>
        /// <param name="APIClient">The VRChat API client. <see cref="VRChatAPIClient"/></param>
        /// <returns>True if two-factor authentication succeeds, otherwise false.</returns>
        public static bool DoTwoFactorFlow(VRChatAPIClient APIClient)
        {
            string twoFactorCode;
            do
            {
                Console.WriteLine("Enter your two-factor authentication code (6 digits):");
                twoFactorCode = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(twoFactorCode) || twoFactorCode.Length != 6);

            try
            {
                string payload = JsonConvert.SerializeObject(new { code = twoFactorCode });
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                Console.WriteLine("Authenticating two-factor...");
                var response = APIClient._httpClient.PostAsync("auth/twofactorauth/totp/verify", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Two-factor authentication successful!");
                    return true;
                }
                else
                {
                    HandleFailedAuthentication(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during two-factor authentication: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Saves authentication credentials (auth and two-factor cookies) for later use.
        /// </summary>
        /// <param name="APIClient">The VRChat API client. <see cref="VRChatAPIClient"/></param>
        public static void SaveCreds(VRChatAPIClient APIClient)
        {
            Uri vrChatUri = new Uri("https://api.vrchat.cloud");
            CookieCollection cookies = APIClient.httpClientHandler.CookieContainer.GetCookies(vrChatUri);

            foreach (Cookie cookie in cookies)
            {
                if (cookie.Name == "auth" && !string.IsNullOrEmpty(cookie.Value))
                    Config.GetInstance().AuthCookie = cookie.Value;

                if (cookie.Name == "twoFactorAuth" && !string.IsNullOrEmpty(cookie.Value))
                    Config.GetInstance().twoFactorAuthCookie = cookie.Value;

                Console.WriteLine($"Cookie saved: {cookie.Name} = {cookie.Value}");
            }

            Config.GetInstance().Save();
        }

        /// <summary>
        /// Prompts the user for their VRChat username and password, and returns a Base64-encoded authentication string.
        /// </summary>
        /// <returns>A Base64-encoded string of username and password.</returns>
        public static string GetAuthString()
        {
            string username, password;

            do
            {
                Console.WriteLine("Please enter your VRChat username:");
                username = Console.ReadLine();

                Console.WriteLine("Please enter your VRChat password:");
                password = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password));

            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        }

        /// <summary>
        /// Handles failed authentication attempts by logging the error response.
        /// </summary>
        /// <param name="response">The HTTP response indicating failure.</param>
        private static void HandleFailedAuthentication(HttpResponseMessage response)
        {
            Console.WriteLine($"Authentication failed. Status code: {response.StatusCode}");
            string responseContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseContent);

            if (responseContent.Contains("Invalid Username/Email or Password"))
            {
                Console.WriteLine("Invalid username or password. Please restart to retry the setup.");
            }
        }
    }

}

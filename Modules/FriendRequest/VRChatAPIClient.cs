/*
 * Zuxi.OSC - VRChatAPIClient.cs
 * Copyright 2023 - 2024 Zuxi and contributors
 * https://zuxi.dev
 *
 */

using System.Net;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="VRChatAPIClient"/> class with the necessary HTTP headers and cookies.
    /// </summary>
    public VRChatAPIClient()
    {
        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer(),
            UseCookies = true
        };

        _httpClient = new HttpClient(httpClientHandler);
        httpClientHandler.CookieContainer.Add(new Cookie("auth", Config.AuthCookie)
        { Domain = "api.vrchat.cloud", Path = "/" });
        httpClientHandler.CookieContainer.Add(new Cookie("twoFactorAuth", Config.twoFactorAuthCookie)
        { Domain = "api.vrchat.cloud", Path = "/" });
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ZuxiJapi%2F4.0.0%20vrchat%40mail.imzuxi.com");
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
        var url = "https://api.vrchat.cloud/api/1/" + apiendpoint;
        var response = _httpClient.GetAsync(url).Result;

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
        var response = _httpClient.PutAsync(_VRChatBaseEndpoint + apiendpoint,
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
}

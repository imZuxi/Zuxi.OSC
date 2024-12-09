// /*
//  *
//  * Zuxi.OSC - HClient.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using System.Net;
using System.Text;
using Zuxi.OSC.Modules.FriendRequest.Json;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Modules.FriendRequests;

internal class VRChatAPIClient
{
    private static VRChatAPIClient VrChatApiClient { get; set; }
    internal HttpClient _httpClient { get; set; }
    private const string _VRChatBaseEndpoint = "https://api.vrchat.cloud/api/1/";

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

    internal static VRChatAPIClient GetInstance()
    {
        if (VrChatApiClient is null) VrChatApiClient = new VRChatAPIClient();

        return VrChatApiClient;
    }


    /// <summary>
    /// Checks the authentication status by sending a GET request to the VRChat API.
    /// </summary>
    /// <returns>A JSON string containing the authentication status retrieved from the VRChat API.</returns>
    public string CheckAuthStatus()
    {
        return MakeAPIGetRequest("auth");
    }

    /// <summary>
    /// Retrieves the authenticated user's information by sending a GET request to the VRChat API.
    /// </summary>
    /// <returns>A JSON string containing the authenticated user's information retrieved from the VRChat API.</returns>
    public string GetLocalUser()
    {
        return MakeAPIGetRequest("auth/user");
    }

    /// <summary>
    /// Retrieves the authenticated user's notifications by sending a GET request to the VRChat API.
    /// </summary>
    /// <returns>A JSON string containing the authenticated user's notifications retrieved from the VRChat API.</returns>
    public string GetUserNotis()
    {
        return MakeAPIGetRequest("auth/user/notifications");
    }

    /// <summary>
    /// Retrieves user information from the VRChat API based on the provided user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the VRChat user.</param>
    /// <returns>A JSON string containing the VRChat user data retrieved from the API.</returns>
    public VRCPlayer GetVRCUserByID(string userId)
    {
        return VRCPlayer.CreateVRCPlayer(MakeAPIGetRequest($"users/{userId}"));
    }

    public bool AcceptRequest(string frid)
    {
        if (!frid.ToLower().Contains("frq_"))
            return false;

        if (MakeAPIPutRequest($"auth/user/notifications/{frid}/accept", null) != "[]")
            return true;

        return false;
    }

    /// <summary>
    /// Sends a GET request to the specified VRChat API endpoint and retrieves data.
    /// </summary>
    /// <param name="apiendpoint">The endpoint of the VRChat API to which the request is sent.</param>
    /// <returns>A JSON response from the VRChat API containing the requested data.</returns>
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
            //  Console.WriteLine("Request failed with status code: " + response.StatusCode);
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Response content: " + content);
            Console.ForegroundColor = ConsoleColor.Cyan;
            return content;
        }

        return "[]";
    }

    /// <summary>
    /// Sends a PUT request to the specified VRChat API endpoint with the provided data.
    /// </summary>
    /// <param name="apiendpoint">The endpoint of the VRChat API to which the request is sent.</param>
    /// <param name="data">The data to be included in the PUT request.</param>
    /// <returns>A JSON response from the VRChat API containing the result of the request.</returns>
    public string MakeAPIPutRequest(string apiendpoint, string data)
    {
        var response = _httpClient.PutAsync(_VRChatBaseEndpoint + apiendpoint,
            new StringContent(string.IsNullOrEmpty(data) ? "" : data, Encoding.UTF8, "application/json")).Result;

        // Handle the response
        if (response.IsSuccessStatusCode)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully Made Request to: {0} ", apiendpoint);
            Console.ForegroundColor = ConsoleColor.Cyan;
            return response.Content.ReadAsStringAsync().Result;
            ;
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

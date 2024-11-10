// /*
//  *
//  * Zuxi.OSC - Config.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace Zuxi.OSC.utility;

public class Config
{
    public static string AuthCookie { get; set; }
    public static string twoFactorAuthCookie { get; set; }
    public static List<string> IgnoredFriendRequests { get; set; }

    private static readonly string filePath = "data.json"; // Change the file path as needed
    public static string HypeRateID { get; set; }
    public static string HypeRateSecretToken { get; set; }
    public static string Bio { get; set; }

    public static int VRChatOSCPort { get; set; } = 9000;

    public static void SaveData()
    {
        var jsonData = JsonConvert.SerializeObject(new
        {
            AuthCookie,
            twoFactorAuthCookie,
            HypeRateID,
            HypeRateSecretToken,
            Bio,
            VRChatOSCPort,
            // ALWAYS ADD BIG LIST AT THE BOTTOM
            IgnoredFriendRequests
        }, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
    }

    public static void LoadData()
    {
        if (File.Exists(filePath))
        {
            var jsonData = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeAnonymousType(jsonData, new
            {
                AuthCookie = "",
                twoFactorAuthCookie,
                IgnoredFriendRequests = new List<string>(),
                HypeRateID = "",
                HypeRateSecretToken = "",
                Bio = "",
                VRChatOSCPort = 9000
            });

            AuthCookie = data.AuthCookie;
            IgnoredFriendRequests = data.IgnoredFriendRequests;
            HypeRateID = data.HypeRateID;
            HypeRateSecretToken = data.HypeRateSecretToken;
            twoFactorAuthCookie = data.twoFactorAuthCookie;
            Bio = data.Bio;
            VRChatOSCPort = data.VRChatOSCPort;

            if (VRChatOSCPort == 0) VRChatOSCPort = 9000;
            SaveData();
        }
        else
        {
            AuthCookie = "";
            IgnoredFriendRequests = new List<string>();
            HypeRateID = "";
            HypeRateSecretToken = "";
            twoFactorAuthCookie = "";
            Bio = "";
            VRChatOSCPort = 9000;
            SaveData();
        }
    }

    public static void AddUserToIgnored(string userId)
    {
        IgnoredFriendRequests.Add(userId);
        SaveData();
    }
}
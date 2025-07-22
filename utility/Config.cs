// /*
//  *
//  * Zuxi.OSC - Config.GetInstance().cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace Zuxi.OSC.utility;

public class Config
{
    private static Config? _instance;

    public string AuthCookie { get; set; } = ""; 
    public string twoFactorAuthCookie { get; set; } = "";
    public List<string> IgnoredFriendRequests { get; set; } = new List<string>();
    public string HypeRateID { get; set; } = "";
    public string HypeRateSecretToken { get; set; } = "";
    public string Bio { get; set; } = ""; 
    public string VRCAuthValue { get; set; } = "";
    public int VRChatOSCPort { get; set; } = 9000;

    internal void Load()
    {
        if (File.Exists("data.json"))
        {
            _instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data.json"));
            Console.WriteLine("Loaded Config");
            _instance!.Save();
        }
        else
        {
            Save();
            Console.WriteLine("Created New Config");
        }
    }
    public void Save()
    {
        File.WriteAllText("data.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        Console.WriteLine("Saved Config");
    }

    public static Config GetInstance()
    {
        if (_instance is null)
            new Config().Load();
        return _instance!;
    }

    public void AddUserToIgnored(string userId)
    {
        IgnoredFriendRequests.Add(userId);
        Save();
    }
}

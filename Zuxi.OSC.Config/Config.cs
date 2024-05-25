using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


namespace Zuxi.OSC.Config
{
    public class JsonConfig
    {
        public static string AuthCookie { get; set; }
        public static string twoFactorAuthCookie { get; set; }
        public static List<string> IgnoredFriendRequests { get; set; }

        private static readonly string filePath = "data.json"; // Change the file path as needed
        public static string HypeRateID  { get; set; }
        public static string HypeRateSecretToken { get; set; }

        public static string Bio { get; set; }




        public static void SaveData()
        {
            string jsonData = JsonConvert.SerializeObject(new { AuthCookie, twoFactorAuthCookie,  HypeRateID, HypeRateSecretToken, Bio,   IgnoredFriendRequests,  }, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }

        public static void LoadData()
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeAnonymousType(jsonData, new { AuthCookie = "", twoFactorAuthCookie,  IgnoredFriendRequests = new List<string>(), HypeRateID = "", HypeRateSecretToken = "", Bio = "" });

                AuthCookie = data.AuthCookie;
                IgnoredFriendRequests = data.IgnoredFriendRequests;
                HypeRateID = data.HypeRateID;
                HypeRateSecretToken = data.HypeRateSecretToken;
                twoFactorAuthCookie = data.twoFactorAuthCookie;
                Bio = data.Bio;
            }
            else
            {
                AuthCookie = "";
                IgnoredFriendRequests = new List<string>();
                HypeRateID = "";
                HypeRateSecretToken = "";
                twoFactorAuthCookie = "";
                Bio = "";
                SaveData();
            }
        }

        public static void AddUserToIgnored(string UID)
        {
            IgnoredFriendRequests.Add(UID);
            SaveData();
        }

















    }
}

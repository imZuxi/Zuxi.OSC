// /*
//  *
//  * Zuxi.OSC - VRCUser.cs
//  * Copyright 2023 - 2026 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

//Resharper Disable All :: this is a user class so it should be treated as such.
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zuxi.OSC.Modules.FriendRequest.Json;
/// <summary>
/// VRCUser - Local User or known as you some feilds are only populated when sending api requests that changes
/// something IE update bio... this also handles these cases so not everything is nuked when updating users
/// </summary>
public class VRCUser
{
    // Hopefully this is a complete VRCLocal User i will update as vrchat updates...
    // updated 3/24/26 9:30pm
    public static VRCUser? CurrentUser;
    public VRCUser(string user)
    {
        HashSet<string> existingFriends = CurrentUser?.Friends != null ? new HashSet<string>(CurrentUser.Friends) : new HashSet<string>();

        if (CurrentUser != null)
        {
            JsonConvert.PopulateObject(JsonConvert.SerializeObject(CurrentUser), this);
        }
        JsonConvert.PopulateObject(user, this);
        if (Friends != null)
        {
            existingFriends.UnionWith(Friends);
            Friends = existingFriends.ToList();
        }
        CurrentUser = this;
        if (UnknownFields?.Count > 0)
        {
            foreach (var kv in UnknownFields)
            {
                PrintUnknownFields(kv.Key, kv.Value);
            }
        }
    }


    [JsonExtensionData]
    public Dictionary<string, JToken> UnknownFields { get; set; }

    void PrintUnknownFields(string parentPath, JToken token)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                foreach (var prop in token.Children<JProperty>())
                    PrintUnknownFields(string.IsNullOrEmpty(parentPath) ? prop.Name : parentPath + "." + prop.Name, prop.Value);
                break;
            case JTokenType.Array:
                int index = 0;
                foreach (var item in token.Children())
                {
                    PrintUnknownFields($"{parentPath}[{index}]", item);
                    index++;
                }
                break;
            default:
                Console.WriteLine($"[{nameof(VRCUser)}] {Id} => Unknown field: {parentPath} ({token.Type}) = {token}");
                break;
        }
    }

    public int AcceptedTOSVersion { get; set; }
    public int AcceptedPrivacyVersion { get; set; }
    public DateTime? AccountDeletionDate { get; set; }
    public List<string> ActiveFriends { get; set; }
    public bool AllowAvatarCopying { get; set; }
    public string Bio { get; set; }
    public List<string> BioLinks { get; set; }
    public string CurrentAvatar { get; set; }
    public string CurrentAvatarAssetUrl { get; set; }
    public string CurrentAvatarImageUrl { get; set; }
    public string CurrentAvatarThumbnailImageUrl { get; set; }
    [JsonProperty("date_joined")]
    public DateTime DateJoined { get; set; }
    public string DeveloperType { get; set; }
    public string DisplayName { get; set; }
    public bool EmailVerified { get; set; }
    public string FallbackAvatar { get; set; }
    public string FriendKey { get; set; }
    public List<string> Friends { get; set; }
    public bool HasBirthday { get; set; }
    public bool HasEmail { get; set; }
    public bool HasLoggedInFromClient { get; set; }
    public bool HasPendingEmail { get; set; }
    public string HomeLocation { get; set; }
    public string Id { get; set; }
    public bool IsFriend { get; set; }
    [JsonProperty("last_activity")]
    public DateTime LastActivity { get; set; }
    [JsonProperty("last_login")]
    public DateTime LastLogin { get; set; }
    [JsonProperty("last_platform")]
    public string LastPlatform { get; set; }
    public string ObfuscatedEmail { get; set; }
    public string ObfuscatedPendingEmail { get; set; }
    public string OculusId { get; set; }
    public List<string> OfflineFriends { get; set; }
    public List<string> OnlineFriends { get; set; }
    public List<DisplayNameEntry> PastDisplayNames { get; set; }
    public PresenceInfo Presence { get; set; }
    public string ProfilePicOverride { get; set; }
    public string State { get; set; }
    public string Status { get; set; }
    public string StatusDescription { get; set; }
    public bool StatusFirstTime { get; set; }
    public List<string> StatusHistory { get; set; }
    public Dictionary<string, string> SteamDetails { get; set; }
    public string SteamId { get; set; }
    public List<string> Tags { get; set; }
    public bool TwoFactorAuthEnabled { get; set; }
    public DateTime TwoFactorAuthEnabledDate { get; set; }
    public bool Unsubscribe { get; set; }
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
    public string UserIcon { get; set; }
    public List<Badge> Badges { get; set; }
    public string Pronouns { get; set; }
    public List<string> CurrentAvatarTags { get; set; }
    [JsonProperty("last_mobile")]
    public object LastMobile { get; set; }
    public string ProfilePicOverrideThumbnail { get; set; }
    [JsonProperty("platform_history")]
    public List<PlatformHistory> Platform_History { get; set; }
    public List<string> pronounsHistory { get; set; }
    public string queuedInstance { get; set; }
    public bool receiveMobileInvitations { get; set; }
    public string userLanguage { get; set; }
    public string userLanuageCode { get; set; }
    public string userLanguageCode { get; set; }
    public string username { get; set; }
    public bool usesGeneratedPassword { get; set; }
    public string viveId { get; set; }
    public DateTime? accountDeletionLog { get; set; }
    public string ageVerificationStatus { get; set; }
    public bool ageVerified { get; set; }
    public string discordId { get; set; }
    public string googleId { get; set; }
    public bool hasSharedConnectionsOptOut { get; set; }
    public bool hideContentFilterSettings { get; set; }
    public bool isAdult { get; set; }
    public bool isBoopingEnabled { get; set; }
    public string picoId { get; set; }

    public DiscordDetails discordDetails { get; set; }
    public bool hasAcceptedDiscordSocialSDKPerms { get; set; }

    public bool hasDiscordFriendsOptOut { get; set; }
    public string appleId { get; set; } 

    public class DisplayNameEntry
    {
        public string DisplayName { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PresenceInfo
    {
        public string AvatarThumbnail { get; set; }
        public string DisplayName { get; set; }
        public List<string> Groups { get; set; }
        public string Id { get; set; }
        public string Instance { get; set; }
        public string InstanceType { get; set; }
        public string IsRejoining { get; set; }
        public string Platform { get; set; }
        public string ProfilePicOverride { get; set; }
        public string Status { get; set; }
        public string TravelingToInstance { get; set; }
        public string TravelingToWorld { get; set; }
        public string World { get; set; }
    }

    public class Badge
    {
        public string BadgeId { get; set; }
        public string BadgeName { get; set; }
        public string BadgeDescription { get; set; }
        public string BadgeImageUrl { get; set; }
        public bool Hidden { get; set; }
        public bool Showcased { get; set; }
        public DateTime AssignedAt { get; set; }
    }

    public class PastDisplayName
    {
        public string DisplayName { get; set; }
        public bool Reverted { get; set; }
        public string UpdatedAt { get; set; }
    }

    public class PlatformHistory
    {
    public bool IsMobile { get; set; }
    public string platform { get; set; }
    public DateTime recorded { get; set; }

    }

    public class DiscordDetails
    {
        public string global_name { get; set; }
        public string id { get; set; }
    }
}


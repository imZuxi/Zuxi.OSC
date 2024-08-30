using Newtonsoft.Json;

namespace Zuxi.OSC.Modules.FriendRequest.Json
{
    public class VRCUser
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static VRCUser CurrentUser;
        public static VRCUser CreateVRCUser(string value)
        {
            VRCUser vrChatUser = JsonConvert.DeserializeObject<VRCUser>(value);

            CurrentUser = vrChatUser;

            return vrChatUser;
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
        public DateTime LastActivity { get; set; }
        public DateTime LastLogin { get; set; }
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
        public DateTime UpdatedAt { get; set; }
        public string UserIcon { get; set; }

    }



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



}

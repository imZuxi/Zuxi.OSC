// /*
//  *
//  * Zuxi.OSC - VRCPlayer.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Zuxi.OSC.Modules.FriendRequest.Json.VRCUser;

namespace Zuxi.OSC.Modules.FriendRequest.Json;

/// <summary>
/// VRCPlayer for non-local user can be friend or non friend however some will not be populated for non friend
/// </summary>
public class VRCPlayer
{
    // updated 12/09/24 7am
    // @note i will update periodically

   public VRCPlayer(string user)
   {
       JsonConvert.PopulateObject(user, this);
   }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public bool AllowAvatarCopying { get; set; }
    public string Bio { get; set; }
    public List<string> BioLinks { get; set; }
    public string CurrentAvatarImageUrl { get; set; }
    public string CurrentAvatarThumbnailImageUrl { get; set; }

    [JsonProperty("date_joined")]
    [JsonConverter(typeof(CustomDateFormatConverter), "yyyy-MM-dd")]
    public DateTime DateJoined { get; set; }

    public string DeveloperType { get; set; }
    public string DisplayName { get; set; }
    public string FriendKey { get; set; }
    public string FriendRequestStatus { get; set; }
    public string Id { get; set; }
    public string InstanceId { get; set; }
    public bool IsFriend { get; set; }
    public string LastActivity { get; set; }
    public string LastLogin { get; set; }
    public string LastPlatform { get; set; }
    public string Location { get; set; }
    public string Note { get; set; }
    public string ProfilePicOverride { get; set; }
    public string State { get; set; }
    public string Status { get; set; }
    public string StatusDescription { get; set; }
    public List<string> Tags { get; set; }
    public string TravelingToInstance { get; set; }
    public string TravelingToLocation { get; set; }
    public string TravelingToWorld { get; set; }
    public string UserIcon { get; set; }
    public string WorldId { get; set; }
    public string AgeVerificationStatus { get; set; }
    public List<string> CurrentAvatarTags { get; set; }
    public string Platform { get; set; }
    public string ProfilePicOverrideThumbnail { get; set; }
    public string Pronouns { get; set; }


    public List<Badge> Badges { get; set; }
    public object LastMobile { get; set; }

    private class CustomDateFormatConverter : IsoDateTimeConverter
    {
        public CustomDateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}

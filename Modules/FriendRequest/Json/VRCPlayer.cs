﻿// /*
//  *
//  * Zuxi.OSC - VRCPlayer.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Zuxi.OSC.Modules.FriendRequest.Json;

public class VRCPlayer
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static VRCPlayer CreateVRCPlayer(string value)
    {
        var _VRCPlayer = JsonConvert.DeserializeObject<VRCPlayer>(value);

        return _VRCPlayer;
    }

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

    private class CustomDateFormatConverter : IsoDateTimeConverter
    {
        public CustomDateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
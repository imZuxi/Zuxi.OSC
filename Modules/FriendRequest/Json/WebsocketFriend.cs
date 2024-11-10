// /*
//  *
//  * Zuxi.OSC - WebsocketFriend.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace Zuxi.OSC.Modules.FriendRequest.Json;

public class WebsocketFriend
{
    [JsonProperty("userId")] public string id;
    [JsonProperty("user")] public RequestUser user;

    public class RequestUser
    {
        public string displayName;
    }

    public static WebsocketFriend Create(string json)
    {
        return JsonConvert.DeserializeObject<WebsocketFriend>(json);
    }
}
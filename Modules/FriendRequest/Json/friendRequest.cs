// /*
//  *
//  * Zuxi.OSC - friendRequest.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace Zuxi.OSC.Modules.FriendRequest.Json;

internal class friendRequest
{
    public string Id { get; set; }
    public string SenderUserId { get; set; }
    public string SenderUsername { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public object Details { get; set; }
    public bool Seen { get; set; }
    public DateTime CreatedAt { get; set; }

    public static List<friendRequest> DecodeJson(string json)
    {
        // Deserialize the JSON string into a List<FriendRequest> object
        return JsonConvert.DeserializeObject<List<friendRequest>>(json);
    }
}
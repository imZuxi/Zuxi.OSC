// /*
//  *
//  * Zuxi.OSC - JoinChannelModel.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace VRCHypeRate.HeartRateProvider.HypeRate.Models;

public class JoinChannelModel
{
    [JsonProperty("event")] private string Event = "phx_join";

    [JsonProperty("payload")] private JoinChannelPayload Payload = new();

    [JsonProperty("ref")] private int Ref;

    [JsonProperty("topic")] private string topic = null!;

    [JsonIgnore]
    public string Id
    {
        set => topic = "hr:" + value;
    }
}

public class JoinChannelPayload
{
}
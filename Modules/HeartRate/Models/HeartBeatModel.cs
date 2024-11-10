// /*
//  *
//  * Zuxi.OSC - HeartBeatModel.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace VRCHypeRate.HeartRateProvider.HypeRate.Models;

public class HeartBeatModel
{
    [JsonProperty("event")] private string Event = "heartbeat";

    [JsonProperty("payload")] private WebSocketHeartBeatPayload Payload = new();

    [JsonProperty("ref")] private int Ref;

    [JsonProperty("topic")] private string Topic = "phoenix";
}

public class WebSocketHeartBeatPayload
{
}
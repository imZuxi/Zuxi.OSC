// /*
//  *
//  * Zuxi.OSC - PhxReplyModel.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace VRCHypeRate.HeartRateProvider.HypeRate.Models;
// credits https://github.com/VolcanicArts/VRCHypeRate
public class PhxReplyModel
{
    [JsonProperty("payload")] public PhxReplyPayload Payload = null!;
}

public class PhxReplyPayload
{
    [JsonProperty("status")] public string Status = null!;
}

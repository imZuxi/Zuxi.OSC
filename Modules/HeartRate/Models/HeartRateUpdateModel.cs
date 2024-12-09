// /*
//  *
//  * Zuxi.OSC - HeartRateUpdateModel.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace VRCHypeRate.HeartRateProvider.HypeRate.Models;

// credits https://github.com/VolcanicArts/VRCHypeRate
public class HeartRateUpdateModel
{
    [JsonProperty("payload")] public HeartRateUpdatePayload Payload = null!;
}

public class HeartRateUpdatePayload
{
    [JsonProperty("hr")] public int HeartRate;
}

// /*
//  *
//  * Zuxi.OSC - EventModel.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace VRCHypeRate.HeartRateProvider.HypeRate.Models;

// credits https://github.com/VolcanicArts/VRCHypeRate
public class EventModel
{
    [JsonProperty("event")] public string Event = null!;
}

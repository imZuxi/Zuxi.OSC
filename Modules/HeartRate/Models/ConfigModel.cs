// /*
//  *
//  * Zuxi.OSC - ConfigModel.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Newtonsoft.Json;

namespace VRCHypeRate.Models;

// credits https://github.com/VolcanicArts/VRCHypeRate
public class ConfigModel
{
    [JsonProperty("apikey")] public string ApiKey;

    [JsonProperty("id")] public string Id;
}

﻿// /*
//  *
//  * Zuxi.OSC - ZuxiBioUpdate.cs
//  * Copyright 2023 - 2024 Zuxi and contributors
//  * https://zuxi.dev
//  *
//  */

using Zuxi.OSC.Modules.FriendRequest.Json;
using Zuxi.OSC.Modules.FriendRequests;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Module.FriendRequests;

internal class ZuxiBioUpdate
{
    //  public static string BioTemplate = File.ReadAllText("ZuxiBio.txt");

    internal static void SendUpdate()
    {
        var Update = new UserUpdate()
        {
            statusDescription = VRCUser.CurrentUser.StatusDescription,
            bioLinks = VRCUser.CurrentUser.BioLinks
        };

        Update.bio = Config.Bio.Replace("{CURRENTFRIENDCOUNT}", VRCUser.CurrentUser.Friends.Count.ToString());
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(Update);
        var re = HClient.GetInstance().MakeAPIPutRequest("users/" + VRCUser.CurrentUser.Id, json);
        if (re != "[]")
        {
            VRCUser.CreateVRCUser(re);
            Console.WriteLine("Bio Updated! ");
        }
    }
}

internal class UserUpdate
{
    public string statusDescription { get; set; }
    public string bio { get; set; }

    public List<string> bioLinks { get; set; }
}
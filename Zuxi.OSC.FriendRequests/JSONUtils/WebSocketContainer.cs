using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuxi.OSC.Module.FriendRequests.JSONUtils
{
    internal class WebSocketNotifcationContainer
    {
      






    }


    public class Content
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string ContentType { get; set; }

        [JsonProperty("senderUserId")]
        public string SenderUserId { get; set; }

        [JsonProperty("senderUsername")]
        public string SenderUsername { get; set; }

        [JsonProperty("receiverUserId")]
        public string ReceiverUserId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("details")]
        public object Details { get; set; } // You can change the type based on your actual data structure

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        public static Content FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Content>(json);
        }


    }




   



    public class TNotification
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }


        public static TNotification FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TNotification>(json);
        }
    }
}

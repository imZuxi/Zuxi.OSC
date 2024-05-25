using Newtonsoft.Json;
using System;
using System.Threading;
using VRCHypeRate.HeartRateProvider.HypeRate.Models;

namespace Zuxi.OSC.HeartRate
{
    public class HeartBeat
    {
        static DateTime lasthrt = DateTime.Now;
        static int lasthr = 0;

        public HeartBeat(string ConnctionToken, string APIKey, Action<int> OnHeartRateChanged)
        {
            OnHeartRateUpdate = OnHeartRateChanged;
            Id = ConnctionToken;
            new Websocket(HypeRateUri + APIKey);

        }



        public static Action<int> OnHeartRateUpdate;
        private const string HypeRateUri = "wss://app.hyperate.io/socket/websocket?token=";
        private const int HeartbeatInternal = 30000;
        private static string Id;
        private static Timer? heartBeatTimer;


        public static void OnWsConnected()
        {
            Console.WriteLine("Successfully connected to the HypeRate websocket");
            sendJoinChannel();
            initHeartBeat();
        }

        protected static void OnWsDisconnected()
        {
            Console.WriteLine("Disconnected from the HypeRate websocket");
            heartBeatTimer?.Dispose();
        }

        internal static void OnWsMessageReceived(string message)
        {
            var eventModel = JsonConvert.DeserializeObject<EventModel>(message);
            if (eventModel == null)
            {
                Console.WriteLine($"Received an unrecognised message:\n{message}");
                return;
            }

            switch (eventModel.Event)
            {
                case "hr_update":
                    handleHrUpdate(JsonConvert.DeserializeObject<HeartRateUpdateModel>(message)!);
                    break;
                case "phx_reply":
                    handlePhxReply(JsonConvert.DeserializeObject<PhxReplyModel>(message)!);
                    break;
            }
        }

        private static void initHeartBeat()
        {
            heartBeatTimer = new Timer(sendHeartBeat, null, HeartbeatInternal, Timeout.Infinite);
        }

        private static void sendHeartBeat(object? _)
        {
            Console.WriteLine("Sending HypeRate websocket heartbeat");
            Websocket.Send(new HeartBeatModel());
            heartBeatTimer?.Change(HeartbeatInternal, Timeout.Infinite);
        }

        private static void sendJoinChannel()
        {
            Console.WriteLine($"Requesting to hook into heartrate for Id {Id}");
            var joinChannelModel = new JoinChannelModel
            {
                Id = Id
            };
            Websocket.Send(joinChannelModel);
        }

        private static void handlePhxReply(PhxReplyModel reply)
        {
            Console.WriteLine($"Status of reply: {reply.Payload.Status}");


            TimeSpan timeDifference = DateTime.Now - lasthrt;
            Console.WriteLine($"last hr update: {timeDifference.TotalSeconds} s");
            // Check if the time difference is greater than 1 minute

            if (lasthr == 0)
            {
                lasthrt = DateTime.Now;
                return;
            }

            if (timeDifference.TotalSeconds > 20)
            {
                lasthrt= DateTime.Now;
                Console.WriteLine("Reseting HR to 0 since its been a while since hr updated");
                OnHeartRateUpdate?.Invoke(0);
            }

        }

        private static void handleHrUpdate(HeartRateUpdateModel update)
        {
            lasthrt = DateTime.Now;

            var heartRate = update.Payload.HeartRate;
            Console.WriteLine($"Received heartrate {heartRate}");
            lasthr = heartRate;

            OnHeartRateUpdate?.Invoke(heartRate); 
        }
    }
}

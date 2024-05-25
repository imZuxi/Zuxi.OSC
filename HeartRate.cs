using Newtonsoft.Json;
using System;
using Zuxi.OSC.Config;
using Zuxi.OSC.HeartRate;

namespace Zuxi.OSC.Modules
{
    internal class HeartRateMod
    {

        public static int HeartRateInt = 0;

        public static void Initialize()
        {
            new HeartBeat(JsonConfig.HypeRateID, JsonConfig.HypeRateSecretToken, SetHeartRate);
        }

        public static void SetHeartRate(int value)
        {

            Console.WriteLine(value.ToString());

            HeartRateInt = value;



        }


    }
}

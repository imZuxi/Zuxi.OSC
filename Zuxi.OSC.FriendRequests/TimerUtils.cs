using System;
using System.Timers;

namespace Zuxi.OSC.Modules.FriendRequests
{

    internal class TimerUtils
    {

        static Action OnTimerFinished = delegate { };
        static Timer timer = null;
        internal static void StartTimingMe(Action OnThisTimerFinished)
        {
            timer = new Timer();

            OnTimerFinished = OnThisTimerFinished;



            // Set the interval (in milliseconds) after which the timer will elapse
            timer.Interval = 180000; // 1 second

            // Hook up the Elapsed event handler
            timer.Elapsed += TimerElapsed;

            // Start the timer
            timer.Start();

            // Keep the console application running


        }

        internal static void StopTimer()
        {

            if (timer is null)
                return;


            timer.Stop();
            timer.Dispose();
        }

        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnTimerFinished.Invoke();
        }


    }
}



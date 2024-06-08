using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuxi.OSC.Utils
{
    internal class GeneralUtils
    {
       public static bool IsVR()
        {
            return true;
         //   return Process.GetProcessesByName("VRServer").Length >= 0;
        }
    }
}

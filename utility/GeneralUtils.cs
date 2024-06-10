using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuxi.OSC.utility
{
    internal class GeneralUtils
    {
        //this is a simple check if steamvr is running to disable outher running stuff in cheatbox manager 
        internal static bool IsInVR()
        {
            Process[] process = Process.GetProcessesByName("vrserver");
           return process.Length > 0;
        }
    }
}

﻿using System;
using System.Diagnostics;

using System.Management;
using Microsoft.VisualBasic.Devices;
namespace Zuxi.OSC
{
    internal class SysInfo
    {
       internal static float GetCpuUsage()
        {
            using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                cpuCounter.NextValue(); // Initial value is always 0
                System.Threading.Thread.Sleep(1000); // Wait for a second
                return cpuCounter.NextValue();
            }
        }

        internal static void GetMemoryUsage(out ulong totalMemory, out ulong usedMemory, out float memoryUsage)
        {
            totalMemory = (ulong)new ComputerInfo().TotalPhysicalMemory;
            ulong freeMemory = (ulong)new ComputerInfo().AvailablePhysicalMemory;
            usedMemory = totalMemory - freeMemory;
            memoryUsage = (float)usedMemory / totalMemory * 100;
        }
    }
}
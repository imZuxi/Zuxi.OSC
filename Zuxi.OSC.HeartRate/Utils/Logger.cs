using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VRCHypeRate.Utils;

public static class Logger
{
    public static void Error(string message)
    {
        Console.WriteLine(message); 
    }

    public static void Log(string message, LogLevel logLevel = LogLevel.Debug)
    {


        Console.WriteLine(message);


    }

 
}

public enum LogLevel
{
    Debug = -1,
    Verbose = 0,
    Error = 1
}
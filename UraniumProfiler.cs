using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;


namespace СSharp_Profiler;

public static class UraniumProfiler
{
    public static List<EventData>? Events = new List<EventData>();
    public static string _functionName;
    public static long _startTime;
    //public static long _endTime;
    public static Stopwatch? StopWatch;


    public static void Initialize()
    {
        StopWatch = new Stopwatch();
        StopWatch.Start();
    }


    public static void SaveSession()
    {
        Console.WriteLine("Session saved");
        if (Events == null) return;
        foreach (var e in Events)
        {
            Console.WriteLine(e.FunctionName + " " + e.StartTime + " " + e.EndTime);
        }
    }
}
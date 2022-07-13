using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;


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
        if (Events == null) return;
        var str = ConstructData();
        var data = ConvertStringToBytes(str);
        CreateFile(data);
        //CreateHumanReadableFile(str);
        Console.WriteLine("Session saved");
    }

    private static void CreateFile(byte[] data)
    {
        const string path = "Session.ups";
        var sw = new StreamWriter(File.Create(path), Encoding.UTF8);
        foreach (var i in data)
        {
            sw.Write(i);
        }

        sw.Close();
    }

    /*private static void CreateHumanReadableFile(string str)
    {
        const string path = "HSession.ups";
        var sw = new StreamWriter(File.Create(path), Encoding.UTF8);
        sw.Write(str);
        sw.Close();
    }*/

    private static string ConstructData()
    {
        if (Events == null) return string.Empty;
        StringBuilder sb = new StringBuilder();
        var uniqueFuncs = Events.DistinctBy(data => data.FunctionName).ToList();

        // Header
        UInt32 functionsCount = Convert.ToUInt32(uniqueFuncs.Count);
        double nanosecondsInTick = 1000000000.0 / Stopwatch.Frequency;
        sb.Append(nanosecondsInTick + "\n" + functionsCount + "\n");
        foreach (var e in uniqueFuncs)
        {
            UInt16 funcNameLength = Convert.ToUInt16(e.FunctionName.Length);
            sb.Append(funcNameLength + e.FunctionName);
        }

        sb.Append('\n');

        // Events
        UInt32 eventsCount = (UInt32)Events.Count;
        const string startPrefix = "0000";
        const string endPrefix = "0001";
        sb.Append(eventsCount + "\n");

        foreach (var e in Events)
        {
            UInt32 functionIndex = (UInt32)Events.IndexOf(e);
            UInt64 cpuTicks = (UInt64)e.StartTime;
            UInt64 endCpuTicks = (UInt64)e.EndTime;

            sb.Append(startPrefix + functionIndex + cpuTicks);
            sb.Append(endPrefix + functionIndex + endCpuTicks);
        }

        return sb.ToString();
    }

    private static byte[] ConvertStringToBytes(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }
}
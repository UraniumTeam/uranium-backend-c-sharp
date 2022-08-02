using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace UraniumBackend;

public static class UraniumProfiler
{
    public static List<EventData>? Events = new List<EventData>();
    public static string _functionName;

    public static long _startTime;

    public static Stopwatch? StopWatch;

    public static void Initialize()
    {
        StopWatch = new Stopwatch();
        StopWatch.Start();
    }

    public static void SaveSession()
    {
        if (Events is null)
        {
            return;
        }

        var data = ConstructData();
        WriteBytesToFile(data);
        Console.WriteLine("Session saved");
    }

    private static List<byte> ConstructData()
    {
        if (Events == null)
        {
            return new List<byte>();
        }

        var data = new List<byte>();
        var uniqueFuncs = Events.DistinctBy(e => e.FunctionName).ToList();

        // Header
        var functionsCount = (uint)uniqueFuncs.Count;
        var nanosecondsInTick = 1000000000.0 / Stopwatch.Frequency;
        data.AddRange(BitConverter.GetBytes(nanosecondsInTick));
        data.AddRange(BitConverter.GetBytes(functionsCount));
        foreach (var e in uniqueFuncs)
        {
            var nameBytes = Encoding.ASCII.GetBytes(e.FunctionName);
            data.AddRange(BitConverter.GetBytes((ushort)nameBytes.Length));
            data.AddRange(nameBytes);
        }

        // Events
        var eventsCount = (uint)Events.Count;
        data.AddRange(BitConverter.GetBytes(eventsCount));
        foreach (var e in Events)
        {
            var functionIndex = (uint)Events.IndexOf(e);
            var cpuTicks = (ulong)e.StartTime;
            var endCpuTicks = (ulong)e.EndTime;

            data.AddRange(BitConverter.GetBytes(functionIndex));
            data.AddRange(BitConverter.GetBytes(cpuTicks));
            data.AddRange(BitConverter.GetBytes(functionIndex));
            data.AddRange(BitConverter.GetBytes(endCpuTicks));
        }

        return data;
    }

    private static void WriteBytesToFile(List<byte> data)
    {
        const string path = "Session/thread--0.ups";
        if (!Directory.Exists("Session"))
        {
            Directory.CreateDirectory("Session");
        }

        File.WriteAllBytes(path, data.ToArray());
        File.WriteAllText("Session.ups", Path.GetFullPath(path));
    }
}

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
        var data = ConstructData();
        WriteBytesToFile(data);
        //WriteStrToFile(ConstructHumanReadableData());
        Console.WriteLine("Session saved");
    }
    
    private static List<byte> ConstructData()
    {
        if (Events == null) return null;
        var data = new List<byte>();
        var uniqueFuncs = Events.DistinctBy(e => e.FunctionName).ToList();

        // Header
        var functionsCount = (uint)uniqueFuncs.Count;
        var nanosecondsInTick = 1000000000.0 / Stopwatch.Frequency;
        data.AddRange(BitConverter.GetBytes(nanosecondsInTick));
        data.AddRange(Encoding.UTF8.GetBytes("\n"));
        data.AddRange(BitConverter.GetBytes(functionsCount));
        data.AddRange(Encoding.UTF8.GetBytes("\n"));
        foreach (var e in uniqueFuncs)
        {
            var funcNameLength = (ushort)e.FunctionName.Length;
            data.AddRange(BitConverter.GetBytes(funcNameLength));
            data.AddRange(Encoding.UTF8.GetBytes(e.FunctionName));
        }
        data.AddRange(Encoding.UTF8.GetBytes("\n"));

        // Events
        var eventsCount = (uint)Events.Count;
        const string startPrefix = "0000";
        const string endPrefix = "0001";
        data.AddRange(BitConverter.GetBytes(eventsCount));
        data.AddRange(Encoding.UTF8.GetBytes("\n"));
        foreach (var e in Events)
        {
            var functionIndex = (uint)Events.IndexOf(e);
            var cpuTicks = (ulong)e.StartTime;
            var endCpuTicks = (ulong)e.EndTime;

            data.AddRange(Encoding.UTF8.GetBytes(startPrefix));
            data.AddRange(BitConverter.GetBytes(functionIndex));
            data.AddRange(BitConverter.GetBytes(cpuTicks));
            data.AddRange(Encoding.UTF8.GetBytes(endPrefix));
            data.AddRange(BitConverter.GetBytes(functionIndex));
            data.AddRange(BitConverter.GetBytes(endCpuTicks));
        }

        return data;
    }

    /*private static string ConstructHumanReadableData()
    {
        if (Events == null) return string.Empty;
        var sb = new StringBuilder();
        var uniqueFuncs = Events.DistinctBy(e => e.FunctionName).ToList();
        var functionsCount = (uint)uniqueFuncs.Count;
        var nanosecondsInTick = 1000000000.0 / Stopwatch.Frequency;
        sb.Append(nanosecondsInTick + "\n" + functionsCount + "\n");
        foreach (var e in uniqueFuncs)
        {
            var funcNameLength = (ushort)e.FunctionName.Length;
            sb.Append(funcNameLength + e.FunctionName);
        }

        sb.Append('\n');
        var eventsCount = (uint)Events.Count;
        const string startPrefix = "0000";
        const string endPrefix = "0001";
        sb.Append(eventsCount + "\n");
        foreach (var e in Events)
        {
            var functionIndex = (uint)Events.IndexOf(e);
            var cpuTicks = (ulong)e.StartTime;
            var endCpuTicks = (ulong)e.EndTime;
            sb.Append(startPrefix + functionIndex + cpuTicks);
            sb.Append(endPrefix + functionIndex + endCpuTicks);
        }

        return sb.ToString();
    }*/

    private static void WriteBytesToFile(List<byte> data)
    {
        const string path = "Session.ups";
        using var stream = File.Open(path, FileMode.OpenOrCreate);
        using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
        foreach (var item in data)
            writer.Write(item);
    }

    /*private static void WriteStrToFile(string str)
    {
        const string path = "HSession.ups";
        var sw = new StreamWriter(File.Create(path));
        sw.Write(str);
        sw.Close();
    }*/
}
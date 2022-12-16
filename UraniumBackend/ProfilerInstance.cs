using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace UraniumBackend;

public sealed class ProfilerInstance
{
    internal static ulong Ticks => (ulong)instance.stopwatch.ElapsedTicks;

    private static readonly ProfilerInstance instance = new();
    private readonly List<EventData> events = new();
    private readonly Stopwatch stopwatch = Stopwatch.StartNew();

    public static void Save()
    {
        instance.SaveSession();
    }

    internal static void AddEvent(in EventData eventData)
    {
        instance.events.Add(eventData);
    }

    private void SaveSession()
    {
        var sessionName = $"session-{DateTime.Now:MM-dd-yyyy--HH-mm-ss}";
        var directory = $"{sessionName}-threads";
        var path = Path.Combine(directory, "thread--0.upt");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var stream = new FileStream(path, FileMode.Create);
        using var writer = new BinaryWriter(stream);
        WriteData(writer);
        File.WriteAllText($"{sessionName}.ups", Path.GetFullPath(path));
    }

    private void WriteData(BinaryWriter writer)
    {
        var functionNames = events
            .DistinctBy(e => e.FunctionName)
            .Select(e => e.FunctionName)
            .OrderBy(e => e)
            .ToList();

        var nanosecondsInTick = 1000000000.0 / Stopwatch.Frequency;
        writer.Write(nanosecondsInTick);
        writer.Write((uint)functionNames.Count);
        foreach (var name in functionNames)
        {
            var nameBytes = Encoding.ASCII.GetBytes(name);
            writer.Write((ushort)nameBytes.Length);
            writer.Write(nameBytes);
        }

        var eventsCount = (uint)events.Count;
        writer.Write(eventsCount);
        foreach (var e in events)
        {
            var functionIndex = (uint)functionNames.BinarySearch(e.FunctionName);
            e.WriteTo(writer, functionIndex);
        }
    }
}

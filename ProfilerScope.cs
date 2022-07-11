using System.Diagnostics;

namespace СSharp_Profiler;

public class ProfilerScope : IDisposable
{
    public ProfilerScope(string functionName)
    {
        UraniumProfiler._functionName = functionName;
        UraniumProfiler._startTime = Stopwatch.GetTimestamp();
    }

    public void Dispose()
    {
        var endTime = Stopwatch.GetTimestamp() - UraniumProfiler._startTime;
        UraniumProfiler.Events?.Add(new EventData(UraniumProfiler._functionName, UraniumProfiler._startTime, endTime));
    }
}
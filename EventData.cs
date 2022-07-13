namespace СSharp_Profiler;

public readonly struct EventData
{
    public readonly string FunctionName;
    public readonly long StartTime;
    public readonly long EndTime;
    public readonly long Elapsed;

    public EventData(string functionName, long startTime, long endTime, long elapsed)
    {
        FunctionName = functionName;
        StartTime = startTime;
        EndTime = endTime;
        Elapsed = elapsed;
    }
}
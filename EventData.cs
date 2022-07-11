namespace СSharp_Profiler;

public readonly struct EventData
{
    public readonly string FunctionName;
    public readonly long StartTime;
    public readonly long EndTime;

    public EventData(string functionName, long startTime, long endTime)
    {
        FunctionName = functionName;
        StartTime = startTime;
        EndTime = endTime;
    }
}
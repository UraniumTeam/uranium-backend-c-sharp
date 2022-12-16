using System.Runtime.CompilerServices;

namespace UraniumBackend;

public readonly struct ProfilerScope : IDisposable
{
    private readonly string functionName;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ProfilerScope(string functionName)
    {
        this.functionName = functionName;
        ProfilerInstance.AddEvent(new EventData(functionName, ProfilerInstance.Ticks, EventKind.Begin));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        ProfilerInstance.AddEvent(new EventData(functionName, ProfilerInstance.Ticks, EventKind.End));
    }
}

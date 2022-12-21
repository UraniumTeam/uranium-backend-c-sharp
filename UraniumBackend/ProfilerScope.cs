using System.Runtime.CompilerServices;

namespace UraniumBackend;

public readonly struct ProfilerScope : IDisposable
{
    private readonly string functionName;

    public static ProfilerScope Begin(string functionName)
    {
        return new ProfilerScope(functionName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ProfilerScope(string functionName)
    {
        this.functionName = functionName;
        ProfilerInstance.AddEvent(new EventData(functionName, ProfilerInstance.Ticks, EventKind.Begin));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (functionName is null)
        {
            throw new ArgumentNullException(nameof(functionName), "Function name must be set");
        }

        ProfilerInstance.AddEvent(new EventData(functionName, ProfilerInstance.Ticks, EventKind.End));
    }
}

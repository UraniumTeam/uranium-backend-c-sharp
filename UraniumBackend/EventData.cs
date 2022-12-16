namespace UraniumBackend;

public readonly struct EventData
{
    public readonly string FunctionName;
    public readonly ulong Time;
    public readonly EventKind Kind;

    public EventData(string functionName, ulong time, EventKind kind)
    {
        FunctionName = functionName;
        Time = time;
        Kind = kind;
    }

    public void WriteTo(BinaryWriter writer, uint functionIndex)
    {
        switch (Kind)
        {
            case EventKind.Begin:
                writer.Write(functionIndex);
                break;
            case EventKind.End:
                writer.Write(functionIndex | (1 << 28));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        writer.Write(Time);
    }
}

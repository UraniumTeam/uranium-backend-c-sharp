using System.Diagnostics;
using System.Reflection;


namespace СSharp_Profiler;

[AttributeUsage(AttributeTargets.Method)]
public class ProfileFunc : Attribute, IDisposable
{
    private static readonly List<long>? _funcTimes = new List<long>();
    private readonly long _startTimeStampNs;
    private long _funcTime;

    public ProfileFunc()
    {
        /*Console.WriteLine("call");
        var t = typeof(Program);
        var methods = t.GetMethods(BindingFlags.NonPublic);
        var customAttrs = new List<IEnumerable<CustomAttributeData>>();
        foreach (var mInfo in methods)
        {
            customAttrs.Add(mInfo.CustomAttributes);;
        }*/
        
        _startTimeStampNs = Stopwatch.GetTimestamp();
        _funcTime = 0;
    }

    /*~ProfileFunc()
    {
        _funcTime = Stopwatch.GetTimestamp() - _startTimeStamp;
        _funcTimes?.Add(_funcTime);
        SaveSession();
    }*/

    public void SaveSession()
    {
        Console.WriteLine("Session saved");
        if (_funcTimes != null)
            foreach (var funcTime in _funcTimes)
            {
                Console.WriteLine(funcTime + " ticks");
            }
    }

    public void Dispose()
    {
        _funcTime = Stopwatch.GetTimestamp() - _startTimeStampNs;
        _funcTimes?.Add(_funcTime);
        SaveSession();
    }
}
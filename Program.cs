namespace СSharp_Profiler
{
    class Program
    {
        public static void Main()
        {
            Test();
            Test2();
            UraniumProfiler.SaveSession();
        }

        public static void Test()
        {
            using var scope = new ProfilerScope(nameof(Test));
            var count = 0;
            for (var i = 0; i < 2; i++)
            {
                count++;
            }
        }

        public static void Test2()
        {
            using var scope = new ProfilerScope(nameof(Test2));
            var count = 0;
            for (var i = 0; i < 1000000000; i++)
            {
                count++;
            }
        }
    }
}
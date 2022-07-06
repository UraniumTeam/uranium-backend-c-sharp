using СSharp_Profiler;

using var profiler = new ProfileFunc();

namespace СSharp_Profiler
{
    class Program
    {
        public static void Main()
        {
            Test();
        }

        [ProfileFunc]
        private static void Test()
        {
            var count = 0;
            for (var i = 0; i < 2; i++)
            {
                count++;
            }
        }
    }
}
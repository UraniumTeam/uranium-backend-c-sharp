using UraniumBackend;

namespace UraniumBackendTests
{
    class Program
    {
        public static void Main()
        {
            Test();
            SecondTest();
            Test();

            ProfilerInstance.Save();
        }

        public static void Test()
        {
            using var scope = ProfilerScope.Begin(nameof(Test));
            var count = 0;
            for (var i = 0; i < 2; i++)
            {
                count++;
            }
        }

        public static void SecondTest()
        {
            using var scope = ProfilerScope.Begin(nameof(SecondTest));
            var count = 0;
            for (var i = 0; i < 1000000000; i++)
            {
                count++;
            }
        }
    }
}
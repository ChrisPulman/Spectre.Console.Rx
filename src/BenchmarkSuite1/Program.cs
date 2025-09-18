using BenchmarkDotNet.Running;

namespace BenchmarkSuite1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DistinctAny
{
    class Program
    {
        static void Main(string[] args)
        {
            var evaluation = "A";

            var stw = Stopwatch.StartNew();
            Console.WriteLine("Start Y1");
            var y1 = new[] { "A", "A", "A", "A", "B" }
                .Any(i =>
                {
                    Thread.Sleep(1000);
                    return i == evaluation;
                });
            Console.WriteLine("Y1 completed {0}", stw.ElapsedMilliseconds);

            stw.Restart();
            Console.WriteLine("Start Y2");
            var y2 = new[] { "A", "A", "A", "A", "B" }
                .Distinct()
                .Any(i =>
                {
                    Thread.Sleep(1000);
                    return i == evaluation;
                });
            Console.WriteLine("Y2 completed {0}", stw.ElapsedMilliseconds);

            Console.ReadKey();
        }
    }
}
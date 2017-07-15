using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Humanizer;

namespace DistinctAny
{
    class Program
    {
        static void Main(string[] args)
        {
            var humanized = "ip-access-control".Humanize(LetterCasing.Title).Singularize();

            Regex IdentifierRegex = new Regex(@"^((?!.*//)[a-zA-Z0-9][a-zA-Z0-9/]*[a-zA-Z0-9]|[a-zA-Z0-9\*])$");

            var dates = new DateTimeOffset?[] {
                new DateTimeOffset(new DateTime(2017, 1, 1)),
                new DateTimeOffset(new DateTime(2017, 1, 2)),
                new DateTimeOffset(new DateTime(2017, 1, 3)),
                null
            };

            IPAddress address = IPAddress.Parse("1.1.1.1");
            IPAddress address2 = IPAddress.Parse("1.1.1.1");

            var x = IPAddress.TryParse("1.1.1.1", out IPAddress address3);

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
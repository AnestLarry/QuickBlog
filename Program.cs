using Fluid;
using Parlot.Fluent;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace QuickBlog
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            QuickBlog quickBlog = new QuickBlog();
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            quickBlog.main(args);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.ToString());
        }
    }
}
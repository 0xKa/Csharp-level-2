using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//StringBuilder is a mutable string type in C#
//that allows efficient modification of strings without creating new string instances.
/*
 * When to Use StringBuilder?
✅ When modifying a string multiple times (e.g., loops).
✅ When concatenating large strings (e.g., generating reports, logs).
✅ When memory efficiency is important.

❌ Avoid StringBuilder if:
🔹 You only need to modify a string once or twice (use string).
🔹 You are working with small strings (overhead isn't worth it).
 */

namespace _17_StringBuilder_vs_string
{
    internal class Program
    {
        static void ConcatenateStringBuilder(int iterations)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < iterations; i++)
            {
                result.Append("r");
            }

            Console.WriteLine("StringBuilder Result Length: " + result.Length);
        }
        static void ConcatenateString(int iterations)
        {
            string result = string.Empty;

            for (int i = 0; i < iterations; i++)
            {
                result += "r";
            }

            Console.WriteLine("Normal String Result Length: " + result.Length);
        }

        static void Main(string[] args)
        {
            int iterations = 250000;

            Stopwatch stopwatch1 = Stopwatch.StartNew();
            ConcatenateStringBuilder(iterations);
            stopwatch1.Stop();
            Console.WriteLine($"StringBuilder concatenation time: " + stopwatch1.ElapsedMilliseconds.ToString() + "ms\n");

            Stopwatch stopwatch2 = Stopwatch.StartNew();
            ConcatenateString(iterations);
            stopwatch2.Stop();
            Console.WriteLine($"String concatenation time: " + stopwatch2.ElapsedMilliseconds.ToString() + "ms\n");

            Console.Read();
        }
    }
}


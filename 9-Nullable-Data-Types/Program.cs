using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _9_Nullable_Data_Types
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //declare a Nullable int
            Nullable<int> num1 = null;

            //shortcut (?)
            int? num2 = null;

            if (num1.HasValue && num2.HasValue)
            {
                Console.WriteLine(num1.ToString());
                Console.WriteLine(num2.ToString());
            }
            else
                Console.WriteLine("Null");


            int? x = null, y = null;
            //using the null-coalescing operator  (??)
            int? result1 = x ?? 0; //assign default value if x was null

            int? result2 = x ?? y ?? 0; //assign default value if x and y was null

            int? result3 = (x.HasValue && y.HasValue ? x * y : 0); 

            Console.WriteLine("result1: " + result1);
            Console.WriteLine("result2: " + result2);
            Console.WriteLine("result3: " + result3);

            //using the null-conditional operator (?.)
            DateTime? date = DateTime.Now;
            string sValue = date?.ToString("dd/MMMM/yyyy") ?? "no date";
            Console.WriteLine(sValue);

            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Lambda Expression: it is a concise way to write anonymous methods (functions without a name).
//It simplifies delegate, LINQ, and event handling code.

//(parameters) => expression;
//=> is called the lambda operator (reads as "goes to").
//The left side is the input parameters.
//The right side is the expression or function body.

namespace _7_Lambda_Expression
{
    internal class Program
    {
        public delegate double Operation(double num1, double num2);


        public static void ExecuteOperation(string OperationName, double Num1, double Num2, Operation operation)
            => Console.WriteLine(OperationName + " Operation Result: " + operation.Invoke(Num1, Num2));

        public static Action<string, int> UserCard = (string name, int age) =>
        {
            Console.WriteLine("\n------------------");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Age: {age}");
            Console.WriteLine("------------------\n");
        };
    


        static void Main(string[] args)
        {
            ExecuteOperation("Add", 04, 5, (double num1, double num2) => num1 + num2); 
            ExecuteOperation("Sub", 20, 5, (double num1, double num2) => num1 - num2); 
            ExecuteOperation("Mult", 07, 5, (double num1, double num2) => num1 * num2); 
            ExecuteOperation("Div", 50, 5, (double num1, double num2) => num1 / num2);

            UserCard.Invoke("Reda", 20);


            Console.Read();
        }
    
    }
}

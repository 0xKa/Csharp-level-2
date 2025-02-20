using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5_Multicast_Delegate
{


    public class MulticastDelegateExample
    {
        public delegate void PrintDelegate(string message);

        static void PrintFirstName(string FirstName)
        {
            Console.WriteLine("First Name: " + FirstName);
        }
        static void PrintMiddleName(string MiddleName)
        {
            Console.WriteLine("Middle Name: " + MiddleName);
        }
        static void PrintLastName(string LastName)
        {
            Console.WriteLine("Last Name: " + LastName);
        }


        public static void Show()
        {
            PrintDelegate multiDelegate = null;


            multiDelegate += PrintFirstName;
            multiDelegate += PrintMiddleName;
            multiDelegate += PrintLastName;

            multiDelegate.Invoke("reda"); //will invoke all the subscribed function, and assign the same parameter for them 

            multiDelegate -= PrintMiddleName; //unsubscribe
            multiDelegate.Invoke("ahmed");

        }
    }

    public class MulticastDelegateExampleWithReturnType
    {
        public delegate int CalculateTwoNumbers(int num1, int num2);

        static int Add(int a, int b) { return a + b; }
        static int Multiply(int a, int b) { return a * b; }

        public static void Show()
        {
            CalculateTwoNumbers operation = null;

            operation += Add;
            operation += Multiply; //this is the last subscriber, so it will be returned when invoking


            Console.WriteLine( operation.Invoke(20, 4) );
            
        }

    }

    internal class Program
    {
        
        static void Main(string[] args)
        {
            //MulticastDelegateExample.Show();
            
            //MulticastDelegateExampleWithReturnType.Show();


            Console.ReadKey();
        }
    }
}
/*
What is a Multicast Delegate in C#?
A Multicast Delegate is a delegate that can hold references 
to multiple methods and execute them in sequence when invoked. 
This allows a single delegate instance to call multiple methods at once.
 */
//note if the function/delegate has a return type,it will only return the last function result

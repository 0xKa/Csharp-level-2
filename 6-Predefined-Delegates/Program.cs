using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//important: (Func, Action, Predicate, event) are just C# predefined/built-in delegates.
/*
1- Func<T, TResult> delegate takes parameters and returns a value.
2- Action<T> delegate takes parameters but returns void (no value).
3- Predicate<T> delegate takes a single parameter and returns bool.
4- event is a special type of multicast delegate used for notifications.
 */
// those are optional shortcuts, we can just use normal delegate.

public class FuncDelegate
{
    //last parameter is the return type of the delegate/function
    static Func<string, int, int> FuncDelegateExample;

    static int SquareArea(string message, int length)
    {
        Console.WriteLine(message);
        return length * 2;
    }

    public static void Show()
    {
        FuncDelegateExample = SquareArea;

        Console.WriteLine( FuncDelegateExample.Invoke("Square Area:", 5) );

    }

}

public class ActionDelegate
{
    //return type is void
    static Action<string, int> ActionDelegateExample;

    static void PrintNameAndAge(string name, int age)
    {
        Console.WriteLine($"Name: {name}, Age: {age}");
    }

    public static void Show()
    {
        ActionDelegateExample = PrintNameAndAge;

        ActionDelegateExample.Invoke("reda", 20);
    }

}

public class PredicateDelegate
{
    //return type is bool
    static Predicate<int> PredicateDelegateExample;

    static bool IsEqualsZero(int num)
    {
        return num == 0;
    }

    public static void Show()
    {
        PredicateDelegateExample = IsEqualsZero;

        Console.WriteLine( PredicateDelegateExample.Invoke(0) );
    }

}

public class EventDelegate
{
    public static event Action<string> MessageReceived; // Event using Action delegate

    public static void SendMessage(string message)
    {
        Console.WriteLine("Publisher Sending Message...");
        MessageReceived?.Invoke(message);
    }
    
    public static void Show()
    {
        MessageReceived += OnMessageReceived;

        SendMessage("Hello");
    }

    public static void OnMessageReceived(string message)
    {
        Console.WriteLine("Subscriber received: " + message);
    }


}



internal class Program
{
    static void Main(string[] args)
    {
        //FuncDelegate.Show();
        //ActionDelegate.Show();
        //PredicateDelegate.Show();
        //EventDelegate.Show();

        Console.Read();
    }
}
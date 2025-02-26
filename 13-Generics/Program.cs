using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Utility
{
    //Generic method
    public static T Swap<T>(ref T firstElement, ref T secondElement)
    {
        T temp = firstElement;
        firstElement = secondElement;
        secondElement = temp;

        return temp;
    }

    //Generic method
    public static void Print<T>(T firstElement, T secondElement)
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine("First Element  : " + firstElement);
        Console.WriteLine("Second Element : " + secondElement);
        Console.WriteLine("------------------------------");
    }

}

public class MyObject<T>
{
    public T Value { get; set; }

    public MyObject(T value) {
    
        this.Value = value;
    }

    public void PrintValue()
    {
        Console.WriteLine("---------------");
        Console.WriteLine("Value: " + Value);
        Console.WriteLine("---------------");
    }

}

internal class Program
{
    static void Main(string[] args)
    {
        string element1 = "hi";
        string element2 = "hello";

        //generic methods
        Utility.Print(element1, element2);
        Utility.Swap(ref element1, ref element2);
        Utility.Print(element1, element2);


        //generic class
        MyObject<string> myObject1 = new MyObject<string>("Reda");
        myObject1.PrintValue();
        
        MyObject<int> myObject2 = new MyObject<int>(99099);
        myObject2.PrintValue();
        
        MyObject<DateTime> myObject3 = new MyObject<DateTime>(DateTime.Now);
        myObject3.PrintValue();

        Console.ReadLine();
    }
}

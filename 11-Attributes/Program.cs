#define Enable_Func1
#define Enable_Func2

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 In C#, attributes are special metadata annotations that add additional information to 
classes, methods, properties, parameters, and other code elements. 
They allow you to attach declarative information that can be used at runtime using reflection.

Think of attributes as labels that provide extra instructions 
for how code should behave, be processed, or be documented.
 */

//Common Attributes:
/*
 * [Serializable] --> Applied to a class to indicate that its instances can be serialized.
 * [NonSerialized] --> Applied to a field to indicate that it should not be serialized.
 * [Conditional("")] --> To conditionally include or exclude methods from compilation based on the specified compilation symbols (string).
 * [Obsolete("")] --> Used to mark program entities (such as classes, methods, properties, etc.) that are considered obsolete or deprecated.
 * [AttributeUsage()] --> To create a custom attribute 
 */

[Serializable]
public class Serializable_Example
{
    // Will be serialized
    public int SerializedField = 32;


    // Will not be serialized
    [NonSerialized]
    public int NonSerializedField = -32;
}

public class Conditional_Example
{
    [Conditional("DEBUG")] //will execute the method only in Debug mode.
    public static void PrintName(string name)
    {
        Console.WriteLine(name);
    }
    public static void PrintAge(int age)
    {
        Console.WriteLine(age);
    }


    [Conditional("Enable_Func1")] //will execute the func is the string is defined in the program (check line 1)
    public static void Func1()
    {
        Console.WriteLine("Func1 is Enabled.");

    }
    
    [Conditional("Enable_Func2")] 
    public static void Func2()
    {
        Console.WriteLine("Func2 is Enabled.");
    }

    public static void Show()
    {
        Conditional_Example.PrintName("Reda");
        Conditional_Example.PrintAge(20);

        Conditional_Example.Func1();
        Conditional_Example.Func2();
    }

}

[Obsolete("this class is obsolete")]
public class Obsolete_Example
{
    [Obsolete("this method is old")]
    private static void Method1()
    {
        Console.WriteLine("I'm method 1");
    }
    
    private static void Method2()
    {
        Console.WriteLine("I'm method 2");
    }

    public static void Show()
    {
        Method1(); //generate a compiler warning
        Method2();
    }
}


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class CustomAttribute_Example : Attribute
{
    public string Description { get; }


    public CustomAttribute_Example(string description)
    {
        Description = description;
    }
}
public class MyCustomAttribute
{
    [CustomAttribute_Example("this is my attribute")]
    public static void CustomAttributeFunc()
    {
        Console.WriteLine("hello");
    }

}


namespace _11_Attributes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Conditional_Example.Show();

            //Obsolete_Example.Show();

            //MyCustomAttribute.CustomAttributeFunc();

            Console.Read();
        }
    }
}

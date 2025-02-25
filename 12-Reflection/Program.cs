using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace _12_Reflection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Type type = typeof(int);

            Console.WriteLine("Type Information:");
            Console.WriteLine($"Name      : {type.Name}");
            Console.WriteLine($"Full Name : {type.FullName}");
            Console.WriteLine($"Base Type : {type.BaseType}");
            Console.WriteLine($"Is Class  : {type.IsClass}");

            Console.Read();
        }
    }
}



/*
 * Reflection in C# refers to the ability of a program to inspect its own structure, metadata, and behavior at runtime.

Reflection in C# refers to the ability of a program to inspect, query, and interact with the metadata of types and members in an assembly at runtime. With reflection, you can obtain information about types, fields, properties, methods, and other members of your code dynamically, without knowing them at compile time.

With reflection, you can dynamically load assemblies, examine and create types, and invoke methods or access properties, all without knowing them at compile-time. It provides a way to manipulate types, objects, and members dynamically.



Key aspects of reflection in C# include:

Type Information: You can retrieve information about a type, such as its name, namespace, methods, properties, fields, events, and more.
Instantiation: You can create an instance of a type dynamically at runtime using the Activator.CreateInstance method.
Method Invocation: You can invoke methods on an object dynamically, even if you don't know the method at compile time, using the Invoke method.
Property and Field Access: You can get and set the values of properties and fields dynamically.
Reflection is powerful but comes with some trade-offs:

Performance: Reflection can have a performance overhead compared to statically typed code because it bypasses some of the optimizations performed by the compiler.
Type Safety: Since reflection allows dynamic interaction with types, there's a risk of runtime errors if the code is not carefully designed.


Practical uses of reflection:

One practical use of reflection in C# is in the development of frameworks and tools where the structure of types is not known at compile time. Reflection enables these frameworks to dynamically discover and interact with types provided by user code. One such example is in the development of dependency injection containers.
Another practical use of reflection in C# is in the development of serialization and deserialization frameworks. These frameworks convert objects into a format that can be easily stored, transmitted, or reconstructed, and reflection is often used to dynamically inspect and manipulate the structure of objects.
Another practical use of reflection in C# is in the development of unit testing frameworks. Unit testing frameworks often utilize reflection to dynamically discover and execute test methods within test classes.
Another practical use of reflection in C# is in the development of data access frameworks or Object-Relational Mapping (ORM) tools. Reflection can be employed to dynamically map object properties to database fields, allowing for flexible and generic data access without explicit knowledge of the underlying database schema.
Another practical use of reflection in C# is in the development of user interface frameworks or libraries that need to dynamically generate or bind UI elements based on the properties of data objects.
Another practical use of reflection in C# is in the implementation of extensibility mechanisms, where the application can dynamically discover and load extensions or plugins at runtime without having to know about them at compile time.
Another practical use of reflection in C# is in the development of attribute-based validation frameworks. By utilizing custom attributes and reflection, you can create a system where validation rules are associated with class properties, making it easier to define and enforce data validation rules.
Another practical use of reflection in C# is in the development of code generation tools or frameworks. Reflection allows you to inspect and analyze the structure of types at runtime, and this capability is often leveraged in code generation scenarios.
Another practical use of reflection in C# is in certain design patterns.
Reflection can also be utilized in the context of documentation generation, where you dynamically extract information about types, methods, properties, and other elements in your codebase to generate documentation automatically.
and may other uses :-)
 */

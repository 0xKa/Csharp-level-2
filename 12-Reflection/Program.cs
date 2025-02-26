using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyNamespace;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class MyCustomAttribute : Attribute
{
    public string Description { get; }


    public MyCustomAttribute(string description = "no description")
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RangeAttribute : Attribute
{
    public int Min { get; }
    public int Max { get; }

    public string ErrorMessage { get; set; }

    public RangeAttribute(int min = 0, int max = 0)
    {
        Min = min;
        Max = max;
    }
}

namespace MyNamespace
{
    [MyCustom("this is a class")]
    public class Person
    {
        // 🔹 Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Range(min: 18, max: 99,  ErrorMessage = "Age must be between 18 and 99")]
        public int Age { get; set; }
        
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        // 🔹 Constructor
        public Person(string firstName, string lastName, int age, string address, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public Person() { }

        // 🔹 Public Methods
        public void ShortIntroduce()
        {
            Console.WriteLine($"Hi, I'm {FirstName}.");
        }

        [MyCustom("This is the Introduce method")]
        public void Introduce()
        {
            Console.WriteLine($"Hi, I'm {FirstName} {LastName}, and I'm {Age} years old.");
        }

        public void UpdateAddress(string newAddress)
        {
            Address = newAddress;
            Console.WriteLine($"Address updated to: {Address}");
        }

        [MyCustom("This is the Call method")]
        public void Call()
        {
            Console.WriteLine($"Calling {PhoneNumber}...");
        }

        // 🔹 Private Method (Can only be used within this class)
        private void CalculateBirthYear()
        {
            int birthYear = DateTime.Now.Year - Age;
            Console.WriteLine($"Birth Year: {birthYear}");
        }

        // 🔹 Public Static Method (Can be called without creating an instance)
        [MyCustom("...")]
        public static void DisplayInfoFormat()
        {
            Console.WriteLine("Person Format: FirstName, LastName, Age, Address, PhoneNumber");
        }
    }
}

namespace _12_Reflection
{
    public class Reflection_test
    {
        public static void PrintTypeBasicInfo(Type type)
        {
            Console.WriteLine("Type Information:");
            Console.WriteLine($"Name      : {type.Name}");
            Console.WriteLine($"Full Name : {type.FullName}");
            Console.WriteLine($"Base Type : {type.BaseType}");
            Console.WriteLine($"Is Class  : {type.IsClass}\n\n");
        }
        
        public static void PrintAssemblyBasicInfo(Assembly assembly)
        {
            Console.WriteLine("Assembly Information:");
            Console.WriteLine($"Full Name  : {assembly.FullName}");
            Console.WriteLine($"Evidence   : {assembly.Evidence}");
            Console.WriteLine($"Code Base  : {assembly.CodeBase}");
            Console.WriteLine($"Location   : {assembly.Location}");
            Console.WriteLine($"Is Dynamic : {assembly.IsDynamic}\n\n");
        }


        // Formats a list of method parameters as a comma-separated string.
        private static string GetParameterList(ParameterInfo[] parameters)
        {
            return string.Join(", ", parameters.Select(parameter => $"{parameter.ParameterType} {parameter.Name}"));
        }
        public  static void Navigate_Library_Using_Reflection()
        {
            // 🔹 Get the assembly that contains the System.String type
            Assembly mscorlib = typeof(string).Assembly;

            // 🔹 Get the Type object representing System.String
            Type type = mscorlib.GetType("System.String");

            if (type != null)
            {
                Console.WriteLine($"\n\nMethods of [{type.FullName}] class:\n");

                // 🔹 Get all public instance(non-static) methods
                var Methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(method => method.Name);

                // 🔹 Iterate through each method and print its details
                foreach (var method in Methods)
                {
                    Console.WriteLine($"\t{method.ReturnType} {method.Name}({GetParameterList(method.GetParameters())})");
                }


            }
            else
                Console.WriteLine("System.String type not found.");


        }

        public static void Navigate_Class_Using_Reflection(Type myClass)
        {
            if (myClass == null || !myClass.IsClass)
                return;

            Console.WriteLine($"Class Name: {myClass.Name}");
            Console.WriteLine($"Full Name: {myClass.FullName}");


            // Get the properties of MyClass
            Console.WriteLine($"\n-Properties:");
            foreach (var property in myClass.GetProperties())
            {
                Console.WriteLine($"\tProperty Name: {property.Name}, Type: [{property.PropertyType}]");
            }

            // Get the methods of MyClass
            Console.WriteLine("\n-Methods:");
            foreach (var method in myClass.GetMethods())
            {
                Console.WriteLine($"\t{method.ReturnType} {method.Name}({GetParameterList(method.GetParameters())})");
            }



        }

        public static void Control_Person_Class_Using_Reflection()
        {
            Type type = typeof(Person);

            // Create an instance of Person
            object PersonInstance = Activator.CreateInstance(type);

            // Set the value of FirstName using reflection
            Console.WriteLine("\nFirstName Property is Set to 'Reda' using reflection.");
            type.GetProperty("FirstName").SetValue(PersonInstance, "Reda");


            // Get the value of FirstName using reflection
            Console.WriteLine("\nGetting FirstName is value using reflection:");
            string FirstNameValue = (string)type.GetProperty("FirstName").GetValue(PersonInstance);
            Console.WriteLine($"FirstName Value: {FirstNameValue}");


            //now how to execute methods using reflection:
            Console.WriteLine("\nExecuting Methods using reflection:");

            // Invoke the ShortIntroduce method using reflection
            type.GetMethod("ShortIntroduce").Invoke(PersonInstance, null);

            // Invoke UpdateAddress with parameters using reflection
            object[] parameters = { "Earth" };
            type.GetMethod("UpdateAddress").Invoke(PersonInstance, parameters);
        }

        public static void Reflection_with_Custom_Attributes()
        {
            Type type = typeof (Person);

            //get class-level attribute 
            object[] classAttributes = type.GetCustomAttributes(typeof(MyCustomAttribute), false);
            foreach (MyCustomAttribute attribute in classAttributes)
            {
                Console.WriteLine($"Class Attributes Description: [{attribute.Description}] ");
            }
            //get method-level attribute 
            MethodInfo methodInfo = type.GetMethod("Call");
            object[] methodAttributes = methodInfo.GetCustomAttributes(typeof(MyCustomAttribute), false);
            foreach (MyCustomAttribute attribute in methodAttributes)
            {
                Console.WriteLine($"Method Attributes Description: [{attribute.Description}] ");
            }

        }

    }

    public class RangeAttribute_test
    {
        private static bool ValidatePerson(Person person)
        {
            Type type = typeof(Person);
            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (PropertyInfo property in propertyInfos)
            {
                //check for RangeAttribute on properties
                if (Attribute.IsDefined(property, typeof(RangeAttribute)))
                {
                    RangeAttribute rangeAttribute = (RangeAttribute) Attribute.GetCustomAttribute(property, typeof(RangeAttribute));
                    int value = (int)property.GetValue(person);

                    //validation
                    if (value < rangeAttribute.Min || value > rangeAttribute.Max)
                    {
                        Console.WriteLine($"Validation Faild for property '{property.Name}'.");
                        Console.WriteLine($"Error Message: '{rangeAttribute.ErrorMessage}'.");
                        return false;
                    }
                }
            }
            return true;
        }

        public static void Show()
        {
            Person person = new Person("reda", "hilal", 100, "oman", "12341234");

            if ( ValidatePerson(person) )
                Console.WriteLine("Person is Valid.");
            else
                Console.WriteLine("Person is not Valid.");
        }
    }

    internal class Program
    {

        static void Main(string[] args)
        {
            Type type = typeof(Person);

            //Reflection_test.PrintTypeBasicInfo( type );

            //Reflection_test.PrintAssemblyBasicInfo( type.Assembly );

            //Reflection_test.Navigate_Library_Using_Reflection();

            //Reflection_test.Navigate_Class_Using_Reflection(typeof(Person));

            //Reflection_test.Control_Person_Class_Using_Reflection();

            //Reflection_test.Reflection_with_Custom_Attributes();

            //RangeAttribute_test.Show();

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

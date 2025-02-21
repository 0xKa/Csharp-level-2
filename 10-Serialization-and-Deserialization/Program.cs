using System;
using System.IO;
using System.Xml.Serialization; //XML Serialization
using System.Runtime.Serialization.Json; //JSON Serialization
using System.Runtime.Serialization.Formatters.Binary; //Binary Serialization
using System.Runtime.Serialization;

[Serializable] //Serializable attribute is required for Binary Serialization, not needed for XML & JSON
[DataContract] //this is for JSON Serialization, also the [DataMember] attribute
public class Student
{
    [DataMember] public int ID { get; set; }
    [DataMember] public string FirstName { get; set; }
    [DataMember] public string LastName { get; set; }
    [DataMember] public int Age { get; set; }
    [DataMember] public DateTime BirthDate { get; set; }
    [DataMember] public string Specialization { get; set; }
    [DataMember] public bool IsActive { get; set; }

    public Student(int ID, string FirstName, string LastName, int Age, DateTime BirthDate, string Specialization, bool IsActive)
    {
        this.ID = ID;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Age = Age;
        this.BirthDate = BirthDate;
        this.Specialization = Specialization;
        this.IsActive = IsActive;
    }

    // Parameterless constructor required for XmlSerializer
    public Student() { }

    public void PrintInfo()
    {
        Console.WriteLine("------ Student Info ------");
        Console.WriteLine($"ID: {ID}");
        Console.WriteLine($"Name: {FirstName} {LastName}");
        Console.WriteLine($"Age: {Age}");
        Console.WriteLine($"Birth Date: {BirthDate.ToShortDateString()}");
        Console.WriteLine($"Specialization: {Specialization}");
        Console.WriteLine($"Active: {(IsActive ? "Yes" : "No")}");
        Console.WriteLine("--------------------------\n");
    }
}

public class Program
{

    public static void XML_Serialization_Example()
    {
        Student student = new Student(1, "Reda", "Hilal", 20, new DateTime(2004, 8, 6), "CS", IsActive: true);

        XmlSerializer serializer = new XmlSerializer(typeof(Student));

        // ✅ Serialize to XML
        using (TextWriter writer = new StreamWriter("student.xml"))
        {
            serializer.Serialize(writer, student);
        }
        Console.WriteLine("✅ Student serialized successfully!\n");


        // ✅ Deserialize from XML
        Student deserializedStudent = null;
        using (TextReader reader = new StreamReader("student.xml"))
        {
            deserializedStudent = (Student)serializer.Deserialize(reader);
        }
        Console.WriteLine("✅ Student deserialized successfully!\n");
        deserializedStudent.PrintInfo();

    }
    public static void JSON_Serialization_Example()
    {
        Student student = new Student(1, "Reda", "Hilal", 20, new DateTime(2004, 8, 6), "CS", IsActive: true);

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Student));

        // ✅ Serialize to JSON
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.WriteObject(stream, student);
            string jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());

            File.WriteAllText("student.json", jsonString);
        }
        Console.WriteLine("✅ Student serialized successfully!\n");


        // ✅ Deserialize from JSON
        Student deserializedStudent = null;
        using (FileStream stream = new FileStream("student.json", FileMode.Open))
        {
            deserializedStudent = (Student)serializer.ReadObject(stream);
        }
        Console.WriteLine("✅ Student deserialized successfully!\n");
        deserializedStudent.PrintInfo();

    }
    public static void Binary_Serialization_Example()
    {
        Student student = new Student(1, "Reda", "Hilal", 20, new DateTime(2004, 8, 6), "CS", IsActive: true);

        BinaryFormatter formatter = new BinaryFormatter();

        // ✅ Serialize to Binary
        using (FileStream stream = new FileStream("student.bin", FileMode.Create))
        {
            formatter.Serialize(stream, student);
        }
        Console.WriteLine("✅ Student serialized successfully!\n");


        // ✅ Deserialize from Binary
        Student deserializedStudent = null;
        using (FileStream stream = new FileStream("student.bin", FileMode.Open))
        {
            deserializedStudent = (Student)formatter.Deserialize(stream);
        }
        Console.WriteLine("✅ Student deserialized successfully!\n");
        deserializedStudent.PrintInfo();

    }

    static void Main(string[] args)
    {

        //XML_Serialization_Example();
        //JSON_Serialization_Example();
        //Binary_Serialization_Example();


        Console.Read();
    }
}

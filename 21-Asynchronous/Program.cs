using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class clsAsynchronous
{


    private static async Task PrintText(string text)
    {
        await Task.Delay(3000);
        Console.WriteLine("Text = " +  text);
    }
    private static async Task PrintNum(int num)
    {
        await Task.Delay(1000);
        Console.WriteLine("Num = " +  num);
    }
    private static async Task<int> GetRandomNum()
    {
        await Task.Delay(4000);
        return new Random().Next(0, 100);
    }
    public static async Task ShowExample1()
    {
        Console.WriteLine("Start");

        Task task1 = PrintText("Five");
        Task task2 = PrintNum(5);
        Task<int> task3 = GetRandomNum();

        await Task.WhenAll(task1, task2); // Wait for both tasks to finish
        Console.WriteLine("End Printing (task1, task2)");

        await task3;
        Console.WriteLine("End Getting Random Number (task3)");

        int randomNum = await task3; 
        await PrintNum(randomNum);


        Console.WriteLine("End");
    }

}

internal class Program
{
    static async Task Main(string[] args)
    {
        await clsAsynchronous.ShowExample1();


        Console.Read();
    }
}
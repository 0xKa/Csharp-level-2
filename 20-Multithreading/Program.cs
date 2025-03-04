using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


public class clsMultithreading
{
    private static void Loop(string name, int count)
    {
        for (int i = 1; i <= count; i++)
        {
            Console.WriteLine(name + ": " + i);
            Thread.Sleep(1000);
        }
    }
    public static void ShowExample1()
    {
        //Main Function is the Main Thread
        Thread thread1 = new Thread(() => Loop("thread1", 3));
        Thread thread2 = new Thread(() => Loop("thread2", 20));

        thread1.Start();
        thread1.Join(); // wait at this line until thread1 finish (join the main thread)

        thread2.Start();

        Loop("Main", 20);

    }

    private static void DownloadWebPage(string url)
    {
        string content;

        using (WebClient webClient = new WebClient())
        {
            content = webClient.DownloadString(url);
        }
        
        Console.WriteLine($"{url}: [{Regex.Replace(content.Length.ToString(), @"\B(?=(\d{3})+(?!\d))", " ")}] characters downloaded.");
    }
    public static void ShowExample2()
    {
        Console.WriteLine("Starting Threads...\n");

        Thread thread1 = new Thread(() => DownloadWebPage(@"https://www.youtube.com/"));
        Thread thread2 = new Thread(() => DownloadWebPage(@"https://cnn.com/"));
        Thread thread3 = new Thread(() => DownloadWebPage(@"https://www.microsoft.com/"));

        Console.WriteLine("Thread1 started...");
        thread1.Start();
        
        Console.WriteLine("Thread2 started...");
        thread2.Start();
        
        Console.WriteLine("Thread3 started...\n");
        thread3.Start();


        thread1.Join();
        thread2.Join();
        thread3.Join();

        Console.WriteLine("\nAll Threads Finished Execution.");
    }

    private static int SharedCounter = 0;
    private static object lockObject = new object();
    private static void IncrementCounter(string ThreadName)
    {

        for (int i = 0; i < 10; i++)
        {
            /// Use lock to synchronize access to the shared counter
            lock (lockObject)
            {
                SharedCounter++;

                Console.WriteLine(ThreadName + ": " + SharedCounter);
                Thread.Sleep(500);
            }

            ///if you didn't lock the shared counter the value of it will be unexpected
            //SharedCounter++;
            //Console.WriteLine(ThreadName + ": " + SharedCounter);
            //Thread.Sleep(500);
        }

    }
    public static void ShowExample3()
    {
        /// Synchronization Example ///
        // Create threads that increment a shared counter
        Thread t1 = new Thread(() => IncrementCounter("t1"));
        Thread t2 = new Thread(() => IncrementCounter("t2"));
        Thread t3 = new Thread(() => IncrementCounter("t3"));


        t1.Start();
        t2.Start();
        t3.Start();


        // Wait for threads to complete
        t1.Join();
        t2.Join();
        t3.Join();


        Console.WriteLine("Final Counter Value: " + SharedCounter);

    }


}
internal class Program
{
    static void Main(string[] args)
    {
        //clsMultithreading.ShowExample1();

        //clsMultithreading.ShowExample2();

        //clsMultithreading.ShowExample3();

        Console.Read();
    }
}

/* Synchronous Vs. Multithreading Vs. Asynchronous programming in various aspects:

Vidio link: [https://www.youtube.com/watch?v=Vxhp6_WF2Is]

Concurrency Model:
Synchronous Programming: Concurrency is achieved through sequential execution. Each task is completed before moving on to the next, and the program waits for each operation to finish.
Multithreading: Concurrency is achieved by creating multiple threads of execution within a process. Threads can run independently, and the operating system scheduler decides when to switch between them.
Asynchronous Programming: Concurrency is achieved through non-blocking operations. The program can continue executing other tasks while waiting for certain operations (e.g., I/O or network requests) to complete.
Parallelism:
Synchronous Programming: Does not inherently support parallelism. Tasks are executed sequentially.
Multithreading: Supports parallelism as multiple threads can execute tasks simultaneously on multiple CPU cores.
Asynchronous Programming: Does not necessarily imply parallelism but enables efficient use of resources by allowing tasks to run concurrently without blocking the main thread.
Programming Model:
Synchronous Programming: Uses a straightforward, blocking model. Tasks are executed one after another in a linear fashion.
Multithreading: Requires explicit creation and management of threads, often involving synchronization mechanisms like locks.
Asynchronous Programming: Utilizes non-blocking constructs like callbacks, promises, or async/await. Facilitates the creation of non-blocking code, making it easier to handle multiple tasks concurrently.
Complexity and Safety:
Synchronous Programming: Typically less complex and easier to understand, but can lead to blocking and potential inefficiencies.
Multithreading: Introduces complexities related to thread synchronization, shared data safety, and potential race conditions. Requires careful management to avoid issues like deadlocks.
Asynchronous Programming: Can be more readable for certain tasks, especially I/O-bound operations. However, managing callbacks and ensuring proper error handling can introduce complexity.
Resource Overhead:
Synchronous Programming: Generally has lower resource overhead compared to multithreading.
Multithreading: Can have higher resource overhead due to the creation and management of multiple threads. Synchronization mechanisms add to complexity.
Asynchronous Programming: Tends to have lower resource overhead as it doesn't require creating and managing multiple threads.
Use Cases:
Synchronous Programming: Well-suited for simpler applications or scenarios where blocking operations do not significantly impact performance.
Multithreading: Suitable for CPU-intensive tasks (CPU-Bound tasks) that can be parallelized, such as complex calculations or image processing.
Asynchronous Programming: Well-suited for scenarios where I/O operations are prevalent, such as networking, file I/O, or database queries.
In summary, the choice between synchronous programming, multithreading, and asynchronous programming depends on the nature of the tasks, the specific requirements of the application, and the desired balance between simplicity, concurrency, and parallelism. In many cases, a combination of these techniques may be used to achieve optimal performance.
 */

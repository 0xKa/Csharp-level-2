using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


public class clsAsynchronous
{
    public class Example1
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
        public static async Task Show()
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

    public class Example2
    {
        private static async Task DownloadWebPageAsync(string url)
        {
            string content;

            using (WebClient webClient = new WebClient())
            {
                content = await webClient.DownloadStringTaskAsync(url);
            }

            Console.WriteLine($"{url}: [{Regex.Replace(content.Length.ToString(), @"\B(?=(\d{3})+(?!\d))", " ")}] characters downloaded.");
        }
        public static async Task Show()
        {
            Console.WriteLine("Starting Tasks...\n");

            Console.WriteLine("Task1 started...");
            Task task1 = DownloadWebPageAsync(@"https://www.youtube.com/");

            Console.WriteLine("Task2 started...");
            Task task2 = DownloadWebPageAsync(@"https://cnn.com/");

            Console.WriteLine("Task3 started...\n");
            Task task3 = DownloadWebPageAsync(@"https://www.microsoft.com/");


            await Task.WhenAll(task1, task2, task3);

            Console.WriteLine("\nAll Tasks Finished Execution.");
        }

    }

    public class Example3
    {
        //Custom Event Args
        public class CustomEventArgs : EventArgs
        {
            public string Name { get; }
            public int Age { get; }

            public CustomEventArgs(string name, int age)
            {
                Name = name;
                Age = age;
            }
        }
        //Custom Delegate and Event
        public delegate void CallbackEventHandler(CustomEventArgs e);
        public static event CallbackEventHandler Callback;
        //Async func
        static async Task PerformOperationAsync(CallbackEventHandler FuncToInvoke)
        {
            await Task.Delay(1000);

            //Sample Operation
            string Name = "Reda";
            int Age = 20;
            // when done we invoke the call back func

            FuncToInvoke?.Invoke(new CustomEventArgs(Name, Age));
        }
        static void PrintInfo(CustomEventArgs e)
        {
            Console.WriteLine("------------------");
            Console.WriteLine("Name = " + e.Name);
            Console.WriteLine("Age = " + e.Age);
            Console.WriteLine("------------------");
        }
        public static async Task Show()
        {
            Console.WriteLine("Performing Task...");
            Callback += PrintInfo;

            Task task = PerformOperationAsync(PrintInfo);
            Console.WriteLine("Working...");

            await task;

            Console.WriteLine("Done.");
        }
    }

    public class Example4
    {

        public static void DownloadFile(string filename, int sleeptime)
        {
            Console.WriteLine($"Download file: [{filename}] started.");
            Thread.Sleep(sleeptime); //stimulate the operation
            Console.WriteLine($"Download file: [{filename}] completed.");
        }

        public static async Task Show()
        {
            //.Run() uses multithreading, not async
            Task task1 = Task.Run(() => DownloadFile("file1.txt", 5000));
            Task task2 = Task.Run(() => DownloadFile("file2.txt", 1000));

            await Task.WhenAll(task1, task2);
            Console.WriteLine($"All Downloads Completed.");
        }


        /*
         * What is Task.Run?

Task.Run is a powerful method in C# that facilitates multithreading. It is part of the Task Parallel Library (TPL) and serves as a convenient way to execute code concurrently by offloading it to a background thread from the thread pool. This is particularly useful for scenarios where you want to keep the user interface (UI) responsive while computationally intensive or time-consuming tasks run in the background.


Benefits:

1. Improved Responsiveness:

By utilizing Task.Run, you ensure that long-running tasks are executed in the background, preventing them from blocking the UI. This enhances the overall responsiveness of your application, providing a smoother user experience.

2. Increased Efficiency:

 Task.Run enables the simultaneous utilization of multiple CPU cores, optimizing the execution of tasks. This can lead to improved efficiency and performance in scenarios where parallel processing is beneficial.

When to use it:

1. For Long-Running Tasks that Block the UI:

 Task.Run is particularly well-suited for tasks that have the potential to block the UI due to their duration. Offloading such tasks to a background thread ensures that the UI remains responsive to user interactions.

2. When You Need to Perform Multiple Tasks Concurrently:

 If your application involves multiple independent tasks that can run concurrently, Task.Run provides a straightforward way to parallelize the execution of these tasks.

3. When You Want to Improve Responsiveness and Efficiency:

 Task.Run is a valuable tool when the goals include both improving UI responsiveness and optimizing the overall efficiency of task execution through parallelism.



Remember:

1. Use Task.Run Judiciously:

While Task.Run is a powerful tool, excessive use can lead to the creation of too many threads, impacting performance negatively. Exercise caution and use it judiciously based on the specific needs of your application.

2. Manage Resources and Ensure Thread Synchronization:

To prevent potential issues such as race conditions, it is crucial to manage resources carefully and ensure proper thread synchronization. This involves using synchronization mechanisms when accessing shared resources to avoid conflicts.

By understanding Task.Run, you can leverage multithreading effectively in your C# applications, leading to more responsive and efficient code.
         */
    }


}

public class clsTaskFactory
{

    public class Example1
    {
        private static void DownloadFile(string filename, int sleeptime)
        {
            Console.WriteLine($"Download file: [{filename}] started.");
            Thread.Sleep(sleeptime); //stimulate the operation
            Console.WriteLine($"\nDownload file: [{filename}] completed.");
        }
        public static void Show()
        {
            //TaskFactory is basically an umbrella for many tasks

            // Define a cancellation token so we can stop the task if needed
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            // Create a TaskFactory with some common configuration
            TaskFactory taskFactory = new TaskFactory(token, TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);


            // Use the TaskFactory to create and start a new task
            Task task1 = taskFactory.StartNew(() => DownloadFile("file.doc", 1000));
            
            // Create another tasks using the same TaskFactory
            Task task2 = taskFactory.StartNew(() => DownloadFile("sample.txt", 3000));
            Task task3 = taskFactory.StartNew(() => DownloadFile("readme.md", 3500));
            Task task4 = taskFactory.StartNew(() => DownloadFile("text.bin", 4000));


            try
            {
                // Wait for both tasks to complete
                Task.WaitAll(task1, task2, task3, task4);
                Console.WriteLine($"All Downloads Completed.");
            }
            catch (AggregateException ae)
            {
                // Handle exceptions if any task throws
                foreach (var e in ae.InnerExceptions)
                    Console.WriteLine($"Exception: {e.Message}");
            }


            //we can cancel using the token
            //cts.Cancel();

            // Dispose of the CancellationTokenSource
            //cts.Dispose();
        }


    }



    /* TaskFactory Class:

﻿The TaskFactory class in C# is part of the Task Parallel Library (TPL) and is used to create and control tasks more efficiently.

TaskFactory is a class in the System.Threading.Tasks namespace that provides methods for creating and scheduling tasks.

It simplifies the process of working with tasks by offering a set of convenient factory methods.

It provides methods for creating and scheduling Task and Task<TResult> instances, thus offering more control over how these tasks are executed. The TaskFactory class can be especially useful when you need to create multiple tasks with common configuration settings, such as cancellation tokens, task creation options, or task continuations.

Key Uses of TaskFactory:
Creating Tasks: Simplify the creation of tasks, especially when you have multiple tasks that share similar configurations.
Task Scheduling: Manage how tasks are scheduled for execution. It can be used to queue tasks on a specific TaskScheduler.
Continuation Tasks: Easily create continuation tasks that execute after the completion of a previous task.
     */

    /* When to Use TaskFactory:
When you need multiple tasks with similar configurations.
When you need more control over task scheduling.
When working with task continuations or parent-child task relationships.
Remember, while TaskFactory is a powerful tool, it's not always necessary for simple task parallelism scenarios. The Task.Run method is often sufficient for running tasks without requiring specific configurations that TaskFactory provides.



TaskCreationOptions is an enumeration in the .NET Framework that provides several options to control the behavior of tasks created using the Task Parallel Library (TPL). These options give you more fine-grained control over how tasks are scheduled, executed, and how they behave in relation to other tasks. Here are some of the key options available in the TaskCreationOptions enumeration:

None: Specifies that the default configuration should be used. This is the most common setting when none of the other options are needed.
PreferFairness: Hints to the Task Scheduler that fairness should be prioritized. This means that tasks will be scheduled in the order they were queued, attempting to avoid situations where a later-created task runs before an earlier-created one.
LongRunning: Indicates that a task will be a long-running, coarse-grained operation involving fewer, larger components. This hint allows the Task Scheduler to optimize the execution of these tasks, possibly avoiding the overhead of context switching.
AttachedToParent: Specifies that a task is attached to a parent in the task hierarchy. When a parent task waits on its children, it will implicitly wait for all tasks attached to it as well.
DenyChildAttach: Prevents any child tasks from attaching to the current task. This option is useful when you want to create a task within another task but prevent it from being attached to the parent task.
HideScheduler: Prevents the ambient task scheduler from being seen as the current scheduler in the created task. This means that tasks created within this task will not see the current task's scheduler as the default scheduler.
RunContinuationsAsynchronously: Ensures that continuations added to the current task run asynchronously. This option can be used to avoid potential deadlocks and improve responsiveness.


TaskContinuationOptions is an enumeration in C# that provides a set of options that control the behavior of task continuations. Task continuations are created using methods like Task.ContinueWith, which allow you to specify actions that should be executed after a task completes. These options give you fine-grained control over when and how continuations are executed in relation to the task they are continuing from. Here are some of the key options available in the TaskContinuationOptions enumeration:

None: Specifies that the default behavior should be used, with no special conditions applied to the continuation task.
PreferFairness: Indicates that the continuation task should be scheduled in a fair manner in relation to other tasks. This can be useful in scenarios where you want to maintain a first-in, first-out (FIFO) order of task execution.
LongRunning: Suggests that the continuation will be a long-running operation. This option hints to the Task Scheduler that an additional thread might be advantageous for running the continuation, thus reducing the chance of it blocking other tasks.
AttachedToParent: Indicates that the continuation task is attached to the parent task in a nested task structure. The parent task will wait for the attached child continuation tasks to complete before it completes.
ExecuteSynchronously: Advises the Task Scheduler to try to execute the continuation synchronously on the same thread that ran the antecedent task, rather than queuing it to the ThreadPool. This can be more efficient for short continuations, but care should be taken as it can potentially lead to deadlocks or stack overflows.
LazyCancellation: Specifies that the continuation task should not be canceled immediately if its antecedent task is canceled. Instead, the continuation will start and then check the cancellation token, if any, to determine whether to continue executing.
NotOnRanToCompletion: Specifies that the continuation should not be executed if the antecedent task completes successfully.
NotOnFaulted: Specifies that the continuation should not be executed if the antecedent task throws an unhandled exception (faults).
NotOnCanceled: Specifies that the continuation should not be executed if the antecedent task is canceled.
OnlyOnRanToCompletion: Specifies that the continuation should only be executed if the antecedent task completes successfully.
OnlyOnFaulted: Specifies that the continuation should only be executed if the antecedent task throws an unhandled exception.
OnlyOnCanceled: Specifies that the continuation should only be executed if the antecedent task is canceled.
TaskScheduler is a fundamental class in the Task Parallel Library (TPL) in C#. It serves as the engine behind task scheduling, execution, and management. The TaskScheduler abstract class provides a means to queue tasks to the ThreadPool or to a custom scheduler.

Key Concepts of TaskScheduler:
Default Task Scheduler: By default, the TPL uses the ThreadPoolTaskScheduler. This default scheduler is efficient for most scenarios, balancing responsiveness and throughput. It queues tasks to the system's thread pool.
Custom Task Schedulers: You can implement custom task schedulers by deriving from the TaskScheduler class. This is useful for specialized scenarios, like scheduling tasks to run on a UI thread, handling tasks with specific priorities, or managing tasks in an environment with unique threading requirements.
Queuing Tasks: The TaskScheduler determines how tasks are queued and when they are executed. It abstracts the details of how tasks are managed, whether that's on a thread pool, a single thread, or some other mechanism.
Task Execution: The scheduler determines when and how a task is executed. This includes considerations like concurrency limits, prioritization, and dealing with unhandled exceptions in tasks.
Synchronization Context Integration: For applications with a synchronization context (like Windows Forms or WPF applications), the default scheduler can ensure that task continuations that update the UI are executed on the correct thread.
     */
}

public class clsParallel
{
    /// Parallel class make it use for us to use Multithreading 

    /* Parallel.For 
         * is a method in C# that allows you to execute a for loop in parallel,
         * making it easier to perform parallel operations on a collection or for 
         * a specific number of iterations.
         */
    public class Example1
    {



        public static void ShowUsingLambdaExpression()
        {
            int iterations = 10;
            //Parallel class will create 10 threads that work at the same time to execute the given lambda/function
            Parallel.For(0, iterations, i =>
            {
                //i is always predefined, it represent the current iteration
                Console.WriteLine($"Executing iteration {i} on thread [{Task.CurrentId}]");
                Task.Delay(10000).Wait(); //10 secs


            });

            Console.WriteLine("All Iterations Completed.");
        }
       

        public static void Operation(int i) 
        {
            Console.WriteLine($"Executing iteration {i} on thread [{Task.CurrentId}]");
            Task.Delay(10000).Wait(); //10 secs
        }
        public static void ShowUsingFunction()
        {
            int iterations = 10;
            //Parallel class will create 10 threads that work at the same time to execute the given lambda/function
            //i is always predefined, it represent the current iteration
            Parallel.For(0, iterations, Operation);

            Console.WriteLine("All Iterations Completed.");
        }

            

    }

    /* Parallel.ForEach 
         * is used in C# to execute a parallel loop over a collection or an enumerable. 
         * It can efficiently process items in parallel, improving performance for suitable tasks.
         */
    public class Example2
    {

    
        static List<string> urls = new List<string>
        {
            "https://www.cnn.com",
            "https://www.amazon.com",
            "https://www.programmingadvices.com",
            "https://www.microsoft.com",
            "https://www.google.com",
            "https://www.github.com",
            "https://www.youtube.com"
        };
        static void DownloadContent(string url)
        {
            string content;


            using (WebClient client = new WebClient())
            {
                // Simulate some work by adding a delay
                Thread.Sleep(100);


                // Download the content of the web page
                content = client.DownloadString(url);
            }


            Console.WriteLine($"{url}: {content.Length} characters downloaded");
        }
        
        public static void Show()
        {
            Parallel.ForEach(urls, DownloadContent);
            Console.WriteLine("Done.");
        }


    }

    /* Parallel.Invoke 
     * allows you to execute multiple actions in parallel
     */
    public class Example3
    {

        static void Function1()
        {
            Console.WriteLine("Function 1 is starting.");
            Task.Delay(3000).Wait(); // Simulating work
            Console.WriteLine("Function 1 is completed.");
        }
        static void Function2()
        {
            Console.WriteLine("Function 2 is starting.");
            Task.Delay(3000).Wait(); // Simulating work
            Console.WriteLine("Function 2 is completed.");
        }
        static void Function3()
        {
            Console.WriteLine("Function 3 is starting.");
            Task.Delay(3000).Wait(); // Simulating work
            Console.WriteLine("Function 3 is completed.");
        }
        public static void ShowUsingFunction()
        {
            // Run the functions in parallel (multithreading)
            Console.WriteLine("Starting parallel functions.");
            Parallel.Invoke(Function1, Function2, Function3); // or we can use lambda expression if the function requires parameters
            Console.WriteLine("All parallel functions are completed.");
        }
        public static void ShowUsingLambdaExpression()
        {
            Parallel.Invoke(
            () => Console.WriteLine($"Action 1 on thread {Task.CurrentId}"),
            () => Console.WriteLine($"Action 2 on thread {Task.CurrentId}"),
            () => Console.WriteLine($"Action 3 on thread {Task.CurrentId}")
            );
        }

    }


}

internal class Program
{
    static async Task Main(string[] args)
    {
        //await clsAsynchronous.Example1.Show();
        //await clsAsynchronous.Example2.Show();
        //await clsAsynchronous.Example3.Show();
        //await clsAsynchronous.Example4.Show();

        //clsTaskFactory.Example1.Show();

        //clsParallel.Example1.ShowUsingLambdaExpression();
        //clsParallel.Example1.ShowUsingFunction();
        //clsParallel.Example2.Show();
        //clsParallel.Example3.ShowUsingFunction();
        //clsParallel.Example3.ShowUsingLambdaExpression();

        Console.Read();
    }
}
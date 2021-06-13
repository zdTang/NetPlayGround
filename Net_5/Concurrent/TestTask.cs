﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5.Concurrent
{
    class TestTask
    {
        public static async void Test()
        {
            //The Task class provides a powerful abstraction for threading with which you can easily distinguish between the degree of parallelization in an application (the tasks) and the units of parallelization (the threads). On a single-processor computer, these items are usually the same. However, on a computer with multiple processors or with a multicore processor, they are different. (C# SBS)这段说明了TASK 与 THREAD的区别
            #region Start Task
            {
                //最简单的方法
                ////Task.Run(() => Console.WriteLine("Foo"));
                //Task.Run(() => { Thread.Sleep(2000); Console.WriteLine("Foo"); }); //TASK是BACKGROUND，如果MAIN THREAD走完，它就被忽视
                //Console.WriteLine("main thread");
            }
            #endregion

            #region Wait
            {
               // Task task = Task.Run(() =>
               // {
               //     Console.WriteLine("Task started");          //2.这句执行
               //     Thread.Sleep(2000);
               //     Console.WriteLine("Foo");                   //3.
               // });
               // Console.WriteLine(task.IsCompleted);  // False  1. 这句先执行了
               //// task.Wait();  // Blocks until task is complete  //类似于THREAD.JOIN(),主线程BLOCK在这里，等TASK结束
               //                                                 //这里很易引起歧义，好象是TASK在等，而实际上是在等TASK 
               //                                                 //如果没这一句，这个FOO还没打出来，就退出了，主线程不等 
            }
            #endregion

            #region Long Running Task
            {
                //Task task = Task.Factory.StartNew(() =>   //FACTORY模式
                //{
                //    Console.WriteLine("Task started");
                //    Thread.Sleep(2000);
                //    Console.WriteLine("Foo");
                //}, TaskCreationOptions.LongRunning);       //这个FACTORY有好多的OVERLOADING,这里是第二个参数设置LONGRUNNING

                //task.Wait();  // Blocks until task is complete
            }
            #endregion

            #region Returning Values
            {
   //             Task<int> task = Task.Run(() => { Console.WriteLine("Foo"); return 3; });

   //             int result = task.Result;      // Blocks if not already finished 主线程等在这里拿RESULT
   //             Console.WriteLine(result);    // 3

   //             Task<int> primeNumberTask = Task.Run(() =>
   //Enumerable.Range(2, 3000000).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));

   //             Console.WriteLine("Task running...");
   //             Console.WriteLine("The answer is " + primeNumberTask.Result); //主线程等在这里拿RESULT
            }
            #endregion

            #region Exception
            {
                //// Start a Task that throws a NullReferenceException:
                ////exception is automatically rethrown to whoever calls Wait()—or accesses the
                ////Result property of a Task<TResult>注意，谁在WAIT,谁在等着拿结果，就把EXCEPTION丢给谁
                //Task task = Task.Run(() => { throw null; });
                //try
                //{
                //    task.Wait();  //主线程CALL的这个
                //}
                //catch (AggregateException aex)
                //{
                //    if (aex.InnerException is NullReferenceException)
                //        Console.WriteLine("Null!");
                //    else
                //        throw;
                //}
            }
            #endregion

            #region Continuations-GetAwaiter
            //todo:  DOESN'T WORK  ==  已修改，加上主线程等候
            //这个主线程必须等候，如果主线程先退出了，程序就结束了
            //我理解这个POINT在于演示TASK一个接一个运行
 
            {
                //Task<int> primeNumberTask = Task.Run(() =>
                //    Enumerable.Range(2, 3000000).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
                ////primeNumberTask.Wait();            // MAIN THREAD WILL WAIT HERE
                //var awaiter = primeNumberTask.GetAwaiter();
                //awaiter.OnCompleted(() =>
                //{
                //    int result = awaiter.GetResult();
                //    Console.WriteLine(result);       // Writes result
                //});
                //// Console.WriteLine(primeNumberTask.Result);  // MAIN THREAD WILL WAIT HERE
                //Console.WriteLine("bye");
                ////主线程要等在这里才行
                //Thread.Sleep(6000);
            }

            #endregion

            #region Continuations - ContinueWith
            //todo:  DOESN'T WORK  ==修改，加上主线程等候
            //这个主线程必须等候，如果主线程先退出了，程序就结束了
            //我理解这个POINT在于演示TASK一个接一个运行
            {
                //// (See Chapter 22 for more on using ContinueWith.)

                //Task<int> primeNumberTask = Task.Run(() =>
                //    Enumerable.Range(2, 3000000).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
                ////primeNumberTask.Wait();
                //primeNumberTask.ContinueWith(antecedent =>
                //{
                //    int result = antecedent.Result;
                //    Console.WriteLine(result);          // Writes 123
                //});
                //Console.WriteLine("bye,bye");
                ////主线程要等在这里才行
                //Thread.Sleep(6000);

            }

            {
                //////https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/chaining-tasks-by-using-continuation-tasks
                //Console.WriteLine("I am coming! hi");
                //// Declare, assign, and start the antecedent task.
                //Task<DayOfWeek> taskA = Task.Run(() => DateTime.Today.DayOfWeek);

                //// Execute the continuation when the antecedent finishes.
                //taskA.ContinueWith(antecedent => Console.WriteLine($"Today is {antecedent.Result}."));
                //Console.WriteLine("I am leaving! bye");
                
                ////主线程要等在这里才行
                //Thread.Sleep(6000);
            }

            #endregion

            #region TaskCompletionSource

            {
                //var tcs = new TaskCompletionSource<int>();
                ////var myTask=tcs.Task;

                //new Thread(() => { Thread.Sleep(5000); tcs.SetResult(42); }).Start();

                //Task<int> task = tcs.Task;         // Our "slave" task.
                //Console.WriteLine(task.Result);   // 42 

            }
            //Our own Run Method
            {
                //// RUN需要一个无参，带返回值的DELEGATE
                //Task<int> task = Run(() => { Thread.Sleep(5000); return 42; });

                //task.Result.Dump();

                ////这个函数的意思是，你传来一个METHOD
                ////我启动一种THREAD执行这个METHOD,有了结果我返回
                //Task<TResult> Run<TResult>(Func<TResult> function)
                //{
                //    var tcs = new TaskCompletionSource<TResult>();
                //    new Thread(() =>
                //    {
                //        try { tcs.SetResult(function()); }  //拿到回调函数的结果
                //        catch (Exception ex) { tcs.SetException(ex); }
                //    }).Start();
                //    return tcs.Task;   // 返回TSC的SLAVE
                //}
            }
            //GetAnswerToLife
            {
                ////The real power of TaskCompletionSource is in creating tasks that
                ////don’t tie up threads. 
                ////Hence, our method returns a task that completes five seconds later, with a
                ////result of 42.
                //var awaiter = GetAnswerToLife().GetAwaiter();
                //awaiter.OnCompleted(() => Console.WriteLine(awaiter.GetResult())); //AWAITER要在5秒后才能拿到结果

                //Task<int> GetAnswerToLife()
                //{
                //    var tcs = new TaskCompletionSource<int>();
                //    // Create a timer that fires once in 5000 ms:
                //    var timer = new System.Timers.Timer(5000) { AutoReset = false };
                //    timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(42); };//ONCE THE TIME IS OVER 5000 绑定方法，一旦时间超过就执行
                //    timer.Start();
                //    return tcs.Task;
                //}
                ////主线程要等在这里才行
                //Thread.Sleep(6000);
            }

            #endregion

            #region Delay with TaskCompletionSource
            //We could make this more useful and turn it into a general-purpose Delay
            //method by parameterizing the delay time and getting rid of the return value.

            {
                //for (int i = 0; i < 10000; i++)
                //    Delay(5000).GetAwaiter().OnCompleted(() => Console.WriteLine(42));

                //Task Delay(int milliseconds)
                //{
                //    var tcs = new TaskCompletionSource<object>();
                //    var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
                //    timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(null); };
                //    timer.Start();
                //    return tcs.Task;
                //}
                //Console.WriteLine("Act please!");
                //Thread.Sleep(6000);//主人要在这里等着
            }

            // Task Delay
            // The Delay method that we just wrote is sufficiently useful that it’s
            // available as a static method on the Task class
            
            {
                //Task.Delay(5000).GetAwaiter().OnCompleted(() => Console.WriteLine(42));

                //// Another way to attach a continuation:
                //Task.Delay(5000).ContinueWith(ant => Console.WriteLine(42));

                //Console.WriteLine("Act please!");
                //Thread.Sleep(6000);//主人要在这里等着
            }
            // Value Task
            //TODO: 执行顺序很混乱，没有结果，主线程没有执行最后两句，为什么？

            {
                //var vt1 = AnswerQuestionAsync("What's the answer to life?");
                //var vt2 = AnswerQuestionAsync("Is the sun shining?");

                //Console.WriteLine($"vt1.IsCompleted: {vt1.IsCompleted}"); // True
                //Console.WriteLine($"vt2.IsCompleted: {vt2.IsCompleted}"); // False

                //var a1 = await vt1;
                //Console.WriteLine($"a1: {a1}"); // Immediate

                //var a2 = await vt2;
                //Console.WriteLine($"a2: {a2}"); // Takes 5 seconds to appear

                //async ValueTask<string> AnswerQuestionAsync(string question)
                //{
                //    if (question == "What's the answer to life?")
                //        return "42"; // ValueTask<string>

                //    return await AskCortanaAsync(question); // ValueTask<Task<string>>
                //}

                //async Task<string> AskCortanaAsync(string question)
                //{
                //    Console.WriteLine("I am Cortana!");
                //    await Task.Delay(5000);
                //    return "I don't know.";
                //}
                //Console.WriteLine("Act please!");   //这一句以及下一句没有执行，就退出了，为什么？
                //Thread.Sleep(10000);//主人要在这里等着
            }
            #endregion


        }


    }
}

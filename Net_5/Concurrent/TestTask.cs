using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5.Concurrent
{
    class TestTask
    {
        public static void Test()
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
                Task<int> task = Task.Run(() => { Console.WriteLine("Foo"); return 3; });

                int result = task.Result;      // Blocks if not already finished 主线程等在这里拿RESULT
                Console.WriteLine(result);    // 3

                Task<int> primeNumberTask = Task.Run(() =>
   Enumerable.Range(2, 3000000).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));

                Console.WriteLine("Task running...");
                Console.WriteLine("The answer is " + primeNumberTask.Result); //主线程等在这里拿RESULT
            }
            #endregion

            #region Exception
            {
                // Start a Task that throws a NullReferenceException:
                //exception is automatically rethrown to whoever calls Wait()—or accesses the
                //Result property of a Task<TResult>注意，谁在WAIT,谁在等着拿结果，就把EXCEPTION丢给谁
                Task task = Task.Run(() => { throw null; });
                try
                {
                    task.Wait();  //主线程CALL的这个
                }
                catch (AggregateException aex)
                {
                    if (aex.InnerException is NullReferenceException)
                        Console.WriteLine("Null!");
                    else
                        throw;
                }
            }
            #endregion
        }
    }
}

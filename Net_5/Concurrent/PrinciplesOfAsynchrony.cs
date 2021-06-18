using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5.Concurrent
{
    class PrinciplesOfAsynchrony
    {
        public static void Test()
        {
            #region GetPrimesCount === 同步，以及粗粒度异步

            {
                //DisplayPrimeCounts();  //这是同步的，要执行完

                //Console.WriteLine("=============");

                //Task.Run(() => DisplayPrimeCounts());  //asynchronous  //这是异步的，Coarse-grained Asynchrony,粗粒度异步

                //Console.WriteLine("Time to sleep!");
                ////Thread.Sleep(3000);//not enough
                //Console.ReadKey();
                //Console.WriteLine("Time to go!");

                //Console.WriteLine("=============");


            }

            #endregion

            #region Fine-grained Asynchrony 细粒度异步

            {
                //Console.WriteLine($"no_1  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //////如果主线程不给时间，则直接就退出了，主线程不会等

                //DisplayPrimeCounts();
                //Console.WriteLine($"no_2  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //void DisplayPrimeCounts()
                //{
                //    Console.WriteLine($"no_3  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    DisplayPrimeCountsFrom(0);
                //}

                //void DisplayPrimeCountsFrom(int i)      // This is starting to get awkward!
                //{
                //    Console.WriteLine($"no_4  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    var awaiter = GetPrimesCountAsync(i * 1000000 + 2, 1000000).GetAwaiter();
                //    awaiter.OnCompleted(() =>
                //    {
                //        Console.WriteLine($"no_5  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //        Console.WriteLine(awaiter.GetResult() + " primes between " + (i * 1000000) + " and " + ((i + 1) * 1000000 - 1));
                //        if (i++ < 10) DisplayPrimeCountsFrom(i);//这里用了RECURSION,在这里不断递调自己
                //        else Console.WriteLine("Done");
                //    });
                //}


                ////注意， 这是异步的 Task<int>
                //Task<int> GetPrimesCountAsync(int start, int count)
                //{
                //    return Task.Run(() =>
                //        ParallelEnumerable.Range(start, count).Count(n =>
                //            Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
                //}


                //Console.WriteLine("Time to sleep!");
                ////Thread.Sleep(3000);
                //Console.ReadKey();
                //Console.WriteLine("Time to go!");
                //Console.WriteLine($"no_6  ThreadID : {Thread.CurrentThread.ManagedThreadId}");

            }

            #endregion

            #region Making Asynchronous

            {
                //Console.WriteLine($"no_1  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //DisplayPrimeCountsAsync();
                //Console.WriteLine($"no_7  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //// 注意，TASK是没有返回值的，是VOID
                //// 这里用了TASK,却并没有涉及到线程
                //Task DisplayPrimeCountsAsync()
                //{
                //    Console.WriteLine($"no_2  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    var machine = new PrimesStateMachine();
                //    machine.DisplayPrimeCountsFrom(0);
                //    Console.WriteLine($"no_3  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    return machine.Task;
                //}

                //Console.WriteLine($"no_4  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine("Time to sleep!");
                //Console.ReadKey();

                //Console.WriteLine("Time to go!");
            }

            #endregion

            #region Asynchronous Functions to the rescue

            {
                //Console.WriteLine($"no_1 ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //DisplayPrimeCountsAsync();
                //Console.WriteLine($"no_2  ThreadID : {Thread.CurrentThread.ManagedThreadId}");

                //Console.WriteLine("Time to sleep!");
                //Console.ReadKey();

                //Console.WriteLine("Time to go!");
            }

            #endregion
        }


        static void DisplayPrimeCounts()
        {
            for (int i = 0; i < 10; i++)
                Console.WriteLine(GetPrimesCount(i * 1000000 + 2, 1000000) +
                                  " primes between " + (i * 1000000) + " and " + ((i + 1) * 1000000 - 1));

            Console.WriteLine("Done!");
        }

        //注意返回值是INT,这里是同步的
        static int GetPrimesCount(int start, int count)
        {
            return
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }

        //注意返回值是TASK,这里变成异步的了
        static async Task DisplayPrimeCountsAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"no_3  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine(await GetPrimesCountAsync(i * 1000000 + 2, 1000000) +
                                  " primes between " + (i * 1000000) + " and " + ((i + 1) * 1000000 - 1));

            }

            Console.WriteLine("Done!");
        }


        //注意返回值是TASK<INT>,这里变成异步的了
        static Task<int> GetPrimesCountAsync(int start, int count)
        {
            Console.WriteLine($"no_4  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }


    }

    class PrimesStateMachine        // Even more awkward!!
    {
        TaskCompletionSource<object> _tcs = new TaskCompletionSource<object>();
        public Task Task { get { return _tcs.Task; } }

        public void DisplayPrimeCountsFrom(int i)
        {
            Console.WriteLine($"no_5  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            var awaiter = GetPrimesCountAsync(i * 1000000 + 2, 1000000).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                Console.WriteLine($"no_6  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine(awaiter.GetResult());
                if (i++ < 10) DisplayPrimeCountsFrom(i);
                else { Console.WriteLine("Done"); _tcs.SetResult(null); } // non-generic Task 
            });
        }
         
        //注意返回值是TASK<INT>,这里变成异步的了
        Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }
    }
}

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
            #region GetPrimesCount

            {
                //DisplayPrimeCounts();

                //Console.WriteLine("=============");

                //Task.Run(() => DisplayPrimeCounts());

                //Console.WriteLine("Time to sleep!");
                //Thread.Sleep(3000);
                //Console.WriteLine("Time to go!");

                //Console.WriteLine("=============");


            }

            #endregion

            #region Fine-grained Asynchrony

            {
                ////如果主线程不给时间，则直接就退出了，主线程不会等
                //DisplayPrimeCounts();

                //void DisplayPrimeCounts()
                //{
                //    DisplayPrimeCountsFrom(0);
                //}

                //void DisplayPrimeCountsFrom(int i)      // This is starting to get awkward!
                //{
                //    var awaiter = GetPrimesCountAsync(i * 1000000 + 2, 1000000).GetAwaiter();
                //    awaiter.OnCompleted(() =>
                //    {
                //        Console.WriteLine(awaiter.GetResult() + " primes between " + (i * 1000000) + " and " + ((i + 1) * 1000000 - 1));
                //        if (i++ < 10) DisplayPrimeCountsFrom(i);
                //        else Console.WriteLine("Done");
                //    });
                //}

                //Task<int> GetPrimesCountAsync(int start, int count)
                //{
                //    return Task.Run(() =>
                //        ParallelEnumerable.Range(start, count).Count(n =>
                //            Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
                //}


                //Console.WriteLine("Time to sleep!");
                //Thread.Sleep(3000);
                //Console.WriteLine("Time to go!");

            }

            #endregion

            #region Making Asynchronous

            {
                //DisplayPrimeCountsAsync();

                //Task DisplayPrimeCountsAsync()
                //{
                //    var machine = new PrimesStateMachine();
                //    machine.DisplayPrimeCountsFrom(0);
                //    return machine.Task;
                //}


                //Console.WriteLine("Time to sleep!");
                //Thread.Sleep(3000);

                //Console.WriteLine("Time to go!");
            }

            #endregion

            #region Asynchronous Functions to the rescue

            {
                DisplayPrimeCountsAsync();


                Console.WriteLine("Time to sleep!");
                Thread.Sleep(3000);

                Console.WriteLine("Time to go!");
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

        static int GetPrimesCount(int start, int count)
        {
            return
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }

        static async Task DisplayPrimeCountsAsync()
        {
            for (int i = 0; i < 10; i++)
                Console.WriteLine(await GetPrimesCountAsync(i * 1000000 + 2, 1000000) +
                                  " primes between " + (i * 1000000) + " and " + ((i + 1) * 1000000 - 1));

            Console.WriteLine("Done!");
        }

        static Task<int> GetPrimesCountAsync(int start, int count)
        {
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
            var awaiter = GetPrimesCountAsync(i * 1000000 + 2, 1000000).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                Console.WriteLine(awaiter.GetResult());
                if (i++ < 10) DisplayPrimeCountsFrom(i);
                else { Console.WriteLine("Done"); _tcs.SetResult(null); }
            });
        }

        Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }
    }
}

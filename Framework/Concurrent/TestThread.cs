using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Framework.Concurrent
{
    class TestThread
    {
        public static void Test()
        {
            //todo:见nutshell,awaiter测试不成功
            //Tell();

            {
                Console.WriteLine("hi,one");
                Task<int> primeNumberTask = Task.Run(() =>
                    Enumerable.Range(2, 3000000).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));

                var awaiter = primeNumberTask.GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    int result = awaiter.GetResult();
                    Console.WriteLine(result);       // Writes result
                });
            }

            {
                // (See Chapter 22 for more on using ContinueWith.)
                Console.WriteLine("hi,two");
                Task<int> primeNumberTask = Task.Run(() =>
                    Enumerable.Range(2, 3000000).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));

                primeNumberTask.ContinueWith(antecedent =>
                {
                    int result = antecedent.Result;
                    Console.WriteLine(result);          // Writes 123
                });
            }



        }
        //TODO: 不好用，CONTINUATION通不过
        public static async void Tell()
        {

            //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/chaining-tasks-by-using-continuation-tasks
            Console.WriteLine("hi, there.");
            // Declare, assign, and start the antecedent task.
            Task<DayOfWeek> taskA = Task.Run(() => DateTime.Today.DayOfWeek);

            // Execute the continuation when the antecedent finishes.
            await taskA.ContinueWith(antecedent => Console.WriteLine($"Today is {antecedent.Result}."));
        }

    }

	
}

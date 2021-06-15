using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Concurrent
{
    class Asynchronous_Version_5
    {
        public static void TestAsync()//这个返回VOID是不可以的，有AWAIT,必须返回TASK
        {           
            DisplayPrimesCount();
            //DisplayPrimeCounts();
            Console.ReadKey();               //让主线程在这里等，是一种方法
            Console.WriteLine("hello");
        }
        static async void DisplayPrimesCount()
        {
            // AWAIT方式
            // 内部实际上用的AWAITER, 然后用GETRESULT()取出了值

            {
                int result = await GetPrimesCountAsync(2, 1000000);
                Console.WriteLine("hello，bottom!");
                Console.WriteLine(result);
            }
            // AWAITER方式 用AWAIT是可以取出值的
            {
                //var awaiter = GetPrimesCountAsync(2, 1000000).GetAwaiter();
                //var result = awaiter.GetResult();
                //Console.WriteLine(result);
            }

            
        }
        static Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
               ParallelEnumerable.Range(start, count).Count(n =>
                 Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

        async static Task DisplayPrimeCounts()
        {
            for (int i = 0; i < 10; i++)
                Console.WriteLine(await GetPrimesCountTwoAsync(i * 1000000 + 2, 1000000));
        }

        static Task<int> GetPrimesCountTwoAsync(int start, int count)
        {
            return Task.Run(() =>
           ParallelEnumerable.Range(start, count).Count(n =>
             Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

    }
}

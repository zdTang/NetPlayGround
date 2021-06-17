using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Net_5.Concurrent
{
    class Asynchronous_Stream
    {
        private static int _counter = 0;
        public static async  void Test()
        {
            {
                //Console.WriteLine($"Starting async Task<IEnumerable<int>>. Data arrives in one group.");

                //foreach (var data in await RangeTaskAsync(0, 10, 500))  //把DELAY的时间变长，就会看得清楚
                //    Console.WriteLine(data);

                //Console.WriteLine($"Starting async Task<IEnumerable<int>>. Data arrives as available.");

                //await foreach (var number in RangeAsync(0, 10, 5000))
                //    Console.WriteLine(number);
            }


            //结果如下,仔细分析，能看出一些流程的差异，这里可以看到处理的顺序
            /*
                IN  Main() ===ThreadID = 1
                Starting async Task<IEnumerable<int>>. Data arrives in one group.
                IN RangeTaskAsync=1==ThreadID=1
                RangeTaskAsync=1==ThreadID=1==Before Await    //延续一次进入RangeTaskAsync，遇到AWAIT
                BACK TO Main() ===ThreadID = 1                //主线程返回
                RangeTaskAsync=1==ThreadID=4==Async active    //DELAY结束，异步线程启动，处理它的CONTINUATION
                RangeTaskAsync=1==ThreadID=4==Before Await    //遇到AWAIT, 再次离开？
                RangeTaskAsync=1==ThreadID=4==Async active    //DELAY结束，异步启动。。
                RangeTaskAsync=1==ThreadID=4==Before Await
                RangeTaskAsync=1==ThreadID=4==Async active
                RangeTaskAsync=1==ThreadID=4==Before Await
                RangeTaskAsync=1==ThreadID=4==Async active
                RangeTaskAsync=1==ThreadID=4==Before Await
                RangeTaskAsync=1==ThreadID=4==Async active
                RangeTaskAsync=1==ThreadID=4==Before Await
                RangeTaskAsync=1==ThreadID=4==Async active
                RangeTaskAsync=1==ThreadID=4==Before Await
                RangeTaskAsync=1==ThreadID=4==Async active
                RangeTaskAsync=1==ThreadID=4==Before Await
                RangeTaskAsync=1==ThreadID=4==Async active
                RangeTaskAsync=1==ThreadID=4==Before Await
                RangeTaskAsync=1==ThreadID=5==Async active
                RangeTaskAsync=1==ThreadID=5==Before Await
                RangeTaskAsync=1==ThreadID=5==Async active
                OUT RangeTaskAsync=1==ThreadID=5==Return DATA
                0
                1
                2
                3
                4
                5
                6
                7
                8
                9
                Starting async Task<IEnumerable<int>>. Data arrives as available.
                IN RangeAsync=2==ThreadID=5
                RangeAsync=2==ThreadID=5==Async active
                0
                RangeAsync=2==ThreadID=5==Async active
                1
                RangeAsync=2==ThreadID=5==Async active
                2
                RangeAsync=2==ThreadID=4==Async active
                3
                RangeAsync=2==ThreadID=4==Async active
                4
                RangeAsync=2==ThreadID=4==Async active
                5
                RangeAsync=2==ThreadID=4==Async active
                6
                RangeAsync=2==ThreadID=4==Async active
                7
                RangeAsync=2==ThreadID=5==Async active
                8
                RangeAsync=2==ThreadID=4==Async active
                9
                Out  Main() ===ThreadID = 1
                Hello World!

             */

            //TODO: Asynchronous Streams and LINQ
            // NEED: System.Linq.Async  NuGet
            {
                //IAsyncEnumerable<int> query =
                //    from i in RangeAsyncTwo(0, 10, 5000)  //num_0  由于LAZY LOADING,这里开始是不执行的，只有被CALL时才执行
                //    where i % 2 == 0   // Even numbers only.
                //    select i * 10;     // Multiply by 10.
                //WriteLine($"Before FOR EACH===ThreadID={Thread.CurrentThread.ManagedThreadId}"); //num_1
                //await foreach (var number in query)
                //{
                //    WriteLine($"INNER FOR EACH===ThreadID={Thread.CurrentThread.ManagedThreadId}");//num_4
                //    Console.WriteLine(number);
                //}
                   

                //query.Dump();   // in LINQPad, you can directly dump IAsyncEnumerable<T>

                //{
                //    /* 流程
                //   IN  Main() ===ThreadID = 1               //由于LAZY LOADING,开始QUERY是不执行的 num_0，只有被CALL时才执行       
                //   Before FOR EACH===ThreadID=1             // 要第一次进入TEST()的FOREACH, num_1
                //   IN RangeAsyncTwo=1==ThreadID=1==I is 0   // QUERY 调用 RangeAsyncTwo，进入RangeAsyncTwo 的FOR LOOP, num_3
                //   BACK TO Main() ===ThreadID = 1           // 遇到AWAIT,返回主线程 MAIN 【异步线程遇到异步后，返回到哪里？】
                //   RangeAsyncTwo=1==ThreadID=4==Async active ==I is 0   //DELAY结束，异步线程启动，
                //   INNER FOR EACH===ThreadID=4               // 异步线程进入TEST()的FOR EACH中，num_4
                //   0                                         //异步线程执行Console.WriteLine(number);
                //   IN RangeAsyncTwo=1==ThreadID=4==I is 1    //异步线程继续执行CONTINUATION,再次进入QUERY,走到RangeAsyncTwo 的FOR LOOP, num_3
                //   RangeAsyncTwo=1==ThreadID=4==Async active ==I is 1  //异步线程遇到AWAIT DELAY返回，新的异步在DELAY后启动
                //   *******10被丢弃了
                //   IN RangeAsyncTwo=1==ThreadID=4==I is 2      //由于这里LINQ有个FILTER,有的数不需要，比如10，因而10进不到INNER那一步
                //   RangeAsyncTwo=1==ThreadID=4==Async active ==I is 2
                //   INNER FOR EACH===ThreadID=4
                //   20
                //   IN RangeAsyncTwo=1==ThreadID=4==I is 3
                //   RangeAsyncTwo=1==ThreadID=4==Async active ==I is 3
                //   ******30被丢弃了。。。。
                //   IN RangeAsyncTwo=1==ThreadID=4==I is 4
                //   RangeAsyncTwo=1==ThreadID=4==Async active ==I is 4
                //   INNER FOR EACH===ThreadID=4
                //   40
                //   IN RangeAsyncTwo=1==ThreadID=4==I is 5
                //   RangeAsyncTwo=1==ThreadID=5==Async active ==I is 5
                //   IN RangeAsyncTwo=1==ThreadID=5==I is 6
                //   RangeAsyncTwo=1==ThreadID=5==Async active ==I is 6
                //   INNER FOR EACH===ThreadID=5
                //   60
                //   IN RangeAsyncTwo=1==ThreadID=5==I is 7
                //   RangeAsyncTwo=1==ThreadID=5==Async active ==I is 7
                //   IN RangeAsyncTwo=1==ThreadID=5==I is 8
                //   RangeAsyncTwo=1==ThreadID=4==Async active ==I is 8
                //   INNER FOR EACH===ThreadID=4
                //   80
                //   IN RangeAsyncTwo=1==ThreadID=4==I is 9
                //   RangeAsyncTwo=1==ThreadID=5==Async active ==I is 9
                //   System.Linq.AsyncEnumerable+WhereSelectEnumerableAsyncIterator`2[System.Int32,System.Int32]
                //    Out  Main() ===ThreadID = 1
                //   Hello World!
                //*/
                //}


            }
        }

        static async Task<IEnumerable<int>> RangeTaskAsync(int start, int count, int delay)
        {
            _counter++;
            WriteLine($"IN RangeTaskAsync={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}");
            List<int> data = new List<int>();
            for (int i = start; i < start + count; i++)
            {
                WriteLine($"RangeTaskAsync={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}==Before Await");
                await Task.Delay(delay);// 这里，是每一个｛｝都要DELAY一次(各个DELAY互相重叠一部分？ )，还是只有第一次DELAY,
                                        // DELAY是一个结束，接一个来的。 后面的AWAIT部分是前面AWAIT的CONTINUATION(将DELAY时间变长看得更清)
                                        // 这个FOR的执行顺序，是一次全部展开？还是一个执行完一个｛｝，生成一个｛｝
                                        // 关于处理器如何处理LOOP有一些算法
                                        // Loop unwinding或loop unrolling
                WriteLine($"RangeTaskAsync={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}==Async active");
                data.Add(i);
            }
            WriteLine($"OUT RangeTaskAsync={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}==Return DATA");
            return data;
        }


        static async IAsyncEnumerable<int> RangeAsync(
            int start, int count, int delay)
        {
            _counter++;
            WriteLine($"IN RangeAsync={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}");
            for (int i = start; i < start + count; i++)
            {
                await Task.Delay(delay);
                WriteLine($"RangeAsync={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}==Async active");
                yield return i;
            }
        }

        static async IAsyncEnumerable<int> RangeAsyncTwo(
            int start, int count, int delay)
        {
            _counter++;
            for (int i = start; i < start + count; i++)
            {

                WriteLine($"IN RangeAsyncTwo={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}==I is {i}");  //num_3
                await Task.Delay(delay);
                WriteLine($"RangeAsyncTwo={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}==Async active ==I is {i}");
                yield return i;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Net_5.Concurrent
{
    class Asynchronous_Version_5
    {
        private static int _counter = 0;
        public  static async void Test()//这个返回VOID是不可以的，有AWAIT,必须返回TASK
        {
        

            #region Awaiting
            //对于AWAIT，三种形式等，一是TASK.WAIT(),二是TASK.RESULT,三是CONSOLE.READKEY()
            //如果返回类型不是TASK,则只能用CONSOLE.READKEY()等，否则主线程就提前跑掉了
            //如果返回TASK<T>，则通过取值方式等，在哪里取值，就在哪个位置等。
            //如果返回TASK,则通过WAIT()等，放在哪里，就在哪里等
            //如果一个方法返回VOID,则可以替换成TASK,非范形的TASK表示VOID
            //返回TASK<T>，相当于返回一个占位符，表示PLAN做点什么


            //第一种方式，用READKEY等候，否则主进程就执行跑掉了
            {
                ////由于这个方法返回是不是TASK,因而不能用WAIT
                //DisplayPrimesCount();
                //Console.ReadKey();
                ////让主线程在这里等，是一种方法
            }
            //第二种方式，有返回值的TASK，用取值的方式等候
            {
                //Task<int> task = DisplayPrimesCountTwo();
                //Console.WriteLine(task.Result);
                //Console.WriteLine("hello");
            }
            //  第三种情况，无返回值的TASK
            {
                //Task task = DisplayPrimesCountThree();
                //task.Wait();
                //Console.WriteLine("hello,Done");
            }

            //  第四种情况，把这个TestAsync方法改为 RETURN 一个TASK，从而可在上一层用WAIT()等结果
            //  本层用AWAIT， 这样，把取值，等结果的决定权又推到上更上一层
            //  从上层的视角看，MAIN->向下，一旦遇到AWAIT, CALL完后，不等结果即返回CALLING处
            {
                //await DisplayPrimesCountThree();
                ////ReadKey();                       //READKEY在这里没用，上一层READKEY有用，在主线程读到这个Test()，发现是AWAIT，即返回，在上层READKEY处等

                //Console.WriteLine("hello,Done"); //如果上面有READKEY(),则根本跑不这里，不会执行  TODO:为什么
            }



            #endregion

            #region Change GetAwait to await
            //此例中的调用方法时，不是通过TASK.RUN()调用的，而是主THREAD直接调用的， 因而不是THREAD BACKING的异步
            //因为，这个异步过程如果不用AWAIT,就只有一个线程，即主线程
            //这与一种情况是有相似，但还是有区别的：
            //比如用TASK.RUN()运行了另外一个THREAD, 那么主线程为了获取这个副线程的值，在哪里取值就可以在哪里BLACK等
            //但此时，异步只有主线程一人在干，没有生成另外一个线程
            
            {
                ////同步版本
                ////结果：虽然是异步，但只有一个线程
                ////主线程从开始现最后，它BLOCKING在取值的位置,如果离取值之前，又经历了一段时间，到取值时，可能不需要等，已经取出来了 
                ///*
                //    IN  Main() ===ThreadID = 1
                //    before delay,  ThreadID : 1
                //    In Delay,  ThreadID : 1
                //    Out Delay,  ThreadID : 1
                //    after delay,  ThreadID : 1
                //    Will Blocking here...
                //    100                       //主线程BLOCKING在这里等值，后面的也都由主线程完成
                //    READY TO GO,  ThreadID : 1
                //    Other stuff,  ThreadID : 1
                //    BACK TO Main() ===ThreadID = 1
                //     Out  Main() ===ThreadID = 1
                //    Hello World!

                // */

                //学会如何用TaskCompletionSource
                //由于返回的是一个TASK,因而增加了很多灵活性， TASK就是可等的
                //这里要别开的一个事情是，不是只有TASK.RUN（）才能生成TASK,没有NEW THREAD也能生成TASK
                Task<int> Delay(int milliseconds)//这个DELAY方法并没有用一个新THREAD完成
                {
                    Console.WriteLine($"In Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    var tcs = new TaskCompletionSource<int>();
                    var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
                    timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(100); };
                    timer.Start();
                    Console.WriteLine($"Out Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    return tcs.Task;
                }


                Console.WriteLine($"before delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                Task<int> delay = Delay(5000);
                Console.WriteLine($"after delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");


                Console.WriteLine("Will Blocking here...");
                Console.WriteLine(delay.Result);           //也可以直接拿值，各有利弊，见NETSHELL
                Console.WriteLine($"READY TO GO,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Other stuff,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            }
                //用AWAIT改写上面的程序，上面是用一个THREAD实现的异步，只有一个THREAD 1
                //下面的用了AWAIT, 在AWAIT之后，新的THREAD出现了  4，
                //结果，加了AWAIT
                /*
                 IN  Main() ===ThreadID = 1
                before delay,  ThreadID : 1
                In Delay,  ThreadID : 1
                Out Delay,  ThreadID : 1
                after delay,  ThreadID : 1
                Act please!
                BACK TO Main() ===ThreadID = 1  //此处，开始不同了，主线程返回，后面的由异步线程完成
                100
                READY TO GO,  ThreadID : 4      //另外又出现一个线程 4
                 Out  Main() ===ThreadID = 1
                Hello World!
                 */
                {
                //Task<int> Delay(int milliseconds)//这个DELAY方法并没有用一个新THREAD完成
                //{
                //    Console.WriteLine($"In Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    var tcs = new TaskCompletionSource<int>();
                //    var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
                //    timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(100); };
                //    timer.Start();
                //    Console.WriteLine($"Out Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    return tcs.Task;
                //}

                //Console.WriteLine($"before delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //Task<int> delay = Delay(5000);
                //Console.WriteLine($"after delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine("Act please!");
                //int result = await delay;            //主线程从这里返回到MAIN， 这里DELAY的确是个TASK,因而直接返回
                //Console.WriteLine(result);           //
                //Console.WriteLine($"READY TO GO,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            }
                //这个又改写一个AWAIT的位置
                /*
                    IN  Main() ===ThreadID = 1      // 从这看到，遇到AWAIT时，主线程要进入AWAIT表达式中，一直走出来
                    before delay,  ThreadID : 1
                    In Delay,  ThreadID : 1
                    Out Delay,  ThreadID : 1
                    BACK TO Main() ===ThreadID = 1  // 主线程离开
                    after delay,  ThreadID : 4      // AWAIT之后，就是异步线程在做CONTINUATION
                    Act please!
                    100
                    READY TO GO,  ThreadID : 4
                     Out  Main() ===ThreadID = 1
                    Hello World!
                 */
                {
                //    Task<int> Delay(int milliseconds)//这个DELAY方法并没有用一个新THREAD完成
                //        {
                //            Console.WriteLine($"In Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //            var tcs = new TaskCompletionSource<int>();
                //            var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
                //            timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(100); };
                //            timer.Start();
                //            Console.WriteLine($"Out Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //            return tcs.Task;
                //        }

                //        Console.WriteLine($"before delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //        var result= await Delay(5000);  //主线程从这里返回到MAIN， 这里DELAY的确是个TASK,因而直接返回
                //        Console.WriteLine($"after delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //        Console.WriteLine("Act please!");
                         
                //        Console.WriteLine(result);           //
                //        Console.WriteLine($"READY TO GO,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //}

            }

            #endregion

            #region Capturing Local State

            {
                //DisplayPrimeCountsFour();
                //ReadKey();
                //WriteLine("BYE");
            }

            #endregion

            #region Asynchronous Function
            // Return Task 返回VOID 或 TASK
            {
                //Go();
                //ReadKey();
                //Console.WriteLine("hello，bottom!");
            }
            // Return Task<T>
            {
                //GoTwo();
                //Console.WriteLine("want scape??");
                //ReadKey();
                //Console.WriteLine("time to go");
            }
            //  BLOCKING VERSION------同步版本,不需要让MAIN THREAD 等
            {
                //GoBlock();
            }

            #endregion

            #region Parallelism

            {
                //GoParallel();
                //Console.WriteLine("want scape? wait!!");
                //ReadKey();
                //Console.WriteLine("time to go");
            }

            #endregion

            #region Asynchronous Lambda Expression
            {
                //// Unnamed asynchronous method:
                //// return value is a Task
                //// 注意要加ASYNC
                //Func<Task> unnamed = async () =>
                //{
                //    WriteLine($"IN unnamed={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //    await Task.Delay(1000);
                //    Console.WriteLine("Foo");
                //    WriteLine($"OUT unnamed={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //};
                //// We can call the two in the same way:
                //WriteLine($"IN Test={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //await NamedMethod(); //这是个关键点，线程返回CALLER后，这里要记录下这可AWAITABLE的TASK的CONTINUATION,
                //                     //这包括这个NAMEMETHOD中AWAIT后没有完成的语句，，以及下面的AWAIT UNMAMED()
                //                     //要理解这个AWAIT是个语法糖，相当于NAMEDMETHOD.GETAWAITER(),  AWAITER.ONCOMPLETE()
                //await unnamed();
                //WriteLine($"OUT Test={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            }
            #endregion

            #region Returning Task of TResult
            {
                //// 注意要加ASYNC
                //Func<Task<int>> unnamed = async () =>
                //{
                //    WriteLine($"IN unnamed={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //    await Task.Delay(1000);//这里是关键点，新的线程要在DELAY结束时启动，执行后面的执作
                //    WriteLine($"OUT unnamed={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //    return 123;
                //};
                //WriteLine($"IN Test={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //int answer = await unnamed(); //这里是关键点，主线程遇到DELAY后，要在这里返回CALLER,即MAIN中
                //                               //而异步新线程执行完毕后，会返回这里，将值交给ANSWER,并向下进行
                //                               //下面的代码是这个UNNAME的CONTINUATION
                //answer.Dump();
                //WriteLine($"OUT Test={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            }
            #endregion

            #region Optimizations=Completing Synchronously
            {
                //WriteLine($"IN Test=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //string html = await GetWebPageAsync("http://www.linqpad.net");
                //WriteLine($"Back to Test First=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //html.Length.Dump("Characters downloaded");

                //// Let's try again. It should be instant this time:
                //html = await GetWebPageAsync("http://www.linqpad.net");
                //WriteLine($"Back to Test Second=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //html.Length.Dump("Characters downloaded");
                //WriteLine($"OUT Test=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            }
            #endregion

            #region Optimization=Caching Tasks
            {

                //WriteLine($"IN Test=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //string html = await GetWebPageAsyncTwo("http://www.linqpad.net");
                //WriteLine($"Another Thread Back to Test First=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //html.Length.Dump("Characters downloaded");

                //// Let's try again. It should be instant this time:
                //html = await GetWebPageAsyncTwo("http://www.linqpad.net");
                //html.Length.Dump("Characters downloaded");
                //WriteLine($"OUT Test=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            }
            #endregion

            #region Optimization = Caching Tasks fully threadsafe
            {
                ////加上了CACHE
                //WriteLine($"IN Test=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //string html = await GetWebPageAsyncThree("http://www.linqpad.net");
                //WriteLine($"Another Thread Back to Test First=ThreadID={Thread.CurrentThread.ManagedThreadId}");
                //html.Length.Dump("Characters downloaded");

                //// Let's try again. It should be instant this time:
                //html = await GetWebPageAsyncThree("http://www.linqpad.net");
                //html.Length.Dump("Characters downloaded");
                //WriteLine($"OUT Test=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            }
            #endregion

            #region Optimizations - Avoiding excessive bouncing
            {
                //A();
            }
            #endregion

        }
        static async void A()
        {
            await B();
        }

        static async Task B()
        {
            for (int i = 0; i < 1000; i++)
                await C().ConfigureAwait(false);
        }
        //todo: 为什么这里加上AWAIT就可以了
        static async Task<string> C() { return await new WebClient().DownloadStringTaskAsync("http://www.linqpad.net"); }


        static Dictionary<string, Task<string>> _cacheThree = new Dictionary<string, Task<string>>();

        static Task<string> GetWebPageAsyncThree(string uri)
        {
            lock (_cacheThree)
            {
                WriteLine($"IN GetWebPageAsyncThree={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}");
                Task<string> downloadTask;
                if (_cacheThree.TryGetValue(uri, out downloadTask)) return downloadTask;
                WriteLine($"IN GetWebPageAsyncThree={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}==Async");
                return _cacheThree[uri] = new WebClient().DownloadStringTaskAsync(uri); //这是异步的？ 这个对象是AWAITABLE,不加AWAIT会怎样？
            }
        }
        static Dictionary<string, Task<string>> _cacheTwo =  new Dictionary<string, Task<string>>();

        static Task<string> GetWebPageAsyncTwo(string uri)
        {
            _counter++;
            WriteLine($"IN GetWebPageAsyncTwo={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}");
            Task<string> downloadTask;
            if (_cacheTwo.TryGetValue(uri, out downloadTask)) return downloadTask;
            WriteLine($"IN GetWebPageAsyncTwo={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}==Async");
            return _cacheTwo[uri] = new WebClient().DownloadStringTaskAsync(uri); //这是异步的？ 是
        }
        
        static Dictionary<string, string> _cache = new Dictionary<string, string>();

        static async Task<string> GetWebPageAsync(string uri)
        {
            _counter++;
            WriteLine($"IN GetWebPageAsync={_counter}==ThreadID={Thread.CurrentThread.ManagedThreadId}");
            string html;
            if (_cache.TryGetValue(uri, out html)) return html;  //第二次是同步的，直接取回值，而不是由异步线程送回
            WriteLine($"IN GetWebPageAsync=ThreadID={Thread.CurrentThread.ManagedThreadId}==Async");
            return _cache[uri] = await new WebClient().DownloadStringTaskAsync(uri);  //第一次是异步的
        }

        static async Task NamedMethod()
        {
            WriteLine($"IN NamedMethod={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000);
            Console.WriteLine("Foo");
            WriteLine($"OUT NamedMethod={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
        }
        static async void DisplayPrimesCount()
        {
            // AWAIT方式
            // 内部实际上用的AWAITER, 然后用GETRESULT()取出了值

            {
                //int result = await GetPrimesCountAsync(2, 1000000);
                //Console.WriteLine("hello，bottom!");
                //Console.WriteLine(result);
            }
            // AWAITER方式 用AWAIT是可以取出值的
            {
                //var awaiter = GetPrimesCountAsync(2, 1000000).GetAwaiter();
                //var result = awaiter.GetResult();
                //Console.WriteLine(result);
            }

            
        }
        //本例由上面改变，有返回值的TASK
        static async Task<int> DisplayPrimesCountTwo()
        {
            int result = await GetPrimesCountAsync(2, 1000000);
            return result;
        }

        //本例由上面改变，无返回值的TASK
        static async Task DisplayPrimesCountThree()
        {
            int result = await GetPrimesCountAsync(2, 1000000);
            Console.WriteLine("hello，bottom!");
            Console.WriteLine(result);
        }

        static Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
               ParallelEnumerable.Range(start, count).Count(n =>
                 Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }


        //TODO: 这个LOOP的执行顺序看不清楚==大体清楚，是多个AWAIT的流程以及Continuation
        //如果将数字变大，发现是按顺序执行的，如同同步，先打LINE==,再经过一段时间计算，然后再打下一个LINE==
        //这是不是COARSE ASYNCHRONOUS? 也就是说当TEST()中一执行这个DisplayPrimeCountsFour，发现AWAIT,就退到READKEY处等候了
        //然后这里再慢慢执行
        //AWAIT表示一个TASK 要被ASYNCHRONOUS
        //这个LOOP中，相当于有多个AWAIT.这里要清楚多个AWAIT时的执行顺序
        //这里有一个概念：ＣＯＮＴＩＮＵＡＴＩＯＮ．第一个ＡＷＡＩＴ余下的CODE,这里，第一个AWAIT之后还有9个AWAIT,都是第一个的CONTINUATION.
        //在第一个AWAIT完成后，再完成第二个，然后后面的8个是它的CONTINUATION
        //async method is a method that returns to the calling method before completing all its work and then completes
        //its work while the calling method continues its execution.[illustration] 这就是个async method
        //TODO: 这段来自ILLUSTRATION,讲解CONTINUATION
        //The continuation: This is the rest of the code in the method, following the await
        //expression.This is packaged up along with its execution environment, which
        //includes the information about which thread it’s on, the values of the variables
        //currently in scope, and other things it’ll need in order to resume execution later, after
        //the await expression completes.
        static async void DisplayPrimeCountsFour()
        {
            for (int i = 0; i < 10; i++)
            {
                WriteLine($"line=={i}");//如果把下面的10000000多加一个0，就会看出效果，会一行接一行打印，而不是先将LINE==全部打出来
                //理解了CONTINUATION，就明白为什么结果会是这样的
                Console.WriteLine(await GetPrimesCountAsyncTwo(i * 1000000 + 2, 1000000));
            }
               
        }

        static Task<int> GetPrimesCountAsyncTwo(int start, int count)
        {
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

        static async Task Go()
        {
            await PrintAnswerToLife();
            Console.WriteLine("Done");    //CONTINUATION
        }

        static async Task PrintAnswerToLife()   // We can return Task instead of void
        {
            await Task.Delay(5000);
            int answer = 21 * 2;          //CONTINUATION
            Console.WriteLine(answer);    //CONTINUATION  == 
        }


        static async Task GoTwo()
        {
            await PrintAnswerToLife();
            Console.WriteLine("Done");
        }

        static async Task PrintAnswerToLifeTwo()
        {
            int answer = await GetAnswerToLifeTwo();
            Console.WriteLine(answer);
        }

       static  async Task<int> GetAnswerToLifeTwo()
        {
            await Task.Delay(5000);
            int answer = 21 * 2;
            return answer;
        }


       //  BLOCKING VERSION------
       static void GoBlock()
       {
           PrintAnswerToLifeBlock();
           Console.WriteLine("Done");
       }
        static void PrintAnswerToLifeBlock()
       {
           int answer = GetAnswerToLifeBlock();
           Console.WriteLine(answer);
       }

       static int GetAnswerToLifeBlock()
       {
           Thread.Sleep(5000);
           int answer = 21 * 2;
           return answer;
       }
       //  Parallel VERSION------
       //  TODO: ASYNCHRONOUS 和 PARALLEL并不是一回事
        static async Task GoParallel()
       {
           //下面四种格式，前两个格式是并发的，效率高，同时启动两个异步，两个异步并发执行
           //两个异步之间不是同步关系，也是异步关系
           //todo: 这是并行，两个结果同时出现,这是主线程引发了两次异步，两个异步由不同的THREAD接手
           {
                //var task1 = PrintAnswerToLifeParallel();
                //var task2 = PrintAnswerToLifeParallel();
                //await task1;      //TASK1是一个结果，是个TASK,
                //await task2;      //TASK2是一个结果，是个TASK,
                /////*
                ////IN PrintAnswerToLifeParallel=1=ThreadID=1    //主线程进入PrintAnswerToLife
                ////IN GetAnswerToLifeParallel  =2=ThreadID=1    //主线程进入GetAnswerToLife, 遇到DELAY的AWAIT, 返回这里，执行第二行 var task2
                ////IN PrintAnswerToLifeParallel=3=ThreadID=1    //主线程进入PrintAnswerToLife
                ////IN GetAnswerToLifeParallel  =4=ThreadID=1    //主线程进入GetAnswerToLife
                ////want scape? wait!!                           //又执行到了DELAY 的AWAIT，返回这里，执行 AWAIT TASK1,又返回到TEST()中，执行WRITELINE()后，等候在那里
                ////EXIT GetAnswerToLifeParallel  =4=ThreadID=4  //AWAIT 结束后， 异步线程启动，开始执行CONTINUATION
                ////EXIT GetAnswerToLifeParallel  =4=ThreadID=5  //这里可以看出有两个线程在反向执行CONTINUATION,4和5
                ////EXIT PrintAnswerToLifeParallel=4=ThreadID=4
                ////EXIT PrintAnswerToLifeParallel=4=ThreadID=5
                ////42
                ////42
                ////Done
                //// time to go
                ////Hello World!
                ////*/
            }

            //TODO: 这是并行，两个结果同时出现,这是主线程引发了两次异步，两个异步由不同的THREAD接手
            {
                //PrintAnswerToLifeParallel();
                //PrintAnswerToLifeParallel();

                ///*  顺序如下
                //    IN PrintAnswerToLifeParallel=1=ThreadID=1
                //    IN GetAnswerToLifeParallel  =2=ThreadID=1
                //    IN PrintAnswerToLifeParallel=3=ThreadID=1
                //    IN GetAnswerToLifeParallel  =4=ThreadID=1
                //    Done
                //    want scape? wait!!
                //    EXIT GetAnswerToLifeParallel=4=ThreadID=4
                //    EXIT GetAnswerToLifeParallel=4=ThreadID=5
                //    EXIT PrintAnswerToLifeParallel=4=ThreadID=4
                //    EXIT PrintAnswerToLifeParallel=4=ThreadID=5
                //    42
                //    42
                // */
            }
            //以下两种情况大体是相同的，即第一个异步结束后，第二个异步才开始，而两个异步之间，实际上是同步关系
            //TODO: 这是异步，一前一后出现， 这种是第一次异步之后，要等第一个异步结束了，再引发第二个异步
            //虽然有两个异步，但两个异步之间是同步关系，一前一后
            {
                //await PrintAnswerToLifeParallel();             // 注意写法，当写成这样时，await task1; 直接返回上一层
                //await PrintAnswerToLifeParallel();             // 写成这样时，虽然有AWAIT,却仍然进入方法，为什么？
                /////*
                ////    IN PrintAnswerToLifeParallel=1=ThreadID=1    // 主线程依次进入两个METHOD
                ////    IN GetAnswerToLifeParallel  =2=ThreadID=1
                ////    want scape? wait!!                           // 遇到最后一个AWAIT, 即TASK.DELAY处，开始返回到TASK()主线程                 
                ////    EXIT GetAnswerToLifeParallel=2=ThreadID=4    // DELAY结束，启动新线程 4，执行CONTINUATION
                ////    EXIT PrintAnswerToLifeParallel=2=ThreadID=4  // 从GetAnswerToLife返回PrintAnswerToLife
                ////    42
                ////    IN PrintAnswerToLifeParallel=3=ThreadID=4
                ////    IN GetAnswerToLifeParallel  =4=ThreadID=4
                ////    EXIT GetAnswerToLifeParallel=4=ThreadID=4
                ////    EXIT PrintAnswerToLifeParallel=4=ThreadID=4
                ////    42
                ////    Done
                ////     time to go
                ////    Hello World!
                //// */
            }
            //TODO: 
            {
                //var task1 = PrintAnswerToLifeParallel();  //1
                //await task1;                            //2   //注意，这个LINE2的CONTINUATION包含了下面子句
                //var task2 = PrintAnswerToLifeParallel();  //3
                //await task2;                            //4    

                /////*
                ////    IN PrintAnswerToLifeParallel  =1=ThreadID=1            // 主线程进入PrintAnswerToLife
                ////    IN GetAnswerToLifeParallel    =2=ThreadID=1            // 主线程进入GetAnswerToLife
                ////    want scape? wait!!                                     // 遇到AWAIT DELAY,返回这里，执行LINE 2,返回上一层TEST(),执行WRITELINE(),等在那里
                ////    EXIT GetAnswerToLifeParallel  =2=ThreadID=4            // 异步线程启动，反向操作，到LINE2
                ////    EXIT PrintAnswerToLifeParallel=2=ThreadID=4
                ////    42
                ////    IN PrintAnswerToLifeParallel  =3=ThreadID=4
                ////    IN GetAnswerToLifeParallel    =4=ThreadID=4
                ////    EXIT GetAnswerToLifeParallel  =4=ThreadID=4
                ////    EXIT PrintAnswerToLifeParallel=4=ThreadID=4
                ////    42
                ////    Done
                ////     time to go 
                ////    Hello World!
                //// */
            }

            WriteLine("Done");
       }
        //理解，看来并不是遇到AWAIT就返回CALLING THREAD
        //从测试结果看，这个线程遇到AWAIT，也还是要继续向下走，直到走到最后的一个AWAIT
        //因为很可能很多的AWAIT其实只是层层嵌套，实际上需要等的只是一个东西，结果套来套去，每个方法都有一个AWAIT
        static async Task PrintAnswerToLifeParallel()
        {
            _counter++;
            WriteLine($"IN PrintAnswerToLifeParallel={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            int answer = await GetAnswerToLifeParallel(); // 当执行到这里时，进不进入GetAnswerToLifeParallel()？ 测试结果是进入的
            WriteLine($"EXIT PrintAnswerToLifeParallel={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine(answer);
       }

        static async Task<int> GetAnswerToLifeParallel()
       {
           _counter++;
            WriteLine($"IN GetAnswerToLifeParallel={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(5000);
           int answer = 21 * 2;
           WriteLine($"EXIT GetAnswerToLifeParallel={_counter}=ThreadID={Thread.CurrentThread.ManagedThreadId}");
           return answer;
       }
    }
}

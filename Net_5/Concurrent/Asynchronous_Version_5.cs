using System;
using System.Collections.Generic;
using System.Linq;
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
                GoParallel();
                Console.WriteLine("want scape? wait!!");
                ReadKey();
                Console.WriteLine("time to go");
            }

            #endregion



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
           //todo: 这是并行，两个结果同时出现
           {
                //var task1 = PrintAnswerToLifeParallel();
                //var task2 = PrintAnswerToLifeParallel();
                //await task1;      //TASK1是一个结果，是个TASK,
                //await task2;      //TASK2是一个结果，是个TASK,
                ///*
                //IN PrintAnswerToLifeParallel=1=ThreadID=1    //主线程进入PrintAnswerToLife
                //IN GetAnswerToLifeParallel  =2=ThreadID=1    //主线程进入GetAnswerToLife, 遇到DELAY的AWAIT, 返回这里，执行第二行 var task2
                //IN PrintAnswerToLifeParallel=3=ThreadID=1    //主线程进入PrintAnswerToLife
                //IN GetAnswerToLifeParallel  =4=ThreadID=1    //主线程进入GetAnswerToLife
                //want scape? wait!!                           //又执行到了DELAY 的AWAIT，返回这里，执行 AWAIT TASK1,又返回到TEST()中，执行WRITELINE()后，等候在那里
                //EXIT GetAnswerToLifeParallel  =4=ThreadID=4  //AWAIT 结束后， 异步线程启动，开始执行CONTINUATION
                //EXIT GetAnswerToLifeParallel  =4=ThreadID=5  //这里可以看出有两个线程在反向执行CONTINUATION,4和5
                //EXIT PrintAnswerToLifeParallel=4=ThreadID=4
                //EXIT PrintAnswerToLifeParallel=4=ThreadID=5
                //42
                //42
                //Done
                // time to go
                //Hello World!
                //*/
            }
            //TODO: 这是异步，一前一后出现
            {
                //await PrintAnswerToLifeParallel();             // 注意写法，当写成这样时，await task1; 直接返回上一层
                //await PrintAnswerToLifeParallel();             // 写成这样时，虽然有AWAIT,却仍然进入方法，为什么？
                ///*
                //    IN PrintAnswerToLifeParallel=1=ThreadID=1    // 主线程依次进入两个METHOD
                //    IN GetAnswerToLifeParallel  =2=ThreadID=1
                //    want scape? wait!!                           // 遇到最后一个AWAIT, 即TASK.DELAY处，开始返回到TASK()主线程                 
                //    EXIT GetAnswerToLifeParallel=2=ThreadID=4    // DELAY结束，启动新线程 4，执行CONTINUATION
                //    EXIT PrintAnswerToLifeParallel=2=ThreadID=4  // 从GetAnswerToLife返回PrintAnswerToLife
                //    42
                //    IN PrintAnswerToLifeParallel=3=ThreadID=4
                //    IN GetAnswerToLifeParallel  =4=ThreadID=4
                //    EXIT GetAnswerToLifeParallel=4=ThreadID=4
                //    EXIT PrintAnswerToLifeParallel=4=ThreadID=4
                //    42
                //    Done
                //     time to go
                //    Hello World!
                // */
            }
            //TODO: 
            {
                //PrintAnswerToLifeParallel();
                //PrintAnswerToLifeParallel();

                /*  顺序如下
                    IN PrintAnswerToLifeParallel=1=ThreadID=1
                    IN GetAnswerToLifeParallel  =2=ThreadID=1
                    IN PrintAnswerToLifeParallel=3=ThreadID=1
                    IN GetAnswerToLifeParallel  =4=ThreadID=1
                    Done
                    want scape? wait!!
                    EXIT GetAnswerToLifeParallel=4=ThreadID=4
                    EXIT GetAnswerToLifeParallel=4=ThreadID=5
                    EXIT PrintAnswerToLifeParallel=4=ThreadID=4
                    EXIT PrintAnswerToLifeParallel=4=ThreadID=5
                    42
                    42
                 */
           }
            //TODO: 
            {
                var task1=PrintAnswerToLifeParallel();  //1
                await task1;                            //2   //注意，这个LINE2的CONTINUATION包含了下面子句
                var task2=PrintAnswerToLifeParallel();  //3
                await task2;                            //4    

                /*
                    IN PrintAnswerToLifeParallel  =1=ThreadID=1            // 主线程进入PrintAnswerToLife
                    IN GetAnswerToLifeParallel    =2=ThreadID=1            // 主线程进入GetAnswerToLife
                    want scape? wait!!                                     // 遇到AWAIT DELAY,返回这里，执行LINE 2,返回上一层TEST(),执行WRITELINE(),等在那里
                    EXIT GetAnswerToLifeParallel  =2=ThreadID=4            // 异步线程启动，反向操作，到LINE2
                    EXIT PrintAnswerToLifeParallel=2=ThreadID=4
                    42
                    IN PrintAnswerToLifeParallel  =3=ThreadID=4
                    IN GetAnswerToLifeParallel    =4=ThreadID=4
                    EXIT GetAnswerToLifeParallel  =4=ThreadID=4
                    EXIT PrintAnswerToLifeParallel=4=ThreadID=4
                    42
                    Done
                     time to go
                    Hello World!
                 */
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

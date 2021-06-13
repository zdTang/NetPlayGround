using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Net_5.Concurrent
{
    //public delegate void People(int x); 
    class Multithreading
    {
        public static void Test()
        {
            #region Creating a thread
            //任何一个程序在开始时，都只有一个THREAD
            //这里别开一个概念，单CORE与多CORE(多PROCESSOR),PARALLEL是指在多CORE或多PROCESSOR上，是真正的并行
            //Two scenarios:
            //Single core == Each thread can work within its CPU time-slice
            //Multi-core, multi-processor, two threads works genuinely in parallel

            {
                //Thread t = new Thread(WriteY);                  // Kick off a new thread
                //t.Start();                                      // running WriteY()===This is the new thread
                //                                                //这个START并不一定是马上就执行，
                //                                                //测试时，虽然它START了，每次主线程反而走在前面，先打印X
                //                                                //t.Join();   /如果有JOIN,则这个T先走，T结束了，主线程才走
                //                                                // Simultaneously, do something on the main thread.
                //for (int i = 0; i < 100000; i++) Console.Write("x");

                //void WriteY()
                //{
                //    for (int i = 0; i < 1000; i++) Console.Write("y");
                //    Console.WriteLine("WriteY");
                //}
                //Console.WriteLine("main is over");
            }

            #endregion

            #region Join and Sleep
            //用谁的JOIN，就表示MAIN THREAD要等候它结束，才继续走
            {
                Thread t = new Thread(Go);
                t.Start();
                t.Join();                          //如果没有JOIN,则主THREAD会先走，有JOIN,它会执行完，再融入主线程
                                                   //JOIN不设时间，就是无限的等，一直等它结束，主THREAD才动
                                                   //t.Join(TimeSpan.FromTicks(10)); //设置时间界限，这个时间太短，子线程没跑完，主线程不等了，就启动了
                                                   //Console.WriteLine(t.Join(TimeSpan.FromTicks(10)));
                                                   //Thread.Sleep(1000);             //由于主线程休息一会儿，这次子线程跑在前面了
                Console.WriteLine("Thread t has ended!");//主线程要等T执行结束，才执行这一句

                void Go()
                {
                    for (int i = 0; i < 1000; i++) Console.Write("y");
                    Console.WriteLine("Go has ended!");
                }
            }

            #endregion

            #region Local vs Shared State

            {
                ////The CLR assigns each thread its own memory stack so that local variables are kept separate
                ////separate copy of the cycles variable is created on each thread’s memory stack,
                ////and so the output is, predictably, 10 question marks.
                ////int cycles = 0;
                //new Thread(Go).Start();      // Call Go() on a new thread
                //Go();                        // Call Go() on the main thread

                //void Go()
                //{
                //    // Declare and use a local variable - 'cycles'
                //    for (int cycles = 0; cycles < 5; cycles++) Console.Write('?');
                //    //for (; cycles < 5; cycles++) Console.Write('?');//如果指导I放在外面，则两个THREAD共享一个I,会冲突
                //}
            }

            #endregion

            #region Share States with Fields -- no safe

            {
                ////调用同一个OBJECT中的同一个METHOD
                ////这个METHOD引用同一个FIELD
                //var tt = new ThreadTest();
                //new Thread(tt.Go).Start();
                //tt.Go();
            }

            #endregion

            #region Share State -- Unsafe

            {
                ////只打印一次
                //bool _done = false;

                //new Thread(Go).Start();
                //Go();

                //void Go()
                //{
                //    if (!_done) { _done = true; Console.WriteLine("Done"); }
                //}
            }

            #endregion

            #region Share State -- Closure -- no safe

            {
                ////Public delegate
                //bool done = false;
                ////这里，ACTION返回一个 CLOSURE
                ////但这个CLOSURE都会访问同一个外面VARIABLE
                ////public delegate void ThreadStart(); 这个THREADSTART就是个普通的DELEGATE类型，VOID,PARAMATERLESS
                ////理解这种定义方式，委托类型 直接跟一个METHOD
                //ThreadStart action = () =>
                //{
                //    if (!done) { done = true; Console.WriteLine("Done"); }
                //};
                //new Thread(action).Start();
                //action();
            }

            #endregion

            #region Share State -- With Statics --no safe

            {
                ////由于这个STATIC 方法改变的是STATIC FIELD
                ////这与实例方法不同，实例只改变实例自己的
                ////而STATIC改变的是整个TYPE的
                //NewThreadTest.MainOne();
            }

            #endregion

            #region Share State - have lock

            {
                ////这个有LOCK
                //ThreadSafe.MainOne();
            }

            #endregion

            #region Lambda 传参
            //向THREAD传递参数的方法
            {
                ////THREAD接收ThreadStart作为参数
                ////ThreadStart是一个DELEGATE, 无参，无返回值
                ////这个，这个THREAD T就可以直接把参数传进去了
                //Thread t = new Thread(() => Print("Hello from t!"));//这样满足无参无返回值
                //t.Start();
                ////Thread w = new Thread(Print("Hello from t!"));//不满足THREAD参数要求

                //void Print(string message) => Console.WriteLine(message);
                
                ////可以传多个参数，
                //new Thread(() =>
                //{
                //    Console.WriteLine("I'm running on another thread!");
                //    Console.WriteLine("This is so easy!");
                //}).Start();

                ////用START传递
                //Thread w = new Thread(PrintOne);
                //w.Start("Hello from t!"); // Start 接收OBJECT，不太方便
                ///*
                //    public void Start(object? parameter)
                //    {
                //      if (this._delegate is ThreadStart)
                //        throw new InvalidOperationException(SR.InvalidOperation_ThreadWrongThreadStart);
                //      this._threadStartArg = parameter;
                //      this.Start(); //又去调那个无参的CONSTRUCTOR了
                //    }
                // */

                //void PrintOne(object messageObj)
                //{
                //    string message = (string)messageObj; // We need to cast here
                //    Console.WriteLine(message);
                //}


            }

            #endregion

            #region Lambda and captured variables
            //However, you must be careful about accidentally modifying cap‐
            //tured variables after starting the thread. For instance, consider the following
            //The problem is that the i variable refers to the same memory location throughout
            //the loop’s lifetime. Therefore, each thread calls Console.Write on a variable whose
            //value can change as it is running!
            
            // No safe
            {
                //for (int i = 0; i < 10; i++)//每个THREAD就找自己的，但THREAD来的有早晚
                //    new Thread(() => Console.Write(i)).Start();
            }//output:46335378910

            // 依旧不safe，虽然这次每个THREAD有自己的值，THREAD并不是按顺序启动的，因而打印顺序仍不可定
            {
                //for (int i = 0; i < 10; i++)
                //{
                //    int temp = i; //每个THREAD就找自己的，无论来的早晚，但元法保护数字按原来的顺序打印
                //    new Thread(() => Console.Write(temp)).Start();
                //}
            }



            #endregion

            #region Exception Handling
            //无效TRY-CATCH
            {
                //try
                //{
                //    new Thread(Go).Start();//这个新线程不会向下走，而是有自己的路
                //    //MAIN THREAD才会沿着这个PATH向下执行
                //}
                //catch (Exception ex)
                //{
                //    // We'll never get here!
                //    Console.WriteLine("Exception!");
                //}

                //static void Go() { throw null; }   // Throws a NullReferenceException
            }
            // 有效TRY-CATCH
            {
                //new Thread(() => { Go();}).Start();

                //void Go([CallerMemberName] string memberName = "", // Must be an optional parameter
                //    [CallerFilePath] string sourceFilePath = "", // Must be an optional parameter
                //    [CallerLineNumber] int sourceLineNumber = 0 // Must be an optional parameter
                //                                                )//把TRY-CATCH放在方法内部--需要认真体会
                //{
                //    try
                //    {
                //        throw null;    // The NullReferenceException will get caught below
                //    }
                //    catch (Exception ex)
                //    {
                //        //Typically log the exception, and/or signal another thread
                //        // that we've come unstuck
                //        // 这里出现EXCEPTION有三个问题需解决：一是LOG,二是显示DIALOG，让USER重新提交，三是重启系统 
                //        //ex.Dump("Caught!");

                //        //这里DEMO如何LOG情况到TRACE.TXT中，但这不是一个好的方式，不方便
                //        //我可以不必用ATTRIBUTE来获取这些数据。因为EX中有这些数据， 只需从EX中拿就可以
                //        Trace.WriteLine(
                //            $"{nameof(Go)} called from {memberName}{Environment.NewLine}" +
                //            $"  File: {sourceFilePath}{Environment.NewLine}" +
                //            $"  Line: {sourceLineNumber}{Environment.NewLine}"+
                //            $"  ExceptionInfo:{ex.Message}{Environment.NewLine}");
                //        //这些需要重写一下，如何自动CLOSE,因为这里没有用USING,因而最好TRACE的FINALIZER中有DISPOSE
                //        Trace.Flush();
                //        Trace.Listeners[0].Close();//关掉文件//不能在MAIN中关，因为主线程跑的快，关了这里就写不上去了
                //        //fileListener.Close()

                //    }
                //}
            }

            #endregion

            #region Basic Signal
            {
                //用于THREAD间互相通信 
                var signal = new ManualResetEvent(false);

                new Thread(() =>
                {
                    Console.WriteLine("Waiting for signal...");
                    signal.WaitOne();   //一直等候SIGNAL
                    signal.Dispose();   //RELEASE ALL RESOURCES
                    Console.WriteLine("Got signal!");
                }).Start();
                
                Console.WriteLine("main thread!");
                Thread.Sleep(6000);
                signal.Set();        // “Open” the signal
            }
            #endregion





        }
    }
    class ThreadTest
    {
        bool _done;

        public void Go()
        {
            if (!_done) { _done = true; Console.WriteLine("Done"); }
        }
    }
    class NewThreadTest
    {
        static bool _done;          // Static fields are shared between all threads
        // in the same application domain.
        public static void MainOne()
        {
            new Thread(Go).Start();  //这两个THREAD都会去访问这个_DONE
            Go();
        }

        static void Go()
        {
            if (!_done) {  Console.WriteLine("Done"); _done = true; }
        }
    }
    class ThreadSafe
    {
        static bool _done;
        static readonly object _locker = new object();

        public static void MainOne()
        {
            new Thread(Go).Start();
            Go();
        }

        static void Go()
        {
            lock (_locker)
            {
                if (!_done) { Console.WriteLine("Done"); _done = true; }
            }
        }
    }

    

}

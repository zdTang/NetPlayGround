using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Net_5.Concurrent
{
    class AsynchronousPatterns
    {
        public static async void Test()
        {
            #region Cancellation

            // Cancellation
            {   
                ////理解，一个TASK，可以AWAIT,也可以不AWAIT
                ////下面这个TASK可以DEMO两种方式
                //var token = new CancellationToken();
                //WriteLine($"Before Test() Delay == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                ////这里设置CONTINUATION的参数，DELAY之后的操作都是CONTINUATION
                //Task.Delay(5000).ContinueWith(ant => token.Cancel());       // 同步建一个TASK,5秒中后将会发生
                ////await Task.Delay(5000).ContinueWith(ant => token.Cancel());   // 异步执行，主THREAD先返回，5秒后异步发动
                //WriteLine($"After Test() Delay == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //await Foo(token);
            }
            // Using the real Cancellation
            {
                //var _cancelSource = new CancellationTokenSource();                   // 系统自带的
                //Task.Delay(5000).ContinueWith(ant => _cancelSource.Cancel());   // Tell it to cancel in two seconds.
                //await FooTwo(_cancelSource.Token);
            }
            // Using the real Cancellation -- improved Version
            // 可以直接把DELAY的时间一起初始化
            {
                //var cancelSource = new CancellationTokenSource(5000); // This tells it to cancel in 5 seconds
                //await Foo(cancelSource.Token);
            }

            #endregion

            #region Process Reporting

            {
                // With Delegate
                {
                    ////仔细看流程，AWAIT遇到AWAITABLE TASK即通回
                    //Action<int> progress = i => Console.WriteLine(i + " %");
                    //await FooFour(progress);
                }
                // with IProgress(系统自带的）
                {
                    //var progress = new Progress<int>(i => Console.WriteLine(i + " %"));
                    //await FooFive(progress);
                }
            }

            #endregion

            #region WhenAny
            //这里的不是同，这个主THREAD一连进了三个DELAY,启动了了三个AWAITABLE TASK才退出，相当于是平行的
            //如果主进程只进到一个DELAY,然后退出，则三个DELAY就不是平行的，而是一个接一个执行的
            {
                //WhenAny

                {
                    //Task<int> winningTask = await Task.WhenAny(Delay1(), Delay2(), Delay3());
                    //Console.WriteLine("Done");
                    //Console.WriteLine(winningTask.Result);   // 情形1

                    //情形1==结果：
                    /*
                     *  IN  Main() ===ThreadID = 1   //这里的不是同，这个主THREAD一连进了三个DELAY,启动了了三个AWAITABLE TASK才退出
                        into Delay1 == ThreadID : 1
                        into Delay2 == ThreadID : 1
                        into Delay3== ThreadID : 1
                        BACK TO Main() ===ThreadID = 1
                        exit Delay1 == ThreadID : 4 //最快的异步有结果了
                        Done
                        1
                        exit Delay2 == ThreadID : 5
                        exit Delay3 == ThreadID : 5
                         Out  Main() ===ThreadID = 1
                        Hello World!
                     */
                }
                // WhenAny -- AWAIT WINING TASK  // 注意 todo: 为什么 await winningTask 可以直接取出结果
                //int result = await winningTask;        // AWAIT一个TASK 可以直接取出结果
                {
                    //Task<int> winningTask = await Task.WhenAny(Delay1(), Delay2(), Delay3());
                    //Console.WriteLine("Done");
                    ////int result = await winningTask;        // AWAIT一个TASK 可以直接取出结果
                    //Console.WriteLine(await winningTask);   //  情形2： 结果与情形1相同，但加一个AWAIT更好，见NUTSHELL  
                    ////AWAIT WININGTASK另一种情形，这里加上AWAIT
                }
                // WhenAny -- In one STEP
                // 两次AWAIT就可以直接取出结果，参考上面
                {
                    
                    //int answer = await await Task.WhenAny(Delay1(), Delay2(), Delay3());  //两个AWAIT, 直接取出结果
                    //answer.Dump();
                }
                // WhenAny -- time out
                {
                    //WriteLine($"before task == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //Task<string> task = SomeAsyncFunc();
                    //WriteLine($"before winner == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    ////Task winner = await (Task.WhenAny(task, Task.Delay(5000))); //注意，这里与下行，改变时间，获得不同的结果
                    //Task winner = await (Task.WhenAny(task, Task.Delay(11000)));  // LINE_111
                    //WriteLine($"before if == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //if (winner != task) throw new TimeoutException();
                    //string result = await task;   // Unwrap result/re-throw
                    //WriteLine($"last == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    ///*  流程，用await (Task.WhenAny(task, Task.Delay(11000)));这一行
                    //    注意主THEAD的运动轨迹
                    //    IN  Main() ===ThreadID = 1
                    //    before task == ThreadID : 1
                    //    into SomeAsyncFunc == ThreadID : 1  //主线程进入方法中，遇到AWAIT，从方法中跳出
                    //    before winner == ThreadID : 1       //主线程继续向下，
                    //    BACK TO Main() ===ThreadID = 1      //又遇到AWAIT,从TEST()返回MAIN()
                    //    exit SomeAsyncFunc == ThreadID : 4  //异步线程启动 LINE_111
                    //    before if == ThreadID : 4
                    //    last == ThreadID : 4
                    //     Out  Main() ===ThreadID = 1
                    //    Hello World!
                    // */
                }
                // WhenAll
                {
                    //WriteLine($"into Test == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //await Task.WhenAll(Delay1(), Delay2(), Delay3());
                    //WriteLine($"Under WhenAll == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //"Done".Dump();
                    //WriteLine($"Leave Test == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    ////流程
                    ///*
                    //    IN  Main() ===ThreadID = 1
                    //    into Test == ThreadID : 1
                    //    into Delay1 == ThreadID : 1   //同时启动了三个TASK
                    //    into Delay2 == ThreadID : 1
                    //    into Delay3== ThreadID : 1
                    //    BACK TO Main() ===ThreadID = 1 //主线程退出
                    //    exit Delay1 == ThreadID : 4
                    //    exit Delay2 == ThreadID : 4
                    //    exit Delay3 == ThreadID : 4    //注意，这里三个TASK都出来了，WhenAll才算结束
                    //    Under WhenAll == ThreadID : 4  //异步线程继续执行CONTINUATION
                    //    D
                    //    o
                    //    n
                    //    e
                    //    Leave Test == ThreadID : 4
                    //     Out  Main() ===ThreadID = 1
                    //    Hello World!
                    // */
                }
                // WhenAll - exception
                {
                    //Task task1 = Task.Run(() =>
                    //{
                    //    WriteLine($"into Task1 == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //    throw null;
                    //});

                    //Task task2 = Task.Run(() =>
                    //{
                    //    WriteLine($"into Task2 == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //    throw null;
                    //});

                    //WriteLine($"Before WhenAll == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //Task all = Task.WhenAll(task1, task2);
                    //WriteLine($"After WhenAll == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //try { await all; }
                    //catch
                    //{
                    //    WriteLine($"Catch == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //    Console.WriteLine(all.Exception.InnerExceptions.Count);   // 2 
                    //}

                    ////运行结果
                    ///*
                    //    IN  Main() ===ThreadID = 1
                    //    Before WhenAll == ThreadID : 1
                    //    After WhenAll == ThreadID : 1  
                    //    into Task2 == ThreadID : 5        // 此时才进入TASK1, TASK2中，之前的定义根据没有运行？？
                    //    into Task1 == ThreadID : 4
                    //    BACK TO Main() ===ThreadID = 1
                    //    Catch == ThreadID : 5
                    //    2
                    //     Out  Main() ===ThreadID = 1
                    //    Hello World!
                    // */
                }

                //=============================

                async Task<string> SomeAsyncFunc()
                {
                    WriteLine($"into SomeAsyncFunc == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(10000);
                    WriteLine($"exit SomeAsyncFunc == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    return "foo";
                }

                async Task<int> Delay1()
                {
                    WriteLine($"into Delay1 == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(1000);
                    WriteLine($"exit Delay1 == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    return 1;
                }

                async Task<int> Delay2()
                {
                    WriteLine($"into Delay2 == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(2000);
                    WriteLine($"exit Delay2 == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    return 2;
                }

                async Task<int> Delay3()
                {
                    WriteLine($"into Delay3== ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    await Task.Delay(3000);
                    WriteLine($"exit Delay3 == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    return 3;
                }

            }

            #endregion

        }





    static Task FooFive(IProgress<int> onProgressPercentChanged)
        {
            WriteLine($"into FooFive == ThreadID : {Thread.CurrentThread.ManagedThreadId}"); // 主线程到此，遇到AWAIT TASK, 返回MAIN
            return Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    WriteLine($"in the for lOOP == ThreadID : {Thread.CurrentThread.ManagedThreadId}==I: {i}");
                    if (i % 10 == 0) onProgressPercentChanged.Report(i / 10);
                    // Do something compute-bound...
                }
            });
        }
        static Task FooFour(Action<int> onProgressPercentChanged)
        {
            WriteLine($"into FooFour == ThreadID : {Thread.CurrentThread.ManagedThreadId}");   // 主线程到此，遇到AWAIT TASK, 返回MAIN
            return Task.Run(() =>                                                              //也就是说，AWAIT是遇到AWAITABLE的TASK就返回？
            {   //主线程回去了，这里的所有工作都由异步线程，用传来的DELEGATE完成
                for (int i = 0; i < 1000; i++)
                {
                    WriteLine($"in the for lOOP == ThreadID : {Thread.CurrentThread.ManagedThreadId}==I: {i}");
                    if (i % 10 == 0) onProgressPercentChanged(i / 10);
                    // Do something compute-bound...
                }
            });
        }

        async Task FooThree(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
                await Task.Delay(1000, cancellationToken);  // Cancellation tokens propagate nicely
            }
        }
        static async Task Foo(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 10; i++)
            {
                WriteLine($"into Foo == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine(i);
                await Task.Delay(1000);
                WriteLine($"After Foo() Delay==ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                cancellationToken.ThrowIfCancellationRequested();   //把判断语名放在这里
            }
        }

        static async Task FooTwo(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 10; i++)
            {
                WriteLine($"into FooTwo == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine(i);
                await Task.Delay(1000);
                WriteLine($"After FooTwo() Delay==ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }

    // This is a simplified version of the CancellationToken type in System.Threading:
    // this one is fake one, 系统有一个跟它同名的。测试完1之后，要COMMENT这个，以免影响系统自己的TOKEN
    // 这个简化版可以很好理解这个TOKEN的工作原理
    //class CancellationToken
    //{
    //    public bool IsCancellationRequested { get; private set; }
    //    public void Cancel() { IsCancellationRequested = true; }
    //    public void ThrowIfCancellationRequested()
    //    {
    //        if (IsCancellationRequested) throw new OperationCanceledException();
    //    }
    //}


}

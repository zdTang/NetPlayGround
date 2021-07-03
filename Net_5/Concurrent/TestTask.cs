#define TESTMODE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5.Concurrent
{
    class TestTask
    {
        public static async void Test()
        {
            // task可以有BACKING THREAD,也可以没有。如果有的话，肯定是多一个线程，
            // TASK的参数都由FUNC,ACTION表示，好好理解意义
            // 感觉TASK传参也是用LAMBDA最方便  --- 
            // 如果一个方法的回值是返 TASK<T>的，那么应该是异步的，即主线程不会等结果
            ////The Task class provides a powerful abstraction for threading with which you can easily distinguish between the degree of parallelization in an application (the tasks) and the units of parallelization (the threads). On a single-processor computer, these items are usually the same. However, on a computer with multiple processors or with a multicore processor, they are different. (C# SBS)这段说明了TASK 与 THREAD的区别

            #region Start Task

            {
                ////最简单的方法
                //Task.Run(PrintOne);
                //// Task.Run(() => { Thread.Sleep(2000); Console.WriteLine("Foo"); }); //TASK是BACKGROUND，如果MAIN THREAD走完，它就被忽视
                ////Console.WriteLine("main thread");
                ////Console.ReadKey();

                //// 注意，这个<Result>表示TASK中的方法的返回值，而不是TASK的返回值，TASK是不可以直接返回的，必须包装成METHOD的形式
                //// 感觉还是用LAMBDA表达式传值最方便
                //// TASK一般要一个ACTION,或一个无参的FUNC
                ///* 如下，可见TASK中的方法一般是无参的
                //    public Task(Func<TResult> function);
                //    public Task(Func<TResult> function, CancellationToken cancellationToken);
                //    public Task(Func<TResult> function, TaskCreationOptions creationOptions);
                //    public Task(Func<object?, TResult> function, object? state,TaskCreationOptions creationOptions);这个FUNC可以带一个可以NULL的OBJECT,必须是OBJECT
                // */

                // NEW TASK()  与 TASK.RUN()的不同
                {
                    //Task task = new Task(PrintOne);
                    //task.Start();
                    //Task.Run(PrintOne);
                    
                }
                
                //几种取值的方法
                {
                    //Task<int> task = Task.Run(() => { return 10; });
                    //var result = task.Result;
                    //(await task).Dump("await task");           //WORK      
                    //result.Dump("result");                     //WORK    
                    //task.Result.Dump("Result");                //WORK
                }
                //  TASK的建立方法
                //  https://stackoverflow.com/questions/29693362/regarding-usage-of-task-start-task-run-and-task-factory-startnew
                //Don't ever create a Task and call Start() unless you find an extremely good reason to do so. It should only be used if you have some part that needs to create tasks but not schedule them and another part that schedules without creating. That's almost never an appropriate solution and could be dangerous.
                // 不要使用 NEW+START的方法
                // 用 TASK.RUN() 以及TASK.FACTORY.STARTNEW()两种
                {
                   //// Task<string> taskTwo = new Task<string>(Demo);
                   //// Task<string> taskThree = new Task<string>(DemoTwo,
                   ////     new CancellationToken(false)); //用NEW实例的，如果带参数，则需要加一个CANCELLATIONTOKEN
                   // //注意， 这是异步调用，主线程CALL完之后，不等结果，直接返回，向下进行
                   // var taskFour = Task.Factory.StartNew(PrintOne);
                   // var taskFive = Task.Factory.StartNew((object str) => str.ToString(), true);
                   // //注意， 这是异步调用，主线程CALL完之后，不等结果，直接返回，向下进行
                   // Task<string> taskSix = Task.Run(() => "mike");
                   // taskSix.Result.Dump("taskSix");
                   // //(await taskSix).Dump("await taskSix");

                   // string Demo()
                   // {
                   //     return "Mike";
                   // }

                   // string DemoTwo(object obj)
                   // {
                   //     return obj.ToString();
                   // }
                }
                
            }
            #endregion

            #region Wait

            {
                //Task task = Task.Run(() =>
                //{
                //    Console.WriteLine("Task started");          //2.这句执行
                //    Thread.Sleep(2000);
                //    Console.WriteLine("Foo");                   //3.
                //});
                //Console.WriteLine(task.IsCompleted);  // False  1. 这句先执行了
                ////task.Wait();  // Blocks until task is complete  //类似于THREAD.JOIN(),主线程BLOCK在这里，等TASK结束
                //              //这里很易引起歧义，好象是TASK在等，而实际上是在等TASK 
                //              //如果没这一句，这个FOO还没打出来，就退出了，主线程不等 
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
                //Console.WriteLine("I am leaving too!");
            }

                #endregion

            #region Returning Values

            {
                //Task<int> primeNumberTask = Task.Run(() =>
                //    {
                //        Console.WriteLine($"in primeNumberTask = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //        return Enumerable.Range(2, 3000000)
                //            .Count(n => Enumerable
                //                .Range(2, (int) Math
                //                    .Sqrt(n) - 1)
                //                .All(i => n % i > 0));
                //    }
                //);

                //Console.WriteLine("Task running...");
                ////Console.WriteLine("The answer is " + primeNumberTask.Result); //主线程等在这里拿RESULT
                //Console.WriteLine($"Before task = ThreadID : {Thread.CurrentThread.ManagedThreadId}");

                //Task<int> task = Task.Run(() => 
                //{
                //    Console.WriteLine("Foo");
                //    Console.WriteLine($"in task = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    return 3;
                //});

                //Console.WriteLine($"After task = ThreadID : {Thread.CurrentThread.ManagedThreadId}");

                //int result = task.Result;      // Blocks if not already finished 主线程等在这里拿RESULT
                //Console.WriteLine(result);    // 3
                //Console.WriteLine("The answer is " + primeNumberTask.Result); //主线程等在这里拿RESULT


            }

            #endregion

            #region Exception

                {
                    //// Start a Task that throws a NullReferenceException:
                    ////exception is automatically rethrown to whoever calls Wait()—or accesses the
                    ////Result property of a Task<TResult>注意，谁在WAIT,谁在等着拿结果，就把EXCEPTION丢给谁
                    //Task task = Task.Run(() => { throw null; });
                    //try
                    //{
                    //    task.Wait();  //主线程CALL的这个
                    //}
                    //catch (AggregateException aex)  //AggregateException这个用来包装TASK发生的EXCEPTION
                    //{
                    //    if (aex.InnerException is NullReferenceException)
                    //        Console.WriteLine("Null!");
                    //    else
                    //        throw;
                    //}
                }

            #endregion

            #region Continuations-GetAwaiter

            //todo:  DOESN'T WORK  ==  已修改，加上主线程等候
            //这个主线程必须等候，如果主线程先退出了，程序就结束了
            //我理解这个POINT在于演示TASK一个接一个运行
            //这是C#的异步回调模式？ C# asynchronous Function mode


            //will (in general) execute on the same thread as the antecedent, avoid‐ing unnecessary overhead.
            //这里，这个AWAITER的设置方式，决定了这个CONTINUTAION是在哪个THREAD上执行
            //下面两种方式
            /*If a synchronization context is present, OnCompleted automatically captures it and posts the continuation to that context.
            This is very useful in rich client applications because it bounces the continuation back to the UI thread
            */
            //synchronization context 是个CLASS
            //https://docs.microsoft.com/en-us/dotnet/api/system.threading.synchronizationcontext?view=net-5.0
            //https://docs.microsoft.com/en-us/archive/msdn-magazine/2011/february/msdn-magazine-parallel-computing-it-s-all-about-the-synchronizationcontext
            //If no synchronization context is present—or you use ConfigureAwait(false)—the continuation will (in general) execute on the same thread as the antecedent, avoiding unnecessary overhead.

            //var awaiter = primeNumberTask.GetAwaiter(); //continuation 被丢回 context,不见是还是执行刚才ANTECEDENT的那个THREAD

            {
                //Task<int> primeNumberTask = Task.Run(() =>
                //    {
                //        Console.WriteLine($"in primeNumberTask = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //        return Enumerable.Range(2, 3000000)
                //            .Count(n => Enumerable
                //                .Range(2, (int)Math
                //                    .Sqrt(n) - 1)
                //                .All(i => n % i > 0));
                //    }
                //);
                ////ConfiguredTaskAwaiter 是个结构
                ///*
                // public readonly struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion,INotifyCompletion,IConfiguredTaskAwaiter
                                          
                //  {
                //   public bool IsCompleted { get; }
                //   public void OnCompleted(Action continuation);
                //   public void UnsafeOnCompleted(Action continuation);
                //   public TResult GetResult();
                //  }                         
                                          
                                  
                // */
                //Console.WriteLine($"Come into Test = ThreadID : {Thread.CurrentThread.ManagedThreadId}");

                //var awaiter = primeNumberTask.ConfigureAwait(false).GetAwaiter();   //same thread as the antecedent
                ////主线程绕过，直接向下进行，不在这里等。
                //awaiter.OnCompleted(() =>
                //{
                //    int result = awaiter.GetResult();
                //    Console.WriteLine($"in OnCompleted = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    Console.WriteLine($"=={result}==");       // Writes result
                //});
                //Console.WriteLine($"Before result = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine(primeNumberTask.Result);  // MAIN THREAD WILL WAIT HERE
                //Console.WriteLine($"Before bye = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine("bye");

            }
            // 了解 SynchronizationContext的结构
            {
                SynchronizationContext x = new SynchronizationContext();
                /*
                   public virtual void Send(SendOrPostCallback d, object? state) => d(state);

                   public virtual void Post(SendOrPostCallback d, object? state) => ThreadPool.QueueUserWorkItem <(SendOrPostCallback, object)>((Action<(SendOrPostCallback, object)>) (s => s.d(s.state)), (d, state), false);
                 */

            }
            //不用AWAITER的方案，而直接用WAIT()的方案
            {
                //Task<int> primeNumberTask = Task.Run(() =>
                //    {
                //        Console.WriteLine($"in primeNumberTask = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //        return Enumerable.Range(2, 3000000)
                //            .Count(n => Enumerable
                //                .Range(2, (int)Math
                //                    .Sqrt(n) - 1)
                //                .All(i => n % i > 0));
                //    }
                //);

                //primeNumberTask.Wait();            // MAIN THREAD WILL WAIT HERE


                //int result = primeNumberTask.Result;
                //Console.WriteLine(result);       // Writes result


                //Console.WriteLine("bye");
            }

            #endregion

            #region Continuations - ContinueWith

            //todo:  DOESN'T WORK  ==修改，加上主线程等候
            //这个主线程必须等候，如果主线程先退出了，程序就结束了
            //我理解这个POINT在于演示TASK一个接一个运行
            {
                //// (See Chapter 22 for more on using ContinueWith.)

                //Task<int> primeNumberTask = Task.Run(
                //    () => Enumerable.Range(2, 3000000)
                //            .Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1)
                //            .All(i => n % i > 0)));
                ////primeNumberTask.Wait();
                //primeNumberTask.ContinueWith(antecedent =>
                //{
                //    int result = antecedent.Result;
                //    Console.WriteLine($"in ContinueWith = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    Console.WriteLine(result);          // Writes 123
                //});
                //Console.WriteLine("bye,bye");

            }
            // ContinueWith 构成一个链条
            {
                //// (See Chapter 22 for more on using ContinueWith.)

                //Task<int> primeNumberTask = Task.Run(
                //    () => Enumerable.Range(2, 3000000)
                //        .Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1)
                //            .All(i => n % i > 0)));
                ////primeNumberTask.Wait();
                //Task task=primeNumberTask.ContinueWith(antecedent =>
                //{
                //    int result = antecedent.Result;
                //    Console.WriteLine($"in ContinueWith = ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    Console.WriteLine(result);          // Writes 123
                //});
                //task.ContinueWith( mytask => //注意这里，这里的参数不能为空，本例中，它是一个TASK
                //{
                //    Console.WriteLine($"id is {mytask.Id}");
                //    Console.WriteLine("very good");
                //});
                //Console.WriteLine("bye,bye");

            }
            //不用CONTINUATION的方案
            //没有让TASK用CONTINUATION,而是直接把后继动作放在TASK之中
            //结果仍是可行的，但COUPLING是个大问题，不利于动态的加入动作
            {
                //// (See Chapter 22 for more on using ContinueWith.)

                //Task primeNumberTask = Task.Run(
                //    () =>
                //    {
                //        var i = Enumerable.Range(2, 3000000)
                //            .Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
                //        Console.WriteLine(i);//把上面的动作直接放在这里了
                //    });

                //Console.WriteLine("bye,bye");

            }
            //这里的核心在AWAITER, CONTINUE WITH都是由TASK来做，主线程是不粘边的，已经退出
            //把AWAITER,ContinueWith与 await 结合起来理解
            {
                //////https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/chaining-tasks-by-using-continuation-tasks
                //Console.WriteLine("I am coming! hi");
                //// Declare, assign, and start the antecedent task.
                //Task<DayOfWeek> taskA = Task.Run(() => DateTime.Today.DayOfWeek);

                //// Execute the continuation when the antecedent finishes.
                //taskA.ContinueWith(antecedent => Console.WriteLine($"Today is {antecedent.Result}."));
                //Console.WriteLine("I am leaving! bye");

            }

        #endregion

            #region TaskCompletionSource

            //这里，这个TCS仅仅是在TASK完成任务后，取回RESULT??
            //首先，主线程把TCS 放到TASK中
            //TASK在执行后，会把执行的结果，交给TCS
            //主线程可以任意时地方，通过对TSC的引用，取得一个新的TASK，并通过它，拿到上一个TASK的结果
            {

                //var tcs = new TaskCompletionSource<int>();
                ////var myTask=tcs.Task;
                ////new Thread(() => { Thread.Sleep(5000); tcs.SetResult(42); }).Start();
                //Task.Run(() =>
                //{
                //    Console.WriteLine($"In the Task,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    Thread.Sleep(5000);
                //    tcs.SetResult(42);
                //    Console.WriteLine($"Out the Task,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //});
                //Console.WriteLine($"NO 1 : {Thread.CurrentThread.ManagedThreadId}");
                //Task<int> task = tcs.Task;           // Our "slave" task.
                //Console.WriteLine($"NO 2 : {Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine(task.Result);     // 42   取值时，会BLOCK在这里
                //Console.WriteLine($"NO 3 : {Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine(await task);       // 42  为什么可以这样表示 AWAIT 等同于GET AWAITER  然后取值GETVALUE()
                //                                     //     注意  一个TASK<INT> 可用AWAIT直接取出INT值
                //Console.WriteLine(tcs.Task.Result);   // 42 
                //Console.WriteLine($"ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine(await tcs.Task);   // 42 


            }
            //不使用TaskCompletionSource<int>，照样取回值，那它有啥用？
            {
                //// Task的初始化， New task(ACTION), 而TASK.RUN(FUNC)记住这两种
                //Task<int> task = Task.Run(() =>
                //{
                //    Thread.Sleep(5000);
                //    return 42;
                //});

                //Console.WriteLine(task.Result);   // 42 
            }
                //Our own Run Method
                //这里要这样理解
                //我们当然可以直接返回传来的这个方法的结果
            {
                //// RUN需要一个无参，带返回值的DELEGATE
                //Task<int> task = Run(() => { Thread.Sleep(5000); return 42; });

                //task.Result.Dump();

                ////这个函数的意思是，你传来一个METHOD
                ////我启动一种THREAD执行这个METHOD,有了结果我返回
                //Task<TResult> Run<TResult>(Func<TResult> function)
                //{
                //    var tcs = new TaskCompletionSource<TResult>();
                //    new Thread(() =>
                //    {
                //        try { tcs.SetResult(function()); }  //拿到回调函数的结果
                //            catch (Exception ex) { tcs.SetException(ex); }
                //    }).Start();
                //    return tcs.Task;   // 返回TSC的SLAVE
                //}
            }
                //不用TaskCompletionSource<TResult>的方案
                //上下这两个同样的结果，用TaskCompletionSource<TResult的好处在哪里？
            {
                    //// RUN需要一个无参，带返回值的DELEGATE
                    //Task<int> task = Run(() => { Thread.Sleep(5000); return 42; });

                    //task.Result.Dump();

                    //Task<TResult> Run<TResult>(Func<TResult> function)
                    //{

                    //    return Task.Run(() =>
                    //    {
                    //        return function();
                    //    });

                    //}
            } //GetAnswerToLife用TaskCompletionSource实施
                //这里还需要正确理解AWAITER.ONCOMPLETED,主线程见到后不会BLOCKING,会继续向下
            {
                ////The real power of TaskCompletionSource is in creating tasks that
                ////don’t tie up threads. 
                ////Hence, our method returns a task that completes five seconds later, with a
                ////result of 42.
                //Console.WriteLine($" before awaiter ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //var awaiter = GetAnswerToLife().GetAwaiter();
                //Console.WriteLine($" after awaiter ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                ////主线程到此处，没有等候，继续向下执行，这里是异步操作
                ////这里如同开启了一个新的THREAD,主线程并不关心，各干各的
                //awaiter.OnCompleted(() =>
                //{
                //    //由别的线程完成
                //    Console.WriteLine($"in OnCompleted == ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    Console.WriteLine(awaiter.GetResult());
                //}); //AWAITER要在5秒后才能拿到结果

                //Task<int> GetAnswerToLife()
                //{
                //    Console.WriteLine($"in GetAnswerToLife,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    var tcs = new TaskCompletionSource<int>();
                //    // Create a timer that fires once in 5000 ms:
                //    var timer = new System.Timers.Timer(5000) { AutoReset = false };
                //    timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(42); };//ONCE THE TIME IS OVER 5000 绑定方法，一旦时间超过就执行
                //    timer.Start();
                //    Console.WriteLine($"exit GetAnswerToLife,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    return tcs.Task;
                //}
                //Console.WriteLine("try if the main thread go here!");//主线程要等在这里才行
                
            }
                //GetAnswerToLife,不用TaskCompletionSource实施(不成功）
            {
                    ////The real power of TaskCompletionSource is in creating tasks that
                    ////don’t tie up threads. 
                    ////Hence, our method returns a task that completes five seconds later, with a
                    ////result of 42.
                    //var awaiter = GetAnswerToLife().GetAwaiter();
                    //awaiter.OnCompleted(() => Console.WriteLine(awaiter.GetResult())); //AWAITER要在5秒后才能拿到结果

                    //Task<int> GetAnswerToLife()
                    //{

                    //    return Task.Run(
                    //        () =>
                    //        {
                    //            int result = 0;
                    //            var timer = new System.Timers.Timer(5000) {AutoReset = false};
                    //            timer.Elapsed += delegate
                    //            {
                    //                timer.Dispose();
                    //                result = 42;
                    //            }; //ONCE THE TIME IS OVER 5000 绑定方法，一旦时间超过就执行
                    //            timer.Start();
                    //            return result;
                    //        });


                    //}
                    ////主线程要等在这里才行
                    //Thread.Sleep(6000);
            }

            #endregion

            #region Delay with TaskCompletionSource

            //We could make this more useful and turn it into a general-purpose Delay
            //method by parameterizing the delay time and getting rid of the return value.
            //这里模拟的是不是异步的情况？
            //由于返回结果是TASK的，都是异步的，异步编程的三种返回结果：Task<TResult>、Task 和 void
            //（说的不对，同步的也可返回TASK）,如用下面的TIMER那个，一直是主线程在做，
            //由于这个DELAY返回的是TASK,是个将来结果，因而直接返回CALLER
            //这些DELAY自己执行，取得结果后，将结果放在TASK中（借助TaskCompletionSource），我们将来就可以获取了
            //这里的TIMER.ELAPSED 模拟了事件通知机制，即I/O结束后，就把结果写入TASK中

            {
                //for (int i = 0; i < 10000; i++)
                //    Delay(5000).GetAwaiter().OnCompleted(() => Console.WriteLine(42));

                //Task Delay(int milliseconds)
                //{
                //    var tcs = new TaskCompletionSource<object>();
                //    var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
                //    timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(null); };
                //    timer.Start();
                //    return tcs.Task;
                //}
                //Console.WriteLine("Act please!");
                //Thread.Sleep(6000);//主人要在这里等着
            }  
            //只要是TASK都是异步的？==要看TASK是不是有BACKING THREAD,有的TASK没有BACKING THREAD
            //只要是TASK,主线程都会绕行，继续向下==说的不对，下面的TIMER那个就是例外
            //不过，在本例中可以理解为主线程分别启动两个TASK,任由两个TASK去工作了，它自己FIRE完TASK,返回
            //主线程如同一个主调度，它调度TASK,并不会自己进入TASK
            //而如果是一个普通的方法，则主线程会亲自进去
            {
                //#if TESTMODE
                //    Console.WriteLine($"Come into Test,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //#endif
                //Task task1 = Task.Run(() => { Console.WriteLine("MIKE"); });
                //#if TESTMODE
                //    Console.WriteLine($"IN Test,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //#endif
                //Task task2 = Task.Run(() => { Console.WriteLine("TOM"); });


                //Console.WriteLine("bYE");

            }
            // 用上面例子模拟异步回调,P这个DELAY中并没有启动新的THREAD，也可以生成异步
            // 这个TIMER可以模拟IO读写之类的操作，由OS线程去完成，需要一定的时间
            // 当IO结束时，以EVENT或MESSAGE信息通知 TaskCompletionSource<int>
            // TaskCompletionSource<int>将结果打包成TASK<INT>的形式返回
            // 此时，如果在主线程中，我们要取这个结果，就可以拿到了，同时主线程根本没有等它，继续做别的事情了
            // 这里面的坑是TASK<TRESULT>,这里需要加入返回值类型
            // 注意，这个例子中，只有MAIN THREAD参与，没有别的线程
            // TaskCompletionSource可以帮助生成TASK
            // 有了TASK,就可以在某个地方等
            {
                //Console.WriteLine($"before delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //Task<int> delay = Delay(5000);
                //Console.WriteLine($"after delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
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
                ////delay.GetAwaiter();
                ////delay.ContinueWith(() => { });
                //Console.WriteLine("Act please!");

                ////var result = delay.GetAwaiter();         //可以用AWAITER间接拿值
                ////Console.WriteLine(result.GetResult());
                //Console.WriteLine(delay.Result);           //也可以直接拿值，各有利弊，见NETSHELL
                //Console.WriteLine($"READY TO GO,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            }
            // Task Delay
            // The Delay method that we just wrote is sufficiently useful that it’s
            // available as a static method on the Task class
            // 结果：
            /*
                IN  Main() ===ThreadID = 1
                START,  ThreadID : 1
                Ready to go,  ThreadID : 1        //主线程不BLOCKING,直接走开
                BACK TO Main() ===ThreadID = 1
                second Delay,  ThreadID : 5       //ONCOMPLETED是由另外线程完成的
                first Delay,  ThreadID : 4        //ONCOMPLETED是由另外线程完成的
                42
                99
             */

            {
                //Console.WriteLine($"START,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //Task.Delay(5000).GetAwaiter().OnCompleted(() =>
                //{
                //    Console.WriteLine($"first Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    Console.WriteLine(42);
                //});

                //// Another way to attach a continuation:
                //Task.Delay(5000).ContinueWith(ant =>
                //{
                //    Console.WriteLine($"second Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //    Console.WriteLine(99);
                //});

                //Console.WriteLine($"Ready to go,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");

            }
                // Value Task
                //TODO: 执行顺序很混乱，没有结果，主线程没有执行最后两句，为什么？

            {
                //var vt1 = AnswerQuestionAsync("What's the answer to life?");
                //var vt2 = AnswerQuestionAsync("Is the sun shining?");

                //Console.WriteLine($"vt1.IsCompleted: {vt1.IsCompleted}"); // True
                //Console.WriteLine($"vt2.IsCompleted: {vt2.IsCompleted}"); // False

                //var a1 = await vt1;
                //Console.WriteLine($"a1: {a1}"); // Immediate

                //var a2 = await vt2;
                //Console.WriteLine($"a2: {a2}"); // Takes 5 seconds to appear

                //async ValueTask<string> AnswerQuestionAsync(string question)
                //{
                //    if (question == "What's the answer to life?")
                //        return "42"; // ValueTask<string>

                //    return await AskCortanaAsync(question); // ValueTask<Task<string>>
                //}

                //async Task<string> AskCortanaAsync(string question)
                //{
                //    Console.WriteLine("I am Cortana!");
                //    await Task.Delay(5000);
                //    return "I don't know.";
                //}
                //Console.WriteLine("Act please!");   //这一句以及下一句没有执行，就退出了，为什么？
                //Thread.Sleep(10000);//主人要在这里等着
            }

            #endregion

            #region MyTest

            {
                //await Task.Run(PrintOne);
                //await Task.Run(PrintTwo);
                ////
                ///*
                //    IN  Main() ===ThreadID = 1
                //    PrintOne,  ThreadID : 4
                //    MIKE
                //    BACK TO Main() ===ThreadID = 1
                //    PrintTwo,  ThreadID : 4
                //    MIKE Tang
                //     Out  Main() ===ThreadID = 1
                //    Hello World!
                // */

            }


            {
                //Task.Run(PrintOne);
                //Task.Run(PrintTwo);

                ////
                ///*
                //IN  Main() ===ThreadID = 1
                //BACK TO Main() ===ThreadID = 1
                //PrintOne,  ThreadID : 4
                //MIKE
                //PrintTwo,  ThreadID : 5
                //MIKE Tang
                // Out  Main() ===ThreadID = 1
                //Hello World!
                // */
            }

            {
                //Task task1=Task.Run(PrintOne);
                //Task task2 = Task.Run(PrintTwo);
                //await task1;
                //await task2;

                ////
                ///*
                //    IN  Main() ===ThreadID = 1
                //    PrintTwo,  ThreadID : 4
                //    MIKE Tang
                //    PrintOne,  ThreadID : 5
                //    MIKE
                //    BACK TO Main() ===ThreadID = 1
                //     Out  Main() ===ThreadID = 1
                //    Hello World!
                // */
            }


            {
                //Task task1 = Task.Run(PrintOne);
                //Task task2 = Task.Run(PrintTwo);
                //await task1;


                ////
                ///*
                //    IN  Main() ===ThreadID = 1
                //    PrintTwo,  ThreadID : 5
                //    MIKE Tang
                //    PrintOne,  ThreadID : 4
                //    MIKE
                //    BACK TO Main() ===ThreadID = 1
                //     Out  Main() ===ThreadID = 1
                //    Hello World!
                // */

            }

            {
                //Task task1 = Task.Run(PrintOne);
                //Task task2 = Task.Run(PrintTwo);
                //await task2;


                ////
                ///*
                //    IN  Main() ===ThreadID = 1
                //    PrintTwo,  ThreadID : 5
                //    MIKE Tang
                //    PrintOne,  ThreadID : 4
                //    MIKE
                //    BACK TO Main() ===ThreadID = 1
                //     Out  Main() ===ThreadID = 1
                //    Hello World!
                // */
            }

            {
                //await Task.Run(PrintOne);
                //Task.Run(PrintTwo);



                ////
                ///*
                //    IN  Main() ===ThreadID = 1
                //    PrintOne,  ThreadID : 4
                //    MIKE
                //    BACK TO Main() ===ThreadID = 1
                //    PrintTwo,  ThreadID : 5
                //    MIKE Tang
                //     Out  Main() ===ThreadID = 1
                //    Hello World!
                // */
            }
            {
                //Task.Run(PrintOne);
                //await Task.Run(PrintTwo);



                ////
                ///*
                //    IN  Main() ===ThreadID = 1
                //    PrintTwo,  ThreadID : 5
                //    PrintOne,  ThreadID : 4
                //    MIKE
                //    MIKE Tang
                //    BACK TO Main() ===ThreadID = 1
                //     Out  Main() ===ThreadID = 1
                //    Hello World!
                // */
            }
            {
                //Console.WriteLine("In Test");
                //await Task.Run(TestOne);
                //Console.WriteLine("Between Test");
                //await Task.Run(TestTwo);
                //Console.WriteLine("Out Test");

                ///*
                //IN  Main() ===ThreadID = 1
                //In Test
                //BACK TO Main() ===ThreadID = 1  //   这里涉及反回CALLER的时机，遇到TASK时才返回，如果AWAIT一个方法，要进入后，找到TASK才回去
                //TestOne-Before Delay,  ThreadID : 4   //遇到 TESTONE中的AWAIT, 退出TESTONE
                //Between Test                          // 
                //TestTwo-Before Delay,  ThreadID : 4
                //Out Test
                //TestOne-After  Delay,  ThreadID : 4
                //MIKE
                //TestTwo-After  Delay,  ThreadID : 4
                //MIKE Tang
                // Out  Main() ===ThreadID = 1
                //Hello World!
                // */
            }

            {
                //Console.WriteLine("In Test");
                //Task.Run(TestOne);
                //Console.WriteLine("Between Test");
                //Task.Run(TestTwo);
                //Console.WriteLine("Out Test");

                ///*
                //IN  Main() ===ThreadID = 1
                //In Test
                //Between Test
                //Out Test
                //BACK TO Main() ===ThreadID = 1
                //TestOne-Before Delay,  ThreadID : 4
                //TestTwo-Before Delay,  ThreadID : 5
                //TestOne-After  Delay,  ThreadID : 4
                //MIKE
                //TestTwo-After  Delay,  ThreadID : 4
                //MIKE Tang
                // Out  Main() ===ThreadID = 1
                //Hello World!
                // */
            }
            #endregion

        }

        static void PrintOne()
        {
            Console.WriteLine($"PrintOne,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("MIKE");
        }

        static void PrintTwo()
        {
            Console.WriteLine($"PrintTwo,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("MIKE Tang");
        }

        static async void TestOne()
        {
            Console.WriteLine($"TestOne-Before Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(5000);
            Console.WriteLine($"TestOne-After  Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("MIKE");
        }

        static async void TestTwo()
        {
            Console.WriteLine($"TestTwo-Before Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(10000);
            Console.WriteLine($"TestTwo-After  Delay,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("MIKE Tang");
        }



    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Disposal
{
    class DisposalGarbageCollection
    {
        public static void Test()
        {
            #region Disposal Finalizer GarbageCollection

            //Current Memory Stats
            {
                //long totalMemAllocated = 0;
                //for (int i = 0; i < 200; i++)
                //{
                //    totalMemAllocated += AllocateSomeNonreferencedMemory();
                //    string procName = Process.GetCurrentProcess().ProcessName;
                //    using PerformanceCounter pcPB = new PerformanceCounter("Process", "Private Bytes", procName);
                //    long memoryUsed = GC.GetTotalMemory(false); // Change to true to force a collection before reporting used memory
                //    Console.WriteLine($"Currently OS allocated: {pcPB.NextValue()}. Current GC reported {memoryUsed}. Allocated at some point {totalMemAllocated}.");
                //}
            }
            // ANONYMOUS DISPOSAL(PROBLEM)
            // dispose()是主动的DISPOSE,在OBJECT退出SCOPE时被调用
            // 而FINALIZER是被动的，是GC在周期性清理无REFERENCE的内存空间时，发现有这个FINALIZER方法，就搜集放在QUEUE中，依次处理
            // 而没有FINALIZER的，会被立即清理掉
            // 这里别开一个逻辑，DISPOSE()和FINALIZE()并不矛盾，它们是发生在不同时间
            // 当变量退出作用域时，就触发DISPOSE(),这时根据事前写的CODE,释放一些UNMANAGED 资源，如FILE HANDLER,SCREAM之类
            // 然后，这个变量继续存在HEAP中，注意，此时这个变量还是LIVE,等GC来清理，
            // 当GC清理到这个变量时，没有FINALIZER的，就是ORPHANED的，就直接清理了。
            // 有FINALIZER的,会被GC放到QUEUE中等候，逐个运行FINALIZER,
            // 然后这个OBJECT才变得ORPHANED,会被下一次清理清掉。
            {
                var foo = new Foo();

                // Test it without calling SuspendEvents()
                foo.FireSomeEvent();

                // Now test it with event suspension:
                //这个 foo.SuspendEvents不但将_suspendCount增加1
                //而且返回一个IDisposable对象：SuspendToken，它意味着，用它这个对象，就会被清理
                //由于USING 结构，出了USING,就要启动这个SuspendToken中的DISABLE()方法
                //这个方法实际是把_suspendCount复原
                // if (_foo != null) _foo._suspendCount--;（又将_suspendCount复原）
                //_foo = null;
                using (foo.SuspendEvents()) //_suspendCount变成了1
                {
                    foo.FireSomeEvent();
                }
                // 此时，_suspendCount又变成了0
                // Now test it again without calling SuspendEvents()
                foo.FireSomeEvent();
            }
            //ANONYMOUS DISPOSAL(solution)
            //这个模板更具操作性和扩展情，可以学习借用
            {
                var foo = new NewFoo();

                // Test it without calling SuspendEvents()
                foo.FireSomeEvent();

                // Now test it with event suspension:
                using (foo.SuspendEvents())
                {
                    foo.FireSomeEvent();
                }

                // Now test it again without calling SuspendEvents()
                foo.FireSomeEvent();
            }
            // TEST DISPOSE
            // 这个DISPOSE()的机制是SCOPE
            // 如果在USING格式中，这个DISPOSE()会被执行，一旦发现退出了当前的SCOPE
            // 如果不在USING格式中，要自己调用这个DISPOSE()
            {
                Demo a = null;
                using (Demo d = new Demo())
                {
                    d.Name = "Mike";
                    a = d;

                }
                //如果有DISPOSE（），而且用在USING格式中，退出SCOPE之前，会被执行一次？
                var name = a.Name;  //MIKE

                Demo c = new Demo(); // 在GO OUT OF SCOPE之前，执行DISPOSE（）？
                c.Name = "tom";
                c.Dispose();    // 手动执行这个方法，不会影响这个对象，它还活着
                var n = c.Name;



            }

            //RESURRECTION
            //这个例子测试不成功
            //这个创建的文件无法被清理掉
            //观察到PUSH TO QUEUE，但何时DEQUEUE,需不需要人工操作，不清楚
            {
                string filename = "d://tempref.tmp";
                // Create and open file so it cannot be deleted
                var writer = File.CreateText(filename);

                // Get the temporary reference in a separate method.
                // Variable will go out of scope upon return and be eligible for GC.
                CreateTempFileRef();

                GC.Collect(); // Run the garbage collector
                //GC.Collect(); // Run the garbage collector
                //GC.Collect(); // Run the garbage collector

                TempFileRef._failedDeletions.Dump();


                void CreateTempFileRef()
                {
                    var tempRef = new TempFileRef(filename);

                }
            }
            #endregion
        }

        static long AllocateSomeNonreferencedMemory()
        {
            int loops = 64;
            int size = 1024;
            for (int i = 0; i < loops; i++)
            {
                int[] array = new int[size];
            }

            return loops * size * 4; // int is 32-bits (4 bytes)
        }

    }
    //模板，方法运行后，利用DISPOSE()来自动关闭之前设置的开关
    class Foo
    {
        int _suspendCount;

        public IDisposable SuspendEvents()
        {
            _suspendCount++; //先修改自己的状态
            return new SuspendToken(this); //然后将自己的引用传给新建的TOKEN
        }

        public void FireSomeEvent()
        {
            if (_suspendCount == 0)
                "Event would fire".Dump();
            else
                "Event suppressed".Dump();
        }

        class SuspendToken : IDisposable
        {
            Foo _foo;//引用的是外面的ENCLOSING CLASS
            public SuspendToken(Foo foo) => _foo = foo; //通过这个拿到FOO的引用，从而可以修改FOO的状态
            public void Dispose()                     // TOKEN在OUT OF SCOPE之前，执行DISPOSE，利用FOO的引用，将FOO的参数再次复原
            {
                if (_foo != null) _foo._suspendCount--;
                _foo = null;
            }
        }
    }

    //更好的模板，方法退回后，利用DISPOSE()来自动关闭之前设置的开关
    //这个被称这anonymous disposal,为什么？？细读NUTSHELL
    //与上一个方案下比，这里返回的是一个DISPOSABLE实例，上一个返回的是TOKEN实例
    //是否因为上面专门建了一个SuspendToken CLASS，只能FOO自己用来管理状态
    //而下面这个建了一个Disposable的CLASS,这个可以一直用，谁用都可以

    class NewFoo
    {
        int _suspendCount;

        public IDisposable SuspendEvents()
        {
            _suspendCount++;
            // 这次返回的是一个Disposable CLASS的实例
            // 注意CONSTRUCTOR的构建方式，是一个ACTION
            // 而且用的是Disposable的一个静态方法要调用它的构造函数
            // 注意，这个ACTION中的逻辑是可以变的，根据具体的情况定
            // 不一定是_suspendCount--，这是因为第一行是_suspendCount++;
            // 但这个ACTION的动作是抵消掉第一行的影响
            return Disposable.Create(() => _suspendCount--);
        }

        public void FireSomeEvent()
        {
            if (_suspendCount == 0)
                "Event would fire".Dump();
            else
                "Event suppressed".Dump();
        }
    }

    // Reusable class
    public class Disposable : IDisposable
    {
        //Disposable的一个静态方法要调用它的构造函数
        public static Disposable Create(Action onDispose)
            => new Disposable(onDispose);
        //FIELD
        Action _onDispose;
        //CONSTRUCTOR这个构造器很简单，只是将接到的ACTION存起来
        Disposable(Action onDispose) => _onDispose = onDispose;

        public void Dispose()
        {
            _onDispose?.Invoke(); //如有这个方法，有启动 _suspendCount--
            _onDispose = null;
        }
    }
    
    // call dispose() From finalizer
    class Test : IDisposable
    {
        public void Dispose()                   // NOT virtual
        {
            Dispose(true);
            GC.SuppressFinalize(this);       // Prevent finalizer from running.
        }
        //这个是PROTECTED和VIRTUAL的，可以传下去被OVERRIDE
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)  //由VOID 这个DISPOSE调用
            {
                // Call Dispose() on other objects owned by this instance.
                // You can reference other finalizable objects here.
                // ...
            }
            else//这个ELSE是我加的， ~Test()是不是会执行这里面的
            {
                // FINALIZER调用的DISPOSE不要涉及别人的引用（NUTSHELL书553）
            }

            // Release unmanaged resources owned by (just) this object.
            // ...
        }

        ~Test()
        {
            Dispose(false);
        }
    }

    //这个DISPOSE起不起作用在于你写了什么
    //它会在退出SCOPE时执行
    //如果在DISPOSE中写了无关的CODE,也没啥用处
    //这个DISPOSE在用在USING格式中，等退出SCOPE时由系统调用，
    //自已主动去调用是没有意义的。注意模板的写法（在FINIALIZER中调DISPOSE)
    public class Demo : IDisposable
    {
        public string Name { set; get; }
        public void Dispose()
        {
            Console.WriteLine("dispose!");
        }
    }

    //RESURRECTION
    public class TempFileRef
    {
        // 注意，这是个STATIC 方法
        static internal ConcurrentQueue<TempFileRef> _failedDeletions    //创建一个THREAD-SAFE QUEUE
            = new ConcurrentQueue<TempFileRef>();            //field

        public readonly string FilePath;                      //field
        public Exception DeletionError { get; private set; }  //Property 
      
        public TempFileRef(string filePath) { FilePath = filePath; }  //constructor

        //int _deleteAttempt;                           // Uncomment if re-registering the finalizer

        ~TempFileRef()          //Finalizer
        {
            try
            {
                Console.WriteLine("try to delete file!");
                File.Delete(FilePath);
                Console.WriteLine("delete file done!");
            }            //清理文件
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                DeletionError = ex;
                _failedDeletions.Enqueue(this);   // Resurrection如果清理出现ERROR,则将这个实例放在QUEUE中，让其保持活性直到
                Console.WriteLine("Push to the Queue");
                // We can re-register for finalization by uncommenting:
                //if (_deleteAttempt++ < 3) GC.ReRegisterForFinalize (this);  // 如果删了三次不行，就重新注册，以便在下一轮清理中再试
            }
        }
    }
}

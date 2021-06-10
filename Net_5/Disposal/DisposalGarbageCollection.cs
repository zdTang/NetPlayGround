using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                long totalMemAllocated = 0;
                for (int i = 0; i < 200; i++)
                {
                    totalMemAllocated += AllocateSomeNonreferencedMemory();
                    string procName = Process.GetCurrentProcess().ProcessName;
                    using PerformanceCounter pcPB = new PerformanceCounter("Process", "Private Bytes", procName);
                    long memoryUsed = GC.GetTotalMemory(false); // Change to true to force a collection before reporting used memory
                    Console.WriteLine($"Currently OS allocated: {pcPB.NextValue()}. Current GC reported {memoryUsed}. Allocated at some point {totalMemAllocated}.");
                }
            }
            // ANONYMOUS DISPOSAL(PROBLEM)
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
    class Foo
    {
        int _suspendCount;

        public IDisposable SuspendEvents()
        {
            _suspendCount++;
            return new SuspendToken(this);
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
            public SuspendToken(Foo foo) => _foo = foo;
            public void Dispose()  // 实现接口
            {
                if (_foo != null) _foo._suspendCount--;
                _foo = null;
            }
        }
    }


    class NewFoo
    {
        int _suspendCount;

        public IDisposable SuspendEvents()
        {
            _suspendCount++;
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
        public static Disposable Create(Action onDispose)
            => new Disposable(onDispose);

        Action _onDispose;
        Disposable(Action onDispose) => _onDispose = onDispose;

        public void Dispose()
        {
            _onDispose?.Invoke();
            _onDispose = null;
        }
    }
}

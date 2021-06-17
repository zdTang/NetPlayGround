using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"IN  Main() ===ThreadID = {Thread.CurrentThread.ManagedThreadId}");
            /*======加一个LOGGER====*/
            //Trace.Listeners.Clear();//CLEAR THE DEFAULT LISTENERS
            //var fileListener = new TextWriterTraceListener("D:\\trace.txt");
            //Trace.Listeners.Add(fileListener);


            /*=====TYPE=====*/
            //Net_5.TestAttribute.TestAttribute.Test();
            //Net_5.Type.TestDynamic.Test();
            //Net_5.Type.CharString.Test();
            //Net_5.Type.Tuple.Test();
            //Net_5.Type.Anonyemous.Test();
            //Net_5.Nullable.TestNullable.Test();
            //Net_5.Type.Utility.Test();
            //Net_5.Type.TestInterface.Test();

            /*====COLLECTION=====*/
            //Enumerator.Test();
            //DrillOnEnumerable.Test();
            //Enumeration.ENumerationDrill.Test();
            //Enumeration.TestEnumerable.Test();
            //Net_5.Enumeration.Iterator.Test();
            //Net_5.Collection.TestArray.Test();
            //Net_5.Collection.ListSoOn.Test();
            //Net_5.Collection.CustomizableCollection.Test();
            //Net_5.Collection.CustomizableCollectionNew.Test();
            //Net_5.Collection.ImmutableCollection.ImmutableCollection.Test();
            //Net_5.Collection.PluggingEqualityOrder.Test();

            /*================Concurrency=================*/
            //Net_5.Concurrent.Multithreading.Test();
            Net_5.Concurrent.TestTask.Test();
            //Net_5.Concurrent.TestAwaiter.Test();
            //Net_5.Concurrent.PrinciplesOfAsynchrony.Test();
            //Net_5.Concurrent.Asynchronous_Version_5.Test();
            //Net_5.Concurrent.ch01_01.TestAsync();
            //Net_5.Concurrent.illustratedBook_CH21.Test();
            //Net_5.Concurrent.Asynchronous_Stream.Test();
            //Net_5.Concurrent.AsynchronousPatterns.Test();
            Console.WriteLine($"BACK TO Main() ===ThreadID = {Thread.CurrentThread.ManagedThreadId}");

            //Net_5.Comparison.EqualityComparison.Test();
            //Net_5.Comparison.OrderComparison.Test();
            //TestTryCatch.Test();
            //Net_5.Linq.TestLinq.Test();
            //Net_5.Disposal.DisposalGarbageCollection.Test();
            //Net_5.Disposal.Template.Test(); TestDiagnostic
            //Net_5.Diagnostics.TestDiagnostic.Test();
            //Net_5.TryCatch.TestThrow.Test();

            /*====Write into log file=====*/
            //fileListener.Flush();  //这个强制写入，冲掉CACHE.任何时候需要确保写入，用FLASH
            //fileListener.Close();// close the LOG FILE Handler

            Console.ReadKey();
            Console.WriteLine($"Out  Main() ===ThreadID = {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Hello World!");
        }



    }
}

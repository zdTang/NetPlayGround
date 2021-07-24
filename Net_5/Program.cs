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

            /*=======Syntax=======*/
            //Net_5.Syntax.TestIterationAndDecision.Test();      // 各种LOOP和IF-ELSE IF
            //Net_5.Syntax.Pattern.Test();                       // 各种SWITCH, PATTERN

            /*=====TYPE=====*/
            //Net_5.TestAttribute.TestAttribute.Test();
            //Net_5.testType.TestDynamic.Test();
            //Net_5.testType.CharString.Test();
            //Net_5.testType.Tuple.Test();
            //Net_5.testType.Anonyemous.Test();
            //Net_5.Nullable.TestNullable.Test();
            //Net_5.testType.Utility.Test();
            //Net_5.testType.TestInterface.Test();
            //Net_5.testType.WhatIsType.Test();
            Net_5.testType.TestClass.Test();
            //Net_5.testType.BasicTypes.Test();
            //Net_5.testType.TestString.Test();
            //Net_5.testType.ImplicitlyTypedLocalVars.Test();
            //Net_5.testType.FunWithArray.Test();

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
            //Net_5.Concurrent.TestTask.Test();
            //Net_5.Concurrent.TestAwaiter.Test();
            //Net_5.Concurrent.PrinciplesOfAsynchrony.Test();
            //Net_5.Concurrent.Asynchronous_Version_5.Test();
            //Net_5.Concurrent.ch01_01.TestAsync();
            //Net_5.Concurrent.illustratedBook_CH21.Test();
            //Net_5.Concurrent.Asynchronous_Stream.Test();
            //Net_5.Concurrent.AsynchronousPatterns.Test();


            /*================ IO =================*/
            //Net_5.Stream.StreamIO.Test();
            //Net_5.Stream.Adapter.Test();


            /*================ NetWorking =================*/

            //Net_5.Networking.Client_side.Test();

            /*================ Reflection =================*/
            //Net_5.TestReflection.ReflectAndActivateType.Test();
            //Net_5.TestReflection.ReflectingAndInvokingMember.Test();
            //Net_5.TestReflection.WorkingWithAttribute.Test();
            //Net_5.TestReflection.WhatIsType.Test();

            //TODO: 有空研究
            //Net_5.TestReflection.DynamicCodeGeneration.Test();

            //TODO: 有空研究
            /*================ Dynamic Programming =================*/
            //Net_5.DynamicProgramming.DynamicProgramming.Test();

            /*================ Delegate =================*/
            //Net_5.TestDelegate.CreateDelegate.Test();

            /*================ Serialization =================*/
            //Net_5.Serialization.XMLserializer.Test();

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
            Console.WriteLine($"BACK TO Main() ===ThreadID = {Thread.CurrentThread.ManagedThreadId}");
            Console.ReadKey();
            Console.WriteLine($"Out  Main() ===ThreadID = {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Hello World!");
        }



    }
}

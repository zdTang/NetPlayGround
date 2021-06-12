﻿using System;
using System.Diagnostics;

namespace Net_5
{
    class Program
    {
        static void Main(string[] args)
        {
            /*======加一个LOGGER====*/
            Trace.Listeners.Clear();//CLEAR THE DEFAULT LISTENERS
            var fileListener = new TextWriterTraceListener("D:\\trace.txt");
            Trace.Listeners.Add(fileListener);


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
            Net_5.Concurrent.Multithreading.Test();



            //Net_5.Comparison.EqualityComparison.Test();
            //Net_5.Comparison.OrderComparison.Test();
            //TestTryCatch.Test();
            //Net_5.Linq.TestLinq.Test();
            //Net_5.Disposal.DisposalGarbageCollection.Test();
            //Net_5.Disposal.Template.Test(); TestDiagnostic
            //Net_5.Diagnostics.TestDiagnostic.Test();

            Console.WriteLine("Hello World!");
            /*====Write into log file=====*/
            //fileListener.Flush();  //这个强制写入，冲掉CACHE.任何时候需要确保写入，用FLASH
            //fileListener.Close();// close the LOG FILE Handler
        }
    }
}

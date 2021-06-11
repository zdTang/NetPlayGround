#define  TESTMODE   //可以当一个开关用
#define PLAYMODE
#undef PLAYMODE  // Cancels our define above if not commented out. Also cancels a define from the compiler e.g. through Visual Studio settings.
#define LOGGINGMODE
#define DEBUG
#define TRACE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5.Diagnostics
{
    class TestDiagnostic
    {
        public static bool EnableLogging;
        public static void Test()
        {
            //当上面有这个TESTMODE标志有定义时，才运行下面这句
            //如果上面取别它，这一句就会被COMMENT OUT
            //可以手动用#DEFINE定义CONSTANT，也可以在PROJ这个文件定义，以便整个PROJECT都可用
            {
                #if TESTMODE
                Console.WriteLine ("in test mode!");
                #endif
                Print(); //这个方法有一个CONDITIONAL ATTRIBUTE,如果没有提前定义TESTMODE这个CONSTANT,这个方法不可用
                #if TESTMODE
                Console.WriteLine("in test mode!");
                #endif
                #if TESTMODE && !PLAYMODE      // if TESTMODE and not PLAYMODE
                 Console.WriteLine ("Test mode and NOT play mode"); 
                #endif
                #if PLAYMODE
                Console.WriteLine("Play mode is defined.");
                #endif
                LogStatus("Only if LOGGINGMODE is defined.");

                [Conditional("LOGGINGMODE")]
                static void LogStatus(string msg)
                {
                    Console.WriteLine($"LOG: {msg}");
                }
            }

            //通过ATTRIBUTE取出CALLER的信息
            {
                var x = MyProperty;
                MyMethod();
            }
            //Conditional Attribute Alternative Lambda
            {
                EnableLogging = true;
                Func<string> msg1 = () => { Console.WriteLine("The first lambda was evaluated"); return "My first message"; };
                LogStatus(msg1);

                EnableLogging = false;
                Func<string> msg2 = () => { Console.WriteLine("The second lambda was evaluated"); return "My second message"; };
                LogStatus(msg2);

                Console.WriteLine("Let's see what was logged:");
                Console.WriteLine(File.ReadAllText("Conditional.log"));
            }
            // Debug and Trace
            {

                Debug.Write("DATA");
                Debug.WriteLine("data");
                Trace.WriteLine("trace!");
                Debug.Fail("file DATA.txt does not exist");
                Debug.Assert(false ,"file DATA.txt does not exist");//if fail,call Fail
            }
            // TRACE listener
            // 这一段是TRACE技术，可以用来LOG或DEBUG
            // 创建了三种不同的LISTENER
            // 将TRACE到的信息写到EVENT, CONSOLE或一个TXT文件中，以供查询
            // 做LOGGER功能的话，这一段需要认真研究
            {
                //这个LISTENER是一个TraceListenerCollection
                Trace.Listeners.Clear();//CLEAR THE DEFAULT LISTENERS

                //第一种，这个LISTENER把监听到的写到文件中去
                //它继承自TraceListener
                // Add a writer that appends to the trace.txt file:
                var fileListener = new TextWriterTraceListener("D:\\trace.txt");
                var x = fileListener.Filter;//这个FILTER来自父类TraceListener的一个PROPERTY
                //x.ShouldTrace()//这里设置哪些需经TRANCE，即写入到FILE中
                Trace.Listeners.Add(fileListener);
               
                // 第二种，拿到向CONSOLE中书写的STREAM，放入TRACE的集合中
                // Obtain the Console's output stream, then add that as a listener:
                System.IO.TextWriter tw = Console.Out;
                Trace.Listeners.Add(new TextWriterTraceListener(tw)); //放入LISTENER集合中

                // Set up a Windows Event log source and then create/add listener.
                // CreateEventSource requires administrative elevation, so this would
                // typically be done in application setup.

                //The following requires Administrator permission to run and is Windows-specific
                //下面要用ADMINISTRATOR运行VS,否则不让CREATE EVENT SOURCE
                if (!EventLog.SourceExists ("DemoApp"))
                    EventLog.CreateEventSource ("DemoApp", "Application");
                
                // 第三种，NEW一个EVENTLOGTRACELISTENER, 加入LISTENER中，这将把TRACE到的写到EVENT LOG
                // 用EVENT VIEWER找APPLICATION + DEMOAPP可以找到两条LOG
                Trace.Listeners.Add (new EventLogTraceListener ("DemoApp"));
                

                Console.WriteLine("Writing to trace. Will appear on console and in trace.txt.");

                Trace.WriteLine("Foo");// 注意， 这条是TRACE发出EMIT的信息，会被三个LISTENER接收

                bool myCondition = true;
                Trace.WriteLineIf(myCondition, "This will write");//TRACE发出EMIT信息，会被三个LISTENER接收
                Trace.WriteLineIf(!myCondition, "This will NOT write");//可以有BOOL

                Console.WriteLine("Done writing to trace. Let's see what's in trace.txt:");
                fileListener.Flush();  //这个强制写入，冲掉CACHE.任何时候需要确保写入，用FLASH
                fileListener.Close();// 可以单独关掉一个LISTENER, 这个写入TXT的LISTENER被关掉了！
                //关掉APP之前，必须CLOSE,否则会丢掉STEAM中的信息
                Console.WriteLine(File.ReadAllText("trace.txt"));
            }

            // DEBUGGER是STATIC CLASS
            // IDE会自动启动DEBUGGER,对于SERVICES之类的无法DEBUG的，要用DEBUG PROCESS的方法
            // https://docs.microsoft.com/en-us/visualstudio/debugger/attach-to-running-processes-with-the-visual-studio-debugger?view=vs-2019
            {
                var a=Debugger.Launch();
                var b = Debugger.IsLogging();
                Debugger.Break();
                Debugger.Launch();
            }

            // Debugger Attribute
            {
                //https://docs.microsoft.com/en-us/dotnet/framework/debug-trace-profile/enhancing-debugging-with-the-debugger-display-attributes
                //
            }

            // Examining Running Processes
            {
                foreach (Process p in Process.GetProcesses()
                        .Where(pr => pr.ProcessName.StartsWith("L")) // Optional filter to narrow it down
                )
                    using (p)
                    {
                        Console.WriteLine(p.ProcessName);
                        Console.WriteLine("   PID:      " + p.Id);
                        Console.WriteLine("   Memory:   " + p.WorkingSet64);
                        Console.WriteLine("   Threads:  " + p.Threads.Count);
                        EnumerateThreads(p);
                    }

                void EnumerateThreads(Process p)
                {
                    try
                    {
                        foreach (ProcessThread pt in p.Threads)
                        {
                            Console.WriteLine(pt.Id);
                            Console.WriteLine("   State:    " + pt.ThreadState);
                            Console.WriteLine("   Priority: " + pt.PriorityLevel);
                            Console.WriteLine("   Started:  " + pt.StartTime);
                            Console.WriteLine("   CPU time: " + pt.TotalProcessorTime);
                        }
                    }
                    catch (InvalidOperationException ex) // The process may go away while enumerating its threads
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                    catch (Win32Exception ex) // We may not have access
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }
            // Stack Trace
            {
                static void Total() { A(); }

                static void A() { B(); }

                static void B() { C(); }

                static void C()
                {
                    StackTrace s = new StackTrace(true);
                    Console.WriteLine("Total frames: " + s.FrameCount);
                    Console.WriteLine("Current method: " + s.GetFrame(0).GetMethod().Name);
                    Console.WriteLine("Calling method: " + s.GetFrame(1).GetMethod().Name);
                    Console.WriteLine("Entry method: " + s.GetFrame
                        (s.FrameCount - 1).GetMethod().Name);
                    Console.WriteLine("Call Stack:");
                    foreach (StackFrame f in s.GetFrames())
                        Console.WriteLine(" File: " + f.GetFileName() +
                                          " Line: " + f.GetFileLineNumber() +
                                          " Col:  " + f.GetFileColumnNumber() +
                                          " Offset: " + f.GetILOffset() +
                                          " Method: " + f.GetMethod().Name);
                }
            }

            // Using Windows EventLOG
            {
                //The following requires Administrator permission to run and is Windows-specific
                //下面要用ADMINISTRATOR运行VS,否则不让CREATE EVENT SOURCE
                if (!EventLog.SourceExists("DemoApp"))
                {
                    EventLog.CreateEventSource("DemoApp", "Application");  //注册一个SOURCE
                    EventLog.WriteEntry("DemoApp", "hello", EventLogEntryType.Error); //写入LOGGER
                    // 这个WRITEENTRY有好多OVERLOADING 
                }
               
            }
            // Read Windows EventLog
            //https://www.loggly.com/ultimate-guide/windows-logging-basics/
            {
                Console.WriteLine("LOGS ON THIS COMPUTER");
                foreach (EventLog l in EventLog.GetEventLogs())
                {
                    try
                    {
                        Console.WriteLine(l.LogDisplayName);
                    }
                    catch (Exception ex)
                    {
                        // The display name might be unavailable but this property still is available
                        Console.WriteLine($"Error processing an event log '{l.Log}': {ex.Message}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Appliation EVENT LOG");

                EventLog log = new EventLog("Application");

                Console.WriteLine("Total entries: " + log.Entries.Count);

                EventLogEntry last = log.Entries[log.Entries.Count - 1];
                Console.WriteLine("Index:   " + last.Index);
                Console.WriteLine("Source:  " + last.Source);
                Console.WriteLine("Type:    " + last.EntryType);
                Console.WriteLine("Time:    " + last.TimeWritten);
                Console.WriteLine("Message: " + last.Message);

                log.EnableRaisingEvents = true;
                log.EntryWritten += DisplayEntry;

                // Monitor for event log entries for 60 seconds
                // If running in LINQPad, you can stop the query to end early
                Thread.Sleep(60 * 1000);

            }

            // Performence Counter
            {
                string category = "Nutshell Monitoring";
                // We'll create two counters in this category:
                string eatenPerMin = "Macadamias eaten so far";
                string tooHard = "Macadamias deemed too hard";

                if (!PerformanceCounterCategory.Exists(category))
                {
                    CounterCreationDataCollection cd = new CounterCreationDataCollection();

                    cd.Add(new CounterCreationData(eatenPerMin,
                        "Number of macadamias consumed, including shelling time",
                        PerformanceCounterType.NumberOfItems32));

                    cd.Add(new CounterCreationData(tooHard,
                        "Number of macadamias that will not crack, despite much effort",
                        PerformanceCounterType.NumberOfItems32));

                    // This line requires elevated permissions. Either run LINQPad as administrator (for this query only!) 
                    // or create the category in a separate program you run as administrator.
                    PerformanceCounterCategory.Create(category, "Test Category",
                        PerformanceCounterCategoryType.SingleInstance, cd);
                }

                using (PerformanceCounter pc = new PerformanceCounter(category,
                    eatenPerMin, ""))
                {
                    pc.ReadOnly = false;
                    pc.RawValue = 1000;
                    pc.Increment();
                    pc.IncrementBy(10);
                    Console.WriteLine(pc.NextValue());    // 1011
                }
            }

            // Monitor Performance
            {
                Console.WriteLine("Display current values of performance counters:");

                using PerformanceCounter pc1 = new PerformanceCounter("Processor",
                    "% Processor Time",
                    "_Total");
                Console.WriteLine(pc1.NextValue());

                string procName = Process.GetCurrentProcess().ProcessName;
                using PerformanceCounter pc2 = new PerformanceCounter("Process",
                    "Private Bytes",
                    procName);
                Console.WriteLine(pc2.NextValue());

                Console.WriteLine("Monitor performance counters:");

                EventWaitHandle stopper = new ManualResetEvent(false);

                new Thread(() =>
                    Monitor("Processor", "% Processor Time", "_Total", stopper)
                ).Start();

                new Thread(() =>
                    Monitor("LogicalDisk", "% Idle Time", "C:", stopper)
                ).Start();


                // When running in LINQPad, we'll monitor for 60 seconds then exit. Stop the query to end early.
                Console.WriteLine("Monitoring - wait 60 seconds or stop query to quit");
                Thread.Sleep(60 * 1000);

                // In a console app, you can run until a key is pressed:
                //Console.WriteLine ("Monitoring - press any key to quit");
                //Console.ReadKey();

                stopper.Set();
            }

            // Bad Program
            {
                // This program is intentially written to perform poorly. You can run it while experimenting with the diagnostics techniques described in Chapter 13.
                // The diagnostic tools need our process ID:
                Console.WriteLine($"Our process ID {Process.GetCurrentProcess().Id}");
                MemoryLeak();
            }
        }

        public static string MyProperty
        {
            get
            {
                CallMe("From a property.");
                return "";
            }
        }

        public static void MyMethod()
        {
            CallMe("From a method.");
        }
        //利用ATTRIBUTE取出CALLER的信息
        //参数会被ATTRIBUTE中取出的信息替代
        static void CallMe(string ordinaryParameter,
            [CallerMemberName] string memberName = "", // Must be an optional parameter
            [CallerFilePath] string sourceFilePath = "", // Must be an optional parameter
            [CallerLineNumber] int sourceLineNumber = 0 // Must be an optional parameter
        )
        {
            Console.WriteLine($"{nameof(CallMe)} called from {memberName}{Environment.NewLine}" +
                              $"  Parameter: {ordinaryParameter}{Environment.NewLine}" +
                              $"  File: {sourceFilePath}{Environment.NewLine}" +
                              $"  Line: {sourceLineNumber}{Environment.NewLine}");
        }

       
        static void LogStatus(Func<string> message)
        {
            string logFilePath = "Conditional.log";
            if (EnableLogging) //EnableLogging是个BOOL,根据它来判断是不是走下一步
                System.IO.File.AppendAllText(logFilePath, message() + "\r\n");
        }
        [Conditional("TESTMODE")]  // TESTMODE是用#DEFINE定义的常量
        static void Print()
        {
            Console.WriteLine("Good");
        }

        static void DisplayEntry(object sender, EntryWrittenEventArgs e)
        {
            EventLogEntry entry = e.Entry;
            Console.WriteLine(entry.Message);
        }


        static void Monitor(string category, string counter, string instance,
            EventWaitHandle stopper)
        {
            if (!PerformanceCounterCategory.Exists(category))
                throw new InvalidOperationException("Category does not exist");

            if (!PerformanceCounterCategory.CounterExists(counter, category))
                throw new InvalidOperationException("Counter does not exist");

            if (instance == null) instance = "";   // "" == no instance (not null!)
            if (instance != "" &&
                !PerformanceCounterCategory.InstanceExists(instance, category))
                throw new InvalidOperationException("Instance does not exist");

            float lastValue = 0f;
            using (PerformanceCounter pc = new PerformanceCounter(category,
                counter, instance))
                while (!stopper.WaitOne(200, false))
                {
                    float value = pc.NextValue();
                    if (value != lastValue)         // Only write out the value
                    {                               // if it has changed.
                        Console.WriteLine(value);
                        lastValue = value;
                    }
                }
        }


        static Dictionary<string, int> cacheThatNeverCleansUp = new Dictionary<string, int>();

        static void MemoryLeak()
        {
            // Pretend this is an expensive calculation worth caching
            int CalculateSentenceScore(string sentence)
            {
                Stopwatch watch = Stopwatch.StartNew();
                try
                {
                    if (cacheThatNeverCleansUp.TryGetValue(sentence, out int score))
                        return score;

                    var calculatedScore = sentence.Split(' ').Count();
                    cacheThatNeverCleansUp.Add(sentence, calculatedScore);
                    return calculatedScore;
                }
                finally
                {
                    MyEventCounterSource.Log.Request(sentence, (float)watch.ElapsedMilliseconds);
                }
            }

            while (true) // Simulate e.g. a web service that keeps taking requests
            {
                var input = RandomSentence();
                var score = CalculateSentenceScore(input);
                // A web service might return the score to a caller    
            }
        }

        static string RandomSentence()
        {
            const string alpha = "abcdefghijklmnopqrstuvwxyz";
            List<string> words = new List<string>();
            int numWords = rnd.Next(2, 15);
            for (int w = 0; w < numWords; w++)
            {
                int wordLen = rnd.Next(1, 10);
                words.Add(new string(Enumerable.Repeat(alpha, wordLen)
                    .Select(_ => _[rnd.Next(_.Length)]).ToArray()));
            }
            return string.Join(' ', words);
        }

        int[] LongRandomArray(int size)
        {
            return Enumerable.Repeat(0, size).Select(i => rnd.Next()).ToArray();
        }

        static Random rnd = new Random();

        [EventSource(Name = "My-EventCounter-Minimal")]
        public sealed class MyEventCounterSource : EventSource
        {
            public static MyEventCounterSource Log = new MyEventCounterSource();
            private EventCounter requestCounter;

            private MyEventCounterSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat)
            {
                this.requestCounter = new EventCounter("Sentence Request", this);
            }

            public void Request(string sentence, float elapsedMSec)
            {
                WriteEvent(1, sentence, elapsedMSec);
                this.requestCounter.WriteMetric(elapsedMSec);
            }
        }



    }
    [DebuggerDisplay("Count = {count}")]
    class MyHashtable
    {
        public int count = 4;
    }
}

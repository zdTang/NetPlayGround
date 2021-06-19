using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Net_5.Stream
{
    class StreamIO
    {
        public async static void Test()
        {
            //Create and Write
            {
                //TODO:  坑
                /*
                   public enum FileMode
                      {
                        CreateNew = 1,
                        Create = 2,
                        Open = 3,
                        OpenOrCreate = 4,
                        Truncate = 5,
                        Append = 6,
                      }
                 */


                //using (System.IO.Stream s = new FileStream("d://demo.log", FileMode.Create))
                //{
                //    Console.WriteLine(s.CanRead);       // True
                //    Console.WriteLine(s.CanWrite);      // True
                //    Console.WriteLine(s.CanSeek);       // 表示可以Seek
                //    s.WriteByte(101);
                //    s.WriteByte(102);
                //    byte[] block = { 1, 2, 3, 4, 5 };
                //    s.Write(block, 0, block.Length);     // Write block of 5 bytes

                //    Console.WriteLine(s.Length);         // 7
                //    Console.WriteLine(s.Position);       // 7
                //    s.Position = 0;                       // Move back to the start

                //    Console.WriteLine(s.ReadByte());     // 101
                //    Console.WriteLine(s.ReadByte());     // 102

                //    // Read from the stream back into the block array:
                //    Console.WriteLine(s.Read(block, 0, block.Length));   // 5

                //    // Assuming the last Read returned 5, we'll be at
                //    // the end of the file, so Read will now return 0:
                //    Console.WriteLine(s.Read(block, 0, block.Length));   // 0
                //}

                #region FileStream
                {

                    //using (System.IO.Stream s = new FileStream("c:\\test.txt", FileMode.Create,FileAccess.Read))
                    //{
                    //    Console.WriteLine($"no_1,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //    byte[] block = { 1, 2, 3, 4, 5 };

                    //    await s.WriteAsync(block, 0, block.Length);    // Write asychronously
                    //    Console.WriteLine($"no_2,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //    s.Position = 0;                       // Move back to the start
                    //    Console.WriteLine($"no_3,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //    // Read from the stream back into the block array:
                    //    Console.WriteLine(await s.ReadAsync(block, 0, block.Length));   // 5
                    //    Console.WriteLine($"no_4,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //}

                    //// Clean up
                    //File.Delete("c:\\test.txt");
                    //Console.WriteLine($"no_5,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");

                }


                // WORK WITH DIRECTORY
                {
                    //string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                    //string logoPath = Path.Combine(baseFolder, "trace.txt");
                    //Console.WriteLine(File.Exists(logoPath));
                }

                //## ShortCut Method
                {
                    //string result= File.ReadAllText("d://trace.txt");//returns a string
                    //string[] resutlTwo=File.ReadAllLines("d://trace.txt");//returns an array of strings
                    //byte[] resultByte=File.ReadAllBytes("d://trace.txt");//returns a byte array
                    //File.WriteAllText("d://traceTwo.txt", result);
                    //File.WriteAllLines("d://traceTwo.txt", resutlTwo);
                    //File.WriteAllBytes("d://traceTwo.txt", resultByte);
                    //File.AppendAllText("d://traceTwo.txt","hello ,append"); //great for appending to a log file
                }


                //## Memory Stream
                {
                    //FileStream fs=File.OpenRead("d://trace.txt");
                    //var ms = new MemoryStream();
                    //fs.CopyTo(ms);
                    //var myFileStream=ms.ToArray();
                    //var l=ms.Length;

                }
                #endregion

                // Reading Test
                {
                    //using (System.IO.Stream s = new FileStream("d:\\trace.txt", FileMode.Open))
                    //{

                    //    s.Position = 0; 
                    //    byte[] block = new byte[1000];
                    //    int num = await s.ReadAsync(block, 0, block.Length);
                    //    Console.WriteLine($"no_4,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //    Console.WriteLine($"no_4,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //    Console.ReadKey();

                    //}

                }
                //丙种方式读文件，第一种，LOOP读入数组
                {
                    //using (System.IO.Stream s = new FileStream("d:\\demo.log", FileMode.Open))
                    //{
                    //    byte[] data = new byte[1000]; // bytesRead will always end up at 1000, unless the stream is // itself smaller in length:
                    //    int bytesRead = 0;
                    //    int chunkSize = 1;  //这里设为1，很奇怪，因为很快被覆盖，但设为0，LOOP就不动
                    //                        //可视为一个技巧，开始设为1，是为了让LOOP开动，因为这个值起被汇总之前，总要被覆盖的
                    //                        // Assuming s is a stream: byte[] data = new byte [1000];
                    //                        //s.Read(data, 0, data.Length);

                    //    //The correct way to read a file
                    //    while (bytesRead < data.Length && chunkSize > 0)
                    //    {
                    //        chunkSize = s.Read(data, bytesRead, data.Length - bytesRead);

                    //        bytesRead += chunkSize;
                    //    }

                    //}



                }
                // 第二种 BinaryReader, 这种方法更好
                // 注意动态数组的用法。这应该是先读出来，再根据尺寸来设ARRAY,而不是先建的ARRAY
                {
                    //using (System.IO.Stream s = new FileStream("d:\\demo.log", FileMode.Open))
                    //{

                    //    //another way to read a file
                    //    // 这种方法用ARRAY来读文件，注意，这个数组没有设定大小，是动态的
                    //    // 如果用同一个FILE HANDLER， 从前面读到的指针位置往下读
                    //    byte[] dataTwo = new BinaryReader(s).ReadBytes((int)s.Length);
                    //}

                }
                //Create s FileStream
                {
                    //FileStream fs1 = File.OpenRead("d://trace.txt"); // Read-only
                    
                    ////fs1.WriteByte(47);   //编译时不报错，RUNTIME报错
                    ////fs1.Flush();
                    //FileStream fs2 = File.OpenWrite("d://writeme.tmp"); // Write-only
                    //fs2.WriteByte(47);   //编译时不报错，RUNTIME报错
                    //fs2.Flush();
                    //FileStream fs3 = File.Create("d://readwrite.tmp"); // Read/write

                    ////using var fs = new FileStream("readwrite.tmp", FileMode.Open);
                    
                }
                {
                    //FileStream fs1 = File.OpenRead("demo.txt"); // Read-only
                    //FileStream fs2 = File.OpenWrite("demo.txt"); // Write-only
                    //   FileStream fs3 = File.Create("demo.txt"); // Read/write
                    /*string baseFolder = AppDomain.CurrentDomain.BaseDirectory; // the location where the assembly is 
                    string logoPath = Path.Combine(baseFolder, "demo.txt");
                    Console.WriteLine(File.Exists(logoPath));
                    string demo=File.ReadAllText("demo.txt");
                    byte[] demoByte=File.ReadAllBytes("demo.txt");
                    string[] demoLine=File.ReadAllLines("demo.txt");*/
                }
                    // Seeking
                    {
                    //using (System.IO.Stream s = new FileStream("d:\\demo.log", FileMode.Open))
                    //{
                    //    var a = s.CanRead;
                    //    var b = s.CanSeek;
                    //    var c = s.CanTimeout;
                    //    var d = s.CanWrite;
                    //    //var f=s.
                    //    var position = s.Position;

                    //}

                }
        

         
			}
        }
    }
}

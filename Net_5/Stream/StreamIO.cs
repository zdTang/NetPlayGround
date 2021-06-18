using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5.Stream
{
    class StreamIO
    {
        public async static void Test()
        {
            //Create and Write
            {
                //using (System.IO.Stream s = new FileStream("d://test.txt", FileMode.Create))
                //{
                //    Console.WriteLine(s.CanRead);       // True
                //    Console.WriteLine(s.CanWrite);      // True
                //    Console.WriteLine(s.CanSeek);       // True

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

                    //using (System.IO.Stream s = new FileStream("c:\\test.txt", FileMode.Create))
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
                #endregion
                {
                    using (System.IO.Stream s = new FileStream("c:\\demo.txt", FileMode.Open))
                    {
                        byte[] data = new byte[1000]; // bytesRead will always end up at 1000, unless the stream is // itself smaller in length:
                        int bytesRead = 0; int chunkSize = 1;
                        // Assuming s is a stream: byte[] data = new byte [1000];
                        //s.Read(data, 0, data.Length);

                        // The correct way to read a file
                        while (bytesRead < data.Length && chunkSize > 0)
                        {
                            bytesRead +=
                        chunkSize = s.Read(data, bytesRead, data.Length - bytesRead);
                        }
                        // another way to read a file
                        byte[] dataTwo = new BinaryReader(s).ReadBytes((int)s.Length);
                    }



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
			}
        }
    }
}

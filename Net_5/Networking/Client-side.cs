using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5.Networking
{
    class Client_side
    {
        public static async void Test()
        {
            #region WebClient
            //好多同步，异步下载上传工具，以及EVENT
            //当有两个AWAIT时，第二个AWAIT是第一个AWAIT的CONTINUATION
            //如果不用AWAIT, TASK会别起线程，同步运行，主线程也会继续向下跑
            //但用了AWAIT, 则主线程会返回CALLER,余下的内容由异步的这个线程完成工作后再做
            {
                //WebClient wc = new WebClient { Proxy = null };
                ////wc.DownloadFile("http://www.albahari.com/nutshell/code.aspx", "code.htm");         // synchronous
                ////可以绑定一个EVENT
                //wc.DownloadProgressChanged += (sender, args) =>
                //    Console.WriteLine(args.ProgressPercentage + "% complete");
                //Console.WriteLine($"NO 1,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                ////下面两个AWAIT，第一个是不需要的，但可以验证对流程的判断
                //await Task.Delay(5000).ContinueWith(ant => wc.CancelAsync());
                //Console.WriteLine($"NO 2,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //await wc.DownloadFileTaskAsync("http://oreilly.com", "webpage.htm");   // asynchronous
                //Console.WriteLine($"NO 3,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //// POST data
                ////wc.UploadValues("http://www.albahari.com/nutshell/code.aspx","POST",new NameValueCollection(100));
                ////OpenHtml("code.htm");
                //OpenHtml("webpage.htm");
            }

            #endregion

            #region WebRequest & WebResponse
            // 注意，CREATE方法可以根据URI的不同，生成不同的SUBCLASS, 包括  WebRequest, FileWebRequest, FtpWebRequest

            {
                //Console.WriteLine($"NO 1,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                ////DownloadPage();
                ////关于遇到AWAIT返回的点，线程并非遇到一个AWAIT方法就返回，而是进入方法，遇到那个具体的TASK值再返回
                //await DownloadPageAsync();
                //Console.WriteLine($"NO 100,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                //foreach (var file in Directory.EnumerateFiles(".", "*.html"))
                //    Console.WriteLine($"{file} {new FileInfo(file).Length} bytes");
            }

            #endregion

            #region HttpClient

            {
                // Simple Request,直接下载内容
                {
                    //string html = await new HttpClient().GetStringAsync("http://linqpad.net");
                    //Console.WriteLine($"NO 100,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");  //由异步线程完成
                    //html.Dump();
                }
                // Parallel Download 
                {
                    //var client = new HttpClient();
                    //Console.WriteLine($"NO 1,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //var task1 = client.GetStringAsync("http://www.linqpad.net");
                    //Console.WriteLine($"NO 2,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //var task2 = client.GetStringAsync("http://www.albahari.com");
                    //Console.WriteLine($"NO 3,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");

                    //(await task1).Length.Dump("First page length");
                    //Console.WriteLine($"NO 99,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //(await task2).Length.Dump("Second page length");
                    //Console.WriteLine($"NO 100,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                }
                //  Response Message
                {
                    var client = new HttpClient();
                    // The GetAsync method also accepts a CancellationToken.
                    Console.WriteLine($"NO 1,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    HttpResponseMessage response = await client.GetAsync("http://www.linqpad.net");
                    var header = response.Headers;
                    Console.WriteLine($"NO 2,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    response.EnsureSuccessStatusCode();
                    string html = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"NO 3,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    html.Dump("html");

                }
            }

            #endregion
        }

        static void OpenHtml(string location)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo("cmd", $"/c start {location}"));
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", location); // Desktop Linux
            else throw new Exception("Platform-specific code needed to open URL.");
        }
        static void DownloadPage()
        {
            WebRequest req = WebRequest.Create
                ("http://www.albahari.com/nutshell/code.html");
            req.Proxy = null;
            using (WebResponse res = req.GetResponse())
            using (System.IO.Stream rs = res.GetResponseStream())
            using (FileStream fs = File.Create("code_sync.html"))
                rs.CopyTo(fs);
        }

        // ##这是原版
        static async Task DownloadPageAsync()
        {
            //这里，根据URI的不同种类，可以得到FtpWebRequest, FileWebRequest
            WebRequest req = WebRequest.Create                            // 注意，这是个STATIC方法，建一个REQUEST
                ("http://www.albahari.com/nutshell/code.html");
            {
                //// 这个CREATE方法可以生成三种REQUEST，根据URI的不同，但需要显式转换
                //// https://docs.microsoft.com/en-us/dotnet/api/system.net.ftpwebrequest?view=net-5.0
                //FtpWebRequest req2 = (FtpWebRequest)WebRequest.Create
                //    ("FTP://www.albahari.com/nutshell/code.html");


                //// 需显式转换, URI要支持
                //// https://docs.microsoft.com/en-us/dotnet/api/system.net.filewebrequest?view=net-5.0
                //FileWebRequest req3 = (FileWebRequest)WebRequest.Create
                //    ("file://www.albahari.com/nutshell/code.html");
            }


            req.Proxy = null;
            using (WebResponse res = await req.GetResponseAsync())        //拿到下载内容,从REQUEST得到RESPONSE
            using (System.IO.Stream rs = res.GetResponseStream())         //将RESPONSE换成STREAM
            using (FileStream fs = File.Create("code_async.html"))    //写入FILE STREAM
                await rs.CopyToAsync(fs);
        }

        // ##这是追踪异步的版本
        //static async Task DownloadPageAsync()
        //{
        //    Console.WriteLine($"NO 2,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
        //    WebRequest req = WebRequest.Create
        //        ("http://www.albahari.com/nutshell/code.html");
        //    req.Proxy = null;  //需要验证的话，可用CREDENTIAL属性
        //    using (WebResponse res = await req.GetResponseAsync())
        //    {
        //        Console.WriteLine($"NO 3,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
        //        using (System.IO.Stream rs = res.GetResponseStream())
        //        using (FileStream fs = File.Create("code_async.html"))
        //        {
        //            await rs.CopyToAsync(fs);
        //            Console.WriteLine($"NO 4,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
        //        }

        //    }

        //}


    }
}

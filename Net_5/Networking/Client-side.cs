using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
                    //var client = new HttpClient();
                    //// The GetAsync method also accepts a CancellationToken.
                    //Console.WriteLine($"NO 1,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //HttpResponseMessage response = await client.GetAsync("http://www.linqpad.net");
                    //var header = response.Headers;
                    //Console.WriteLine($"NO 2,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //response.EnsureSuccessStatusCode();
                    //string html = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($"NO 3,  ThreadID : {Thread.CurrentThread.ManagedThreadId}");
                    //html.Dump("html");

                }
                //  uploading Data
                {
                    //HTTPCLIENT带参的初始化， 要一个HttpClientHandler 
                    //var client = new HttpClient(new HttpClientHandler { UseProxy = false });
                    //var request = new HttpRequestMessage(
                    //    HttpMethod.Post, "http://www.albahari.com/EchoPost.aspx");
                    //request.Content = new StringContent("This is a test");
                    //HttpResponseMessage response = await client.SendAsync(request);
                    //response.EnsureSuccessStatusCode();
                    //Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                // Mocking: 在HttpClientHandler上做文章
                // 这个MockHandler CLASS的构成很有水平，要认真读懂用一个FUNC初始化一个OBJECT
                // 这里的FUNC, ACTION其实就是一个LOGIC,即告诉我你要如何做这件事，我将这个LOGIC插入CLASS的某个组件中
                {
                    ////看懂这个LAMBDA表达式，这里定义一个FUNC,当给一个REQUEST时，如何定制RESPOND
                    //var mocker = new MockHandler(request =>
                    //    new HttpResponseMessage(HttpStatusCode.OK)
                    //    {
                    //        Content = new StringContent("You asked for " + request.RequestUri)
                    //    });
                    ////MOCKER继承了HttpMessageHandler，HttpClient用MOCKER初始化后，将使用它的SendAsync进行回复
                    ////从而达到模仿的目的
                    //var client = new HttpClient(mocker);
                    //var response = await client.GetAsync("http://www.linqpad.net");
                    //string result = await response.Content.ReadAsStringAsync();

                    //Assert.AreEqual("You asked for http://www.linqpad.net/", result);

                }
                //TODO: 弄明白这个例子， HANDLER CHAIN的作用
                //Chaining Handler and DelegatingHandler
                {
                    //var mocker = new MockHandler(request =>
                    //    new HttpResponseMessage(HttpStatusCode.OK)
                    //    {
                    //        Content = new StringContent("You asked for " + request.RequestUri)
                    //    });
                    ////看懂这里的关系 MOCKER->LOGGER->CLIENT
                    //var logger = new LoggingHandler(mocker);

                    //var client = new HttpClient(logger);
                    //var response = await client.GetAsync("http://www.linqpad.net");
                    //string result = await response.Content.ReadAsStringAsync();

                    //Assert.AreEqual("You asked for http://www.linqpad.net/", result);
                }
                //TODO: 无法运行
                //HttpClient with progress
                {
                    //var linqPadProgressBar = new LINQPad.Util.ProgressBar("Download progress");

                    //var progress = new Progress<double>();

                    //progress.ProgressChanged += (sender, value) =>
                    //    linqPadProgressBar.Percent = (int)value;

                    //var cancellationToken = new CancellationTokenSource();

                    //using var destination = File.OpenWrite("LINQPad6Setup.exe");
                    //await DownloadFileAsync("https://www.linqpad.net/GetFile.aspx?LINQPad6Setup.exe", destination, progress, default);
                }

                //Use proxy
                {
                    //// 1. WebClient + Proxy
                    //// Create a WebProxy with the proxy's IP address and port. You can
                    //// optionally set Credentials if the proxy needs a username/password.
                    //WebProxy p = new WebProxy("192.178.10.49", 808);
                    //p.Credentials = new NetworkCredential("username", "password");
                    //// or:
                    //p.Credentials = new NetworkCredential("username", "password", "domain");
                    //WebClient wc = new WebClient();
                    //wc.Proxy = p;
                    
                    //// 2. WebRequest + Proxy
                    //// Same procedure with a WebRequest object:
                    //WebRequest req = WebRequest.Create("...");
                    //req.Proxy = p;

                    //// 3. HttpClient + Proxy
                    //// To use a proxy with HttpClient, first create an HttpClientHandler, assign its Proxy
                    //// property, and then feed that into HttpClient’s constructor:

                    //WebProxy pp = new WebProxy("192.178.10.49", 808);
                    //pp.Credentials = new NetworkCredential("username", "password", "domain");
                    //var handler = new HttpClientHandler { Proxy = pp };
                    ////handler.UseProxy = false;  // stop Proxy
                    //var client = new HttpClient(handler);
                }
                //Authentication
                {
                    ////WebClient credential
                    //WebClient wc = new WebClient { Proxy = null };
                    //wc.BaseAddress = "ftp://ftp.myserver.com";
                    //// Authenticate, then upload and download a file to the FTP server.
                    //// The same approach also works for HTTP and HTTPS.
                    //string username = "myuser";
                    //string password = "mypassword";
                    //wc.Credentials = new NetworkCredential(username, password);
                    //wc.DownloadFile("guestbook.txt", "guestbook.txt");
                    //string data = "Hello from " + Environment.UserName + "!\r\n";
                    //File.AppendAllText("guestbook.txt", data);
                    //wc.UploadFile("guestbook.txt", "guestbook.txt");

                    //// HttpClient credential
                    //var handler = new HttpClientHandler();
                    //handler.Credentials = new NetworkCredential(username, password);
                    //var client = new HttpClient(handler);
                }
                //  CredentialCache
                {
                    //CredentialCache cache = new CredentialCache();
                    //Uri prefix = new Uri("http://exchange.somedomain.com");
                    //cache.Add(prefix, "Digest", new NetworkCredential("joe", "passwd"));
                    //cache.Add(prefix, "Negotiate", new NetworkCredential("joe", "passwd"));
                    //WebClient wc = new WebClient();
                    //wc.Credentials = cache;
                }
                // Authenticating via headers with HttpClient
                {
                    //var client = new HttpClient();
                    //client.DefaultRequestHeaders.Authorization =
                    //    new AuthenticationHeaderValue("Basic",
                    //        Convert.ToBase64String(Encoding.UTF8.GetBytes("username:password")));
                }
                // Exception Handling
                {
                    WebClient wc = new WebClient { Proxy = null };
                    try
                    {
                        string s = wc.DownloadString("http://www.albahari.com/notthere");
                    }
                    catch (WebException ex)
                    {
                        if (ex.Status == WebExceptionStatus.NameResolutionFailure)
                            Console.WriteLine("Bad domain name");
                        else if (ex.Status == WebExceptionStatus.ProtocolError)
                        {
                            HttpWebResponse response = (HttpWebResponse)ex.Response;
                            Console.WriteLine(response.StatusDescription);      // "Not Found"
                            if (response.StatusCode == HttpStatusCode.NotFound)
                                Console.WriteLine("Not there!");                  // "Not there!"
                        }
                        else throw;
                    }
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

        static async Task CopyStreamWithProgressAsync(System.IO.Stream input, System.IO.Stream output, long total, IProgress<double> progress, CancellationToken token)
        {
            const int IO_BUFFER_SIZE = 8 * 1024; // Optimal size depends on your scenario

            // Expected size of input stream may be known from an HTTP header when reading from HTTP. Other streams may have their
            // own protocol for pre-reporting expected size.

            var canReportProgress = total != -1 && progress != null;
            var totalRead = 0L;
            byte[] buffer = new byte[IO_BUFFER_SIZE];
            int read;

            while ((read = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                token.ThrowIfCancellationRequested();
                await output.WriteAsync(buffer, 0, read);
                totalRead += read;
                if (canReportProgress)
                    progress.Report((totalRead * 1d) / (total * 1d) * 100);
            }
        }

        static HttpClient client = new HttpClient();
        
        static async Task DownloadFileAsync(string url, System.IO.Stream destination, IProgress<double> progress, CancellationToken token)
        {
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

            if (!response.IsSuccessStatusCode)
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));

            var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;

            using var source = await response.Content.ReadAsStreamAsync();

            await CopyStreamWithProgressAsync(source, destination, total, progress, token);
        }


    }


    class MockHandler : HttpMessageHandler
    {
        Func<HttpRequestMessage, HttpResponseMessage> _responseGenerator;
        //看懂这个CLASS,它要用一个FUNC, 叫responseGenerator来初始化
        //即要给我一个FUNC,告诉我当有一个REQUEST时，如何生成RESPOND
        //有了这个FUNC,这个CLASS就直接插入SendAsync使用
        public MockHandler
            (Func<HttpRequestMessage, HttpResponseMessage> responseGenerator)
        {
            _responseGenerator = responseGenerator;
        }
        //这里设定一个回复机制，即接到请求后，如果回复
        //这个MOCKER被用到HTTPCLIENT中，这个CLIENT 就使用这个机制返值
        protected override Task<HttpResponseMessage> SendAsync
            (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var response = _responseGenerator(request);
            response.RequestMessage = request;
            return Task.FromResult(response);
        }
    }



    class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler nextHandler)
        {
            InnerHandler = nextHandler;//父类DelegatingHandler中的属性，这里把MOCKERHANDLER传进来，作为INNERHANDLER
        }
        //这个LOGGINGHANDLER的SNDASYNC的写法与上面不同
        protected async override Task<HttpResponseMessage> SendAsync
            (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //在父类方法的基础上，加了些内容？
            Console.WriteLine("Requesting: " + request.RequestUri);
            var response = await base.SendAsync(request, cancellationToken);//调用父类DelegatingHandler中的SendAsync方法,它会调用INNER HANDLER
            Console.WriteLine("Got response: " + response.StatusCode);
            return response;
        }
    }

    static class Assert
    {
        public static void AreEqual(object o1, object o2)
        {
            if (!Equals(o1, o2)) throw new Exception("Objects are not equal");
        }
    }


}

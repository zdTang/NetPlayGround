using System;
using System.Diagnostics;

namespace Net_5.Type
{
    class Utility
    {
        public static void Test()
        {
            #region Console

            {
                Console.WindowWidth = Console.LargestWindowWidth;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("test... 50%");
                Console.CursorLeft -= 3;             //将光标前移，这个功能很好
                Console.Write("90%"); // test... 90%

            }
            

            #endregion

            #region Process ProcessStartInfo

            {

                Process.Start("notepad.exe");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c ipconfig /all",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                Process p = Process.Start(psi);
                string result = p.StandardOutput.ReadToEnd();
                Console.WriteLine(result);

                //  ProcessStartInfo

                LaunchFileOrUrl("http://www.albahari.com/nutshell");

                void LaunchFileOrUrl(string url)
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
            }

            #endregion

            #region Environment-Static Class

            {
                //拿到系统信息
                var e = Environment.UserName;
                var b = Environment.MachineName;
            }


            #endregion

            #region AppContext - Static Class

            {
                //拿到当前程序的信息
                var a = AppContext.BaseDirectory;
                var b = AppContext.TargetFrameworkName;
            }

            #endregion
        }
    }
}

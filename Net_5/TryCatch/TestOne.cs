using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TryCatch
{
    class TestOne
    {
        public static void Test()
        {
            // int y = Calc(0);   // will cause error
            // Console.WriteLine(y);
            /*

           try
                        {
                            int y = Calc(0);
                            Console.WriteLine(y);
                        }
                        catch (DivideByZeroException ex)
                        {
                            Console.WriteLine("x cannot be zero");
                        }
                        Console.WriteLine("program completed");

             */

            Try("one");



        }

        // Because Calc is called with x==0, the runtime throws a DivideByZeroException: 

        static int Calc(int x) { return 10 / x; }

        //static int Calc(int x) { return 10 / x; }


        static void Try(params string[] args)
        {
            try
            {
               // byte b = byte.Parse(args[0]);
               // Console.WriteLine(b);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Please provide at least one argument");
            }
            catch (FormatException ex)
            {
                Console.WriteLine("That's not a number!");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine("You've given me more than a byte!");
            }


            try
            {
                new WebClient().DownloadString("http://thisDoesNotExist");
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                Console.WriteLine("Timeout!");
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.NameResolutionFailure)
            {
                Console.WriteLine("Name resolution failure!");
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Some other failure: {ex.Status}");
            }




        }
    }
}

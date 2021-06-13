using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Net_5.TryCatch
{
    class TestThrow
    {
        public static void Test()
        {
            #region Throwing Exception
            {
                //// Exceptions can be thrown either by the runtime or in user code:

                //try
                //{
                //    Display(null);
                //}
                //catch (ArgumentNullException ex)
                //{
                //    Console.WriteLine("Caught the exception");
                //}

                //static void Display(string name)
                //{
                //    if (name == null)
                //        throw new ArgumentNullException(nameof(name));

                //    Console.WriteLine(name);
                //}
            }
            #endregion

            #region Throw Expression
            {
                //// Prior to C# 7, throw was always a statement. Now it can also appear as an expression in
                //// expression-bodied functions:

                //ProperCase("test").Dump();
                //ProperCase(null).Dump();     // throws an ArgumentException
                ////Foo();
                //string Foo() => throw new NotImplementedException();

                //// A throw expression can also appear in a ternary conditional expression:

                //string ProperCase(string value) =>
                //    value == null ? throw new ArgumentException("value") :
                //    value == "" ? "" :
                //    char.ToUpper(value[0]) + value.Substring(1);
            }
            #endregion

            #region Rethrowing an Exception
            {
                //// Rethrowing lets you back out of handling an exception should circumstances turn out to be
                //// outside what you expected:

                //string s = null;

                //using (WebClient wc = new WebClient())
                //   // try { s = wc.DownloadString("http://www.albahari.com/nutshell/"); }
                //    try { s = wc.DownloadString(""); }  //这个会触发THROW 
                //catch (WebException ex)
                //    {
                //        if (ex.Status == WebExceptionStatus.NameResolutionFailure)
                //            Console.WriteLine("Bad domain name");
                //        else
                //            throw;     // Can’t handle other sorts of WebException, so rethrow
                //                       // 如果不想吃到这个EXCEPTION,可以再THROW,这样系统就中断了
                //                       // 如果这里THROW,下面的S.DUMP()就不会执行
                //    }

                //s.Dump("DISPLAY S");    // 这里S拿到的是EXCEPTION
            }
            #endregion

            #region Rethrow
            {
                ////The other common scenario is to rethrow a more specific exception type:

                //DateTime dt;
                //string dtString = "2010-4-31";  // Assume we're writing an XML parser and this is from an XML file
                //try
                //{
                //    // Parse a date of birth from XML element data
                //    dt = XmlConvert.ToDateTime(dtString);
                //}
                //catch (FormatException ex)
                //{
                //    //throw new XmlException("Invalid DateTime", ex); //如果不想吞下这个EXCEPTION,就继续THROW,否则就算自己收下了
                //    Console.WriteLine("very good!");//如果不THROW,程序会平安无事
                //}
            }
            #endregion
        }
    }
}

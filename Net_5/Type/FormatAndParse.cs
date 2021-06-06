using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net_5.Type
{
    class FormatAndParse
    {

        public static void Test()
        {
            #region Format and Parse
            {
                // The simplest formatting mechanism is the ToString method.
                //就是以什么文本形式表示
                string s = true.ToString();


                // Parse does the reverse:
                bool b = bool.Parse(s);
    

                // TryParse avoids a FormatException in case of error:
                int i;
                int.TryParse("qwerty", out i);
                int.TryParse("123", out i);

                if (int.TryParse("123", out int j))
                {

                }

                bool validInt = int.TryParse("123", out int _);
    

                // Culture trap:
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");  // Germany
                double.Parse("1.234");   // 1234 

                // Specifying invariant culture fixes this:
                double.Parse("1.234", CultureInfo.InvariantCulture);

                (1.234).ToString();
                (1.234).ToString(CultureInfo.InvariantCulture);

                // this is a Class other than ValueType Tuple
                var t1 = new Tuple<int, string>(123, "Hello");
            }
            #endregion
        }
    }
}

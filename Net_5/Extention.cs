using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5
{
    // class must be STATIC
    public static class Extention
    {
        // METHOD must be static
        public static void Dump(this object o,string memo=null)
        {
            if (!String.IsNullOrEmpty(memo))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(memo);
                Console.WriteLine("======");
            }
            Console.ForegroundColor = ConsoleColor.White;
                // 对NESTED TYPE需要RECURSION
            switch (o)
            {
                case Array a:
                    foreach (object item in a)
                    {
                        Console.WriteLine((item ?? "null").ToString());
                    }
                    break;
                case IEnumerable t:
                    foreach (object item in t)
                    {
                        Console.WriteLine((item ?? "null").ToString());
                    }
                    break;
                default:
                    Console.WriteLine(o.ToString());
                    break;

            }
            
        }
        //  ForEach used for  c# Array
        public static void ForEach(this Array a, Action<object> action)
        {
            foreach (object item in a)
            {
                action.Invoke(item ?? "null");
            }
        }

    }
}

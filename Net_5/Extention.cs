using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net_5
{
    // class must be STATIC
    public static class Extention
    {
        // METHOD must be static
        public static void Dump(this object o,object memo=null)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            //Type tt = MethodInfo.GetCurrentMethod().GetParameters()[0].ParameterType;//拿到THIS的TYPE
            switch (memo)
            {
                case string a:
                  
                    Console.WriteLine(memo);
                    break;
                default:
                    Console.WriteLine(o.ToString());
                    break;
                    
            } // 对NESTED TYPE需要RECURSION

            Console.WriteLine("=========================");
            Console.ForegroundColor = ConsoleColor.White;

            switch (o)
            {
                case Array a:
                    foreach (object item in a)
                    {
                        Console.WriteLine((item ?? "null").ToString());
                    }
                    break;
                case string s:
                   {
                        Console.WriteLine(s);
                    }
                    break;
                case Type t:
                {
                    Console.WriteLine(t.FullName);
                }
                    break;
                case IEnumerable t:
                    foreach (object item in t)
                    {
                        Console.WriteLine((item ?? "null").ToString());
                    }
                    break;
                case Boolean b:
                    Console.WriteLine(b ? "true":"false");
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

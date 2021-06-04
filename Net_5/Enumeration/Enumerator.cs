using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Enumeration
{
    class Enumerator
	{

        // High-level way of iterating through the characters in the word “beer”:

        public static void Test()
        {
            
            foreach (char c in "beer")
                Console.WriteLine(c);

            // Low-level way of iterating through the same characters:

            using (var enumerator = "beer".GetEnumerator())
                while (enumerator.MoveNext())// start from -1
                {
                    CharEnumerator e = "beer".GetEnumerator();                   
                    var element = enumerator.Current;
                    Console.WriteLine(element);
                }

            foreach (int fib in Fibs(6))
                Console.Write(fib + "  ");

            //  test yield return
            Consumer();

            //  test yield return and yield break
            foreach (string s in Foo(true))
                Console.WriteLine(s);
        }
        static IEnumerable<string> Foo(bool breakEarly)
        {
            yield return "One";
            yield return "Two";

           // if (breakEarly)
           //     yield break;    // break here

            yield return "Three";
        }

        static IEnumerable<int> Fibs(int fibCount)
        {
            for (int i = 0, prevFib = 1, curFib = 1; i < fibCount; i++)
            {
                yield return prevFib;
                int newFib = prevFib + curFib;
                prevFib = curFib;
                curFib = newFib;
            }
        }


        public static void Consumer()
        {
            foreach (int i in Integers())
            {
                Console.WriteLine(i.ToString());
            }
        }

        public static IEnumerable<int> Integers()
        {
            yield return 1;
            yield return 2;
            yield return 4;
            yield return 8;
            yield return 16;
            yield return 16777216;
        }

    }
}

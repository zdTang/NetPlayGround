using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Enumeration
{
    class Iterator
    {
        public static void Test()
        {
            // Test YIELD RETURN
            foreach (int fib in Fibs(6))
                Console.Write(fib + "  ");
            
            // Test YIELD BREAK
            foreach (string s in Foo(true))
                Console.WriteLine(s);
            
            // Multiple yield statements are permitted:

            foreach (string s in Boo())
                Console.WriteLine(s);         // Prints "One","Two","Three"
        }


        // Yield return statement will generate a Iterator
        // The Return type is IEnumerable<T>
        // Foreach can consume IEnumerable<T>
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

        // here to test "YIELD BREAK"
        static IEnumerable<string> Foo(bool breakEarly)
        {
            yield return "One";
            yield return "Two";

            if (breakEarly)
                yield break;    //  BREAK ITERATION

            yield return "Three";
        }

        // here to test "YIELD RETURN"
        static IEnumerable<string> Boo()
        {
            yield return "One";
            yield return "Two";
            yield return "Three";
        }

        // Iterators are highly composable:
        // input a sequence but only output EVEN number
        IEnumerable<int> EvenNumbersOnly(IEnumerable<int> sequence)
        {
            foreach (int x in sequence)
                if ((x % 2) == 0)
                    yield return x; //only yield EVEN number
        }

        // See Chapter 7 for more information on Iterators
    }
}

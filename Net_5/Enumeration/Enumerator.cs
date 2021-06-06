using System;
using System.Collections;
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

            #region Enumerator Foreach

            {
                //String implement IEnumerable<char>,IEnumerable
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
            

            #endregion
            
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
    //interface subClass Interface
    interface IDemo : IEnumerator
    {
        public new int Current { get; } // 用NEW关键字HIDE掉父接口中的OBJECT CURRENT
    }
    //class subClass Interface
    class Demo : IEnumerator
    {
        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
        // 如果CLASS IMPLEMENT INTERFACE, 与接口中的变量TYPE必须相同
        // 这个CURRENT必须是OBJECT的TYPE,这就会有BOXING,UNBOXING的问题
        public object Current { get; }
    }

    class DemoTwo : IDemo
    {
        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
        // 此时用的IDEMO接口，这个CURRENT可以为INT了
        public int Current { get; }

        object IEnumerator.Current => Current;
    }

    //研究接口的继承
    //IEnumerable<char>是个子接口，它重写了父接口中的GetEnumerator方法
    class DemoThree : IEnumerable<char>
    {
        public IEnumerator<char> GetEnumerator()//IMPLICIT 实现子接口的方法，所谓IMPLECIT就是继承链中的默认方法
        {
            throw new NotImplementedException();
        }
        IEnumerator<char> IEnumerable<char>.GetEnumerator()//EXPLICIT实现子接口的方法
        {
            throw new NotImplementedException();
        }
        //这个方法是从IEnumberable这个父接口来的
        //由于子接口已经NEW了方法，因而此时，整个INTERFACE链条中，这个方法的IMPLICIT已经被NEW的方法代替
        //要使用父接口中原来的方法，只能用EXPLICIT即加上父接口的名
        IEnumerator IEnumerable.GetEnumerator()//EXPLICT BASE INTERFACE
        {
            throw new NotImplementedException();
        }
        //这个方法来自于IEnumerable, 但已被IEnumerable<char> New 掉

        //IEnumerator GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

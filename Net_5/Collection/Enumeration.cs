﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Collection
{
    class Enumeration
    {
        public static void Test()
        {
            #region Low Level Enumeration, Non-Generic

            {
                string s = "Hello";
                
                //直接操作Enumerator
                // Because string implements IEnumerable, we can call GetEnumerator():
                IEnumerator rator = s.GetEnumerator();
                
                while (rator.MoveNext())
                {
                    char c = (char)rator.Current;  // 注意，这个CURRENT是OBJECT
                    Console.Write(c + ".");
                }

                Console.WriteLine();

                // Equivalent to: 用FOREACH来操作Enumerator

                foreach (char c in s)
                    Console.Write(c + ".");

            }

            #endregion

            #region Low Level Enumeration, Generic: 没有TYPE转化，应该更快

            {
                IEnumerable<char> s = "Hello";

                using (var rator = s.GetEnumerator())
                    while (rator.MoveNext())
                    {
                        char c = (char)rator.Current;
                        Console.Write(c + ".");
                    }
            }

            #endregion

            #region When to use Non-generic interface
            {
                Count("the quick brown fix".Split());
                static int Count(IEnumerable e)
                {
                    int count = 0;
                    foreach (object element in e) // OBJECT
                    {
                        var subCollection = element as IEnumerable;
                        if (subCollection != null)
                            count += Count(subCollection);
                        else
                            count++;
                    }
                    return count;
                }
            }


            #endregion

            #region Iterator Class

            {
                foreach (int element in new MyCollection())
                    Console.WriteLine(element);
            }

            #endregion

        }
    }
    //三种方式实现IEnumerable
    //
    public class MyCollection : IEnumerable
    {
        int[] data = { 1, 2, 3 };

        public IEnumerator GetEnumerator()
        {
            //foreach (int i in data)        //第二种，YIELD RETURN, 人工方式产生计数器,在利用现成COLLECTION的基础上，
            //  yield return i;              //可以添加一些LOGIC.注意FOREACH在消费一个计数器，同时产生另一个计数器


            //return data.GetEnumerator();   //第一种，返回 COLLECTION的ENUMERATOR（框架的COLLECTION都有自己的计数）
                                             //这种情况很少见，直接用一个现成的COLLECTION

            return new DemoCollecton<int>(); //第三种，自己实施IENUMERATOR接口，生成计数器NUMERATOR
        }
    }
    
    // Generic Collection
    public class MyGenCollection : IEnumerable<int>
    {
        int[] data = { 1, 2, 3 };

        public IEnumerator<int> GetEnumerator()
        {
            foreach (int i in data)
                yield return i;
        }
        // 必须显式实施父接口中的非GENERIC方法
        // 因为这个方法被子接口NEW了，因而只能显式实施
        // 难道只要NEW了一个方法，被NEW的老方法必须显示实现？
        IEnumerator IEnumerable.GetEnumerator()     // Explicit implementation
        {                                           // keeps it hidden.
            return GetEnumerator();
        }
    }

    //我自己IMPLEMENT IENUMERATOR<T>
    class DemoCollecton<T> : IEnumerator<T>
    {
        int[] data = { 1, 2, 3 };//这里不一定用框架的COLLECTION,如果有框加的COLLECTION,就不必多此一举，可直接调用计数器
        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public T Current { get; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }



}

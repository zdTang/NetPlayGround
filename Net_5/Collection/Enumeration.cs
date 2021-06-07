using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Collection
{
    class Enumeration
    {
        //示例了如何构建ENUMERATOR,或实施IENUMERABLE的几种方式
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

            #region Iterator Class  // CLASS继承IEnumerable

            {
                foreach (int element in new MyCollection())
                    Console.WriteLine(element);
            }

            #endregion

            #region Iterator Method  //用METHOD返回Enumerator, 返回类型为IEnumerable<int>

            {
                foreach (int i in TestTwo.GetSomeIntegers())
                    Console.WriteLine(i);
            }

            #endregion

            #region 自己实现Enumerator的方法

            {
                // Non-Generic
                foreach (int i in new MyIntList())
                    Console.WriteLine(i);
                // Generic
                foreach (int i in new MyGenIntList())
                    Console.WriteLine(i);
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
        // 难道只要NEW了一个方法，被NEW的老方法必须显示实现？ 是的！！
        IEnumerator IEnumerable.GetEnumerator()     // Explicit implementation
        {                                           // keeps it hidden.
            return GetEnumerator();
        }
    }

    //我自己IMPLEMENT IENUMERATOR<T>
    //它的DATA可以由上一层传入,用CONSTRUCTOR 构建
    //但GENERIC CLASS不支持GENERIC CONSTRUCT??
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

    public class TestTwo
    {
        // 返回一个实施了IEnumerable<int>接口的东西
        public static IEnumerable<int> GetSomeIntegers()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }
    }

    // OUTER COLLECTION自己实施ENUMERATOR-非GENERIC
    // 通过建一个INNER CLASS(Enumerator)
    public class MyIntList : IEnumerable
    {
        int[] data = { 1, 2, 3 };

        public IEnumerator GetEnumerator() => new Enumerator(this);  //这里，把OUTER COLLECTION把自己的实例传了进去

        //在自己内部定义Enumerator
        class Enumerator : IEnumerator       // Define an inner class
        {                                    // for the enumerator.
            MyIntList collection;
            int currentIndex = -1;

            internal Enumerator(MyIntList collection)//INNER COLLECTION拿到OUTER COLLECTION
            {
                this.collection = collection;
            }

            public object Current  // 非GENERIC的CURRENT是OBJECT，与下面GENERIC的对比
            {
                get
                {
                    if (currentIndex == -1)
                        throw new InvalidOperationException("Enumeration not started!");
                    if (currentIndex == collection.data.Length) //使用OUTER COLLECTION中的DATA
                        throw new InvalidOperationException("Past end of list!");
                    return collection.data[currentIndex];
                }
            }

            public bool MoveNext()
            {
                if (currentIndex >= collection.data.Length - 1) return false;
                return ++currentIndex < collection.data.Length; //使用OUTER COLLECTION中的DATA
            }

            public void Reset() => currentIndex = -1;
        }
    }


    // OUTER COLLECTION自己实施ENUMERATOR-GENERIC
    // 通过建一个INNER CLASS(Enumerator)
    class MyGenIntList : IEnumerable<int>
    {
        int[] data = { 1, 2, 3 };

        // The generic enumerator is compatible with both IEnumerable and
        // IEnumerable<T>. We implement the nongeneric GetEnumerator method
        // explicitly to avoid a naming conflict.

        public IEnumerator<int> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);
        // Enumerator不是GENERIC的，接口是
        // GENERIC CLASS没有带参的CONSTRUCOR
        class Enumerator : IEnumerator<int>
        {
            int currentIndex = -1;
            readonly MyGenIntList collection;

            internal Enumerator(MyGenIntList collection)
            {
                this.collection = collection;
            }

            public int Current => collection.data[currentIndex]; // 非GENERIC的CURRENT是OBJECT
            object IEnumerator.Current => Current; // 但也有一个显式实现的方法，是OBJECT的，注意这里的实现方式，OBJECT调用INT

            public bool MoveNext() => ++currentIndex < collection.data.Length;

            public void Reset() => currentIndex = -1;

            // Given we don't need a Dispose method, it's good practice to
            // implement it explicitly, so it's hidden from the public interface.

            void IDisposable.Dispose() { }
        }
    }

}

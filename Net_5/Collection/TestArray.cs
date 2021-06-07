using System;
using System.Collections;
using System.Linq;
using System.Text;
using static System.Console;

namespace Net_5.Collection
{
    class TestArray
    {
        public static void Test()
        {
            #region Referential and Structural comparison

            {
                object[] a1 = { "string", 123, true };
                object[] a2 = { "string", 123, true };

                Console.WriteLine(a1 == a2);                          // False
                Console.WriteLine(a1.Equals(a2));                    // False
                // Array CLASS都实现了这个接口
                IStructuralEquatable se1 = a1;
                
                // TODO 不理解这儿，但这个可以作为例程，在两个ARRAY间比较，直接使用
                Console.WriteLine(se1.Equals(a2, StructuralComparisons.StructuralEqualityComparer));   // True
                
                // 这个EQUALS方法是ISTRUCTURALEQUATABLE这个接口才有的，因而必须转为这个接口才能用
                //Console.WriteLine(a1.Equals(a2, StructuralComparisons.StructuralEqualityComparer));   // True
                int[] array = new int[5];
                //var pp = Array.CreateInstance(typeof(Int32), 10);
            }

            #endregion

            #region Shallow Clone

            {
                StringBuilder[] builders = new StringBuilder[5];
                builders[0] = new StringBuilder("builder1");
                builders[1] = new StringBuilder("builder2");
                builders[2] = new StringBuilder("builder3");

                StringBuilder[] builders2 = builders;// BUILDERS2是BUILDERS的复制,是REFERENCE的复制，在HEAP上并没有更多的空间分配
                StringBuilder[] shallowClone = (StringBuilder[])builders.Clone();// HEAP上有空间分配了，但是浅COPY

                builders.Dump("builders");
                builders2.Dump("builders2");

                Console.WriteLine(builders);
                Console.WriteLine(builders2);


                Console.WriteLine(builders[0] == builders2[0]);
            }

            #endregion

            #region Construction and Indexing : COMPILER CREATED ARRAY vs RUNTIME CREATED ARRAY

            {
                // Via C#'s native syntax:

                int[] myArray = { 1, 2, 3 };
                int first = myArray[0];
                int last = myArray[myArray.Length - 1];
                
                var num=myArray.GetValue(1); //这两个方法对于COMPILER CREATED ARRAY 和RUNTIME CREATED ARRAY都可用
                myArray.SetValue(88,2);

                // Using GetValue/SetValue:
                // 用ARRAY CLASS可动态在RUNTIME构建
                // 这种方式构建的ARRAY不能用[INDEX]方式取值

                // Create a string array 2 elements in length:
                Array a = Array.CreateInstance(typeof(string), 2);
                
                a.SetValue("hi", 0);                             //  → a[0] = "hi";
                a.SetValue("there", 1);                          //  → a[1] = "there";
                a.Dump("Take a look");
                string s = (string)a.GetValue(0);               //  → s = a[0];
                s.Dump();
                //var b=a[0]; 这种方式构建的ARRAY不能用[INDEX]方式取值

                
                // We can also cast to a C# array as follows: 可以转为C#方式
                string[] cSharpArray = (string[])a;
                string s2 = cSharpArray[0];
                s2.Dump();
            }

            #endregion

            #region Print the first element of array
            // 理解ARRAY中RATE的使用,以及用GetValue(indexers)取出任何值
            {
                int[] oneD = { 1, 2, 3 };
                int[,] twoD = { { 5, 6 }, { 8, 9 } };

                WriteFirstValue(oneD);   // 1-dimensional; first value is 1
                WriteFirstValue(twoD);   // 2-dimensional; first value is 5
            }

            #endregion

            #region Enumeration 三种方式，第三种是自己写一个用在ARRAY实例上的FOREACH方法

            {
                //普通方式，ARRAY实现了IENUMERABLE接口
                int[] myArray = { 1, 2, 3 };
                foreach (int val in myArray)
                    Console.WriteLine(val);

                // 这个用的是ARRAY这个CLASS自带的STATIC 方法
                // Alternative: 第二个参数传入一个ACTION
                // 本例直接传入ACTION一个方法名
                Array.ForEach(new[] { 1, 2, 3 }, WriteLine);
                int[] myArrayT = { 1, 2, 3 };
                // I WROTE A EXTENSION FUNCITON自己动手写了一个FOREACH, 可在ARRAY实例上直接调用
                myArrayT.ForEach(WriteLine);


            }

            #endregion

            #region Searching

            {
                string[] names = { "Rodney", "Jack", "Jill", "Jane" };

                Array.Find(names, n => n.Contains("a")).Dump("Find"); // Returns first matching element
                Array.FindAll(names, n => n.Contains("a")).Dump("FindAll");  // Returns all matching elements

                // Equivalent in LINQ:

                names.FirstOrDefault(n => n.Contains("a")).Dump("Linq=FirstOrDefault");
                names.Where(n => n.Contains("a")).Dump("Linq=Where");



                string match = Array.Find(names, ContainsA);
                //另一种写法 DELEGATE=ANONYMOUS FUNCTION
                string matchTwo = Array.Find(names, delegate (string name)
                    { return name.Contains("a"); });
                //第三种写法 LAMBDA
                string matchThree = Array.Find(names, name => name.Contains("a"));
                Console.WriteLine(match); // Jack
                
            }

            #endregion

            #region Sorting

            {
                int[] numbers = { 3, 2, 1 };
                Array.Sort(numbers);
                numbers.Dump("Simple sort");

                numbers = new[] { 3, 2, 1 };
                string[] words = { "three", "two", "one" };
                Array.Sort(numbers, words);//可以同时SORT多个
                numbers.Dump("Parallel sort==members");
                words.Dump("Parallel sort==words");
                //new { numbers, words }.Dump("Parallel sort");

                // Sort such that odd numbers come first:
                int[] numbersTwo = { 1, 2, 3, 4, 5 };
                // 传入比较方法COMPARISON
                Array.Sort(numbersTwo, (x, y) => x % 2 == y % 2 ? 0 : x % 2 == 1 ? -1 : 1);
                numbersTwo.Dump();
            }

            #endregion

            #region Reverse, Copying, Conversion

            {
                float[] reals = { 1.3f, 1.5f, 1.8f };
                int[] wholes = Array.ConvertAll(reals, r => Convert.ToInt32(r));

                wholes.Dump("Real to Int");
            }

            #endregion


        }
        // 这个例程可用来访问第一个值
        // This works for arrays of any rank
        static void WriteFirstValue(Array a)
        {
            Console.Write(a.Rank + "-dimensional; ");

            // The indexers array will automatically initialize to all zeros, so
            // passing it into GetValue or SetValue will get/set the zero-based
            // (i.e., first) element in the array.
            // TRICK在这里，如果RANK是N, 这个INDEXERS就是【0，0，0，0，0】，总是取到第一个元素
            int[] indexers = new int[a.Rank];
            Console.WriteLine("First value is " + a.GetValue(indexers));
        }
        static bool ContainsA(string name) { return name.Contains("a"); }
    }
}

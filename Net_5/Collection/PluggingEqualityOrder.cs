using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Net_5.Collection
{
    class PluggingEqualityOrder
    {
        public static void Test()
        {
            #region EqualityComparer
            {
                Customer c1 = new Customer("Bloggs", "Joe");
                Customer c2 = new Customer("Bloggs", "Joe");

                Console.WriteLine(c1 == c2);               // False
                Console.WriteLine(c1.Equals(c2));         // False

                var d = new Dictionary<Customer, string>();
                d[c1] = "Joe";
                Console.WriteLine(d.ContainsKey(c2));         // False
                
                var eqComparer = new LastFirstEqComparer();     //要创建一个COMPARER
                
                d = new Dictionary<Customer, string>(eqComparer); //建DICTIONARY时，传入这个COMPARER
                
                d[c1] = "Joe";                           //由于比较的是名和姓，这次C1,C2被认为是相同的
                Console.WriteLine(d.ContainsKey(c2));         // True
            }

            #endregion

            #region IComparer and Comparer

            {
                var wishList = new List<Wish>();
                wishList.Add(new Wish("Peace", 2));
                wishList.Add(new Wish("Wealth", 3));
                wishList.Add(new Wish("Love", 2));
                wishList.Add(new Wish("3 more wishes", 1));
                //在SORT时，只需传入创建的COMPARER就可以了
                wishList.Sort(new PriorityComparer());
                wishList.Dump();
            }

            #endregion

            #region Surname Comparer

            {
                //为SORTEDDICTIONARY导入比较器
                var dic = new SortedDictionary<string, string>(new SurnameComparer());
                dic.Add("MacPhail", "second!");
                dic.Add("MacWilliam", "third!");
                dic.Add("McDonald", "first!");
                dic.Dump();
            }

            #endregion

            #region StringComparer

            {
                //StringComparer提前预制的一些比较场景，会用就好，它的不同STATAIC PROPERTY就是几种默认OBJECT
                //这个比较器不分大小，认为是相等的
                //很多COMPARAR可用，直接看说明，调用即可
                //EQUTABLE是用在非SORTED COLLECTION中，
                var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                dict["joe"] = 12345;
                dict["JOE"].Dump();


                // CultureInfo应该不同的语言背景
                string[] names = { "Tom", "HARRY", "sheila" };
                CultureInfo ci = new CultureInfo("en-AU");
                Array.Sort<string>(names, StringComparer.Create(ci, false));
                names.Dump();
            }

            #endregion

            #region Culture-Aware Surname Comparer

            {
                var dic = new SortedDictionary<string, string>(new SurnameComparerNEW(CultureInfo.GetCultureInfo("de-DE")));
                dic.Add("MacPhail", "second!");
                dic.Add("MacWilliam", "third!");
                dic.Add("McDonald", "first!");
                dic.Dump();
            }

            #endregion

            #region StructuralEquatable and StructuralComparable
            //弄清两件事，一是EQUALITY COMPARER,用于在非SORTED COLLECTON中使用
            //另一类是COMPARER, 用于比较大小，用于SORT排序
            {
                {
                    int[] a1 = { 1, 2, 3 };
                    int[] a2 = { 1, 2, 3 };
                    IStructuralEquatable se1 = a1;
                    Console.WriteLine(a1.Equals(a2));                                  // False
                    Console.WriteLine(se1.Equals(a2, EqualityComparer<int>.Default));  // True
                }
                {
                    string[] a1 = "the quick brown fox".Split();
                    string[] a2 = "THE QUICK BROWN FOX".Split();
                    IStructuralEquatable se1 = a1;
                    bool isTrue = se1.Equals(a2, StringComparer.InvariantCultureIgnoreCase);
                }
                {
                    var t1 = Tuple.Create(1, "foo");
                    var t2 = Tuple.Create(1, "FOO");
                    IStructuralEquatable se1 = t1;
                    Console.WriteLine(se1.Equals(t2, StringComparer.InvariantCultureIgnoreCase));     // True
                    IStructuralComparable sc1 = t1;
                    Console.WriteLine(sc1.CompareTo(t2, StringComparer.InvariantCultureIgnoreCase));  // 0
                }
                {
                    var t1 = Tuple.Create(1, "FOO");
                    var t2 = Tuple.Create(1, "FOO");
                    Console.WriteLine(t1.Equals(t2));   // True
                }

            }

            #endregion


        }
    }

    public class Customer
    {
        public string LastName;
        public string FirstName;

        public Customer(string last, string first)
        {
            LastName = last;
            FirstName = first;
        }
    }
    //EqualityComparer<T>是系统提供的，已实施IEQUALITYCOMPARER<T>等的接口，只需OVERRIDE下面两个就行
    /*
            
            public abstract bool Equals(T? x, T? y);
            public abstract int GetHashCode([DisallowNull] T obj);
            public static EqualityComparer<T> Default { get; }    //已实现
            int IEqualityComparer.GetHashCode(object obj);        //已实现
            bool IEqualityComparer.Equals(object x, object y);    //已实现
            protected EqualityComparer();                         //已实现(parameterless constructor)
     */

    //这个CLASS是为CUSTOMER定制的平等比较器EUALITY COMPARER
    public class LastFirstEqComparer : EqualityComparer<Customer>
    {
        //将这个CLASS实例后相等的条件改为：实例的名，姓相等，则认为这两个实例相等
        public override bool Equals(Customer x, Customer y)
            => x.LastName == y.LastName && x.FirstName == y.FirstName;

        public override int GetHashCode(Customer obj)
            => (obj.LastName + ";" + obj.FirstName).GetHashCode();
    }

    class Wish
    {
        public string Name;
        public int Priority;

        public Wish(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }
    }
    //比较的是次序，而不是相等，需要专门为某个CLASS定制
    class PriorityComparer : Comparer<Wish>
    {
        public override int Compare(Wish x, Wish y)
        {
            if (object.Equals(x, y)) return 0;          // Fail-safe check
            return x.Priority.CompareTo(y.Priority);
        }
    }

    class SurnameComparer : Comparer<string>
    {
        string Normalize(string s)
        {
            s = s.Trim().ToUpper();
            if (s.StartsWith("MC")) s = "MAC" + s.Substring(2);
            return s;
        }
        //这里重新定义比较规则
        public override int Compare(string x, string y)
            => Normalize(x).CompareTo(Normalize(y));
    }

    class SurnameComparerNEW : Comparer<string>
    {
        StringComparer strCmp;

        public SurnameComparerNEW(CultureInfo ci)
        {
            // Create a case-sensitive, culture-sensitive string comparer
            strCmp = StringComparer.Create(ci, false);
        }

        string Normalize(string s)
        {
            s = s.Trim();
            if (s.ToUpper().StartsWith("MC")) s = "MAC" + s.Substring(2);
            return s;
        }

        public override int Compare(string x, string y)
        {
            // Directly call Compare on our culture-aware StringComparer
            return strCmp.Compare(Normalize(x), Normalize(y));
        }
    }
}

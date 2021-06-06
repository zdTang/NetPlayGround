using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Comparison
{
    class EqualityComparison
    {

        public static void Test()
        {
            #region Comparison 比较两个值是否相同——VALUE TYPE REFERENCE TYPE
            {
                // Simple value equality:
                int x = 5, y = 5;
                Console.WriteLine(x == y);   // True (by virtue of value equality)

                // A more elaborate demonstration of value equality:
                var dt1 = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.FromHours(8));
                var dt2 = new DateTimeOffset(2010, 1, 1, 2, 1, 1, TimeSpan.FromHours(9));
                Console.WriteLine(dt1 == dt2);   // True (same point in time)

                // Referential equality:
                Foo f1 = new Foo { X = 5 };
                Foo f2 = new Foo { X = 5 };
                Console.WriteLine(f1 == f2);   // False (different objects)
               


                var result =f1.Equals(f2);      // 查看源码，发现OBJECT的EQUALS中用的是==判断，这个需要OVERRIDE才能对某一个CLASS适用

                Foo f3 = f1;
                Console.WriteLine(f1 == f3);   // True (same objects)

                // Customizing classes to exhibit value equality:
                Uri uri1 = new Uri("http://www.linqpad.net");
                Uri uri2 = new Uri("http://www.linqpad.net");
                Console.WriteLine(uri1 == uri2); // True
                // 这个==被重写了，重写过程中又引用了uri的EQUALS方法（这个方法极其复杂，比较一些VALUE细节）

                #region == has been overriden [Source Code]
                /*
                    public static bool operator ==(Uri? uri1, Uri? uri2)
                    {
                      if ((object) uri1 == (object) uri2)
                        return true;
                      return (object) uri1 != null && (object) uri2 != null && uri1.Equals((object) uri2);
                    }

                    public static bool operator !=(Uri? uri1, Uri? uri2)
                    {
                      if ((object) uri1 == (object) uri2)
                        return false;
                      return (object) uri1 == null || (object) uri2 == null || !uri1.Equals((object) uri2);
                    }
                                 *
                 */

                #endregion

            }
            #endregion

            #region Value and Reference
            {
                {
                    int x = 5;
                    int y = 5;
                    Console.WriteLine(x == y);      // True
                }
                {
                    object x = 5;
                    object y = 5;
                    Console.WriteLine(x == y);      // False  引用类型==比的是引用地址
                }

            }
            #endregion

            #region Leverage OBJECT'S Virtual Equal()
            {
                //这里要理解虽然用的OBJECT,但本质上X,Y都是VALUE.因而比较的是VALUE
                // Here's an example of how we can leverage the virtual Equals mehtod:
                object x = 5; // 这里的X,Y只是戴上帽子而已
                object y = 5;
                //这里需要理解，==是STATIC的，在COMPILE时期, Compiler就会根据X,Y的类型，决定是值比较还是引用比较，因而这里决定进行REFERENTIAL COMPARISON
                var result = (x == y);
                // 用EQUALS这个方法可以避免上述问题，它在RUNTIME期间，将OBJECT转化为具体的类型，这就避免了==，或！=静态的根据帽子决定比较方式的情况
                Console.WriteLine(x.Equals(y));      // True
                /*   虽然这里X,Y是OBJECT，但到了RUNTIME,都会被转为INT32
                这里涉及对EQUALS在RUNTIME时动作的理解，下列来自BOOK, NUTSHELL
                Equals is resolved at runtime—according to the object’s actual type. 
                In this case, it calls Int32’s Equals method,
                1, which applies value equality to the operands, returning true.
                2, With reference types, Equals performs referential equality comparison by default;
                3, with structs, Equals performs structural comparison by calling Equals on each of its fields

                 *
                 */

                Console.WriteLine(AreEqual(x, y));    // True

                Console.WriteLine(AreEqual(null, null));    // True

                // 利用EQUALS的例子，写法很清致，可以避免NULL EXCEPTION
                // 这个例子可以直接抄着用
                bool AreEqual(object obj1, object obj2)
                {
                    if (obj1 == null) return obj2 == null;
                    return obj1.Equals(obj2);
                    // What we've written is in fact equivalent to the static object.Equals method!
                }
                //或写成这样
                static bool AreEqualTwo(object obj1, object obj2)
                    => obj1 == null ? obj2 == null : obj1.Equals(obj2);
            }
            #endregion

            #region  利用好OBJECT的STATIC EQUALS,这个自带NULL验证
            {
                // OBJECT自带两个EQUALS, 一个是VIRTUAL,可用来被重写，另一个是STATIC的，要两个参数
                // Here's how we can use object.Equals:
                object x = 3, y = 3;
                Console.WriteLine(object.Equals(x, y));   // True
                x = null;
                Console.WriteLine(object.Equals(x, y));   // False
                y = null;
                Console.WriteLine(object.Equals(x, y));   // True

                #region static Equals 源码
                //在RUNTIME时还是会转换为真实的TYPE进行验证，调用该TYPE独有的比较方式，
                //这个OBJECT的STATIC EQUALS相比于那个VIRTUAL EQUALS，多了NULL 安全
                {
                    /*
                       public static bool Equals(object? objA, object? objB)
                        {
                          if (objA == objB)//根据真实TYPE进行比较，两个NULL也是可以相等的
                            return true;
                          return objA != null && objB != null && objA.Equals(objB); //处理NULL的情况
                        }
                     */
                }

                #endregion
            }
            #endregion

            #region ReferenceEquals
            //==，或 EQUALS等方法可能会被具体的CLASS，如WIDGET来OVERRIDE重新了定义逻辑，结果返回TRUE
            //此时用这个方法可以获得真实结果

            {
                Widget w1 = new Widget();
                Widget w2 = new Widget();
                Console.WriteLine(object.ReferenceEquals(w1, w2));     // False
            }

            #endregion

            #region Interface

            {
                new TestNewOne<int>().IsEqual(3, 3);
            }

            #endregion

            #region Equals and ==

            {
                // With value types, it's quite rare:
                double x = double.NaN;
                Console.WriteLine(x == x);            // False
                Console.WriteLine(x.Equals(x));      // True

                // With reference types, it's more common:
                var sb1 = new StringBuilder("foo");
                var sb2 = new StringBuilder("foo");
                Console.WriteLine(sb1 == sb2);          // False (referential equality)
                Console.WriteLine(sb1.Equals(sb2));     // True  (value equality)

                /* StringBuilder的EQUALS被重写，进行VALUE EQUALITY
                 public bool Equals(StringBuilder? sb)
                    {
                      if (sb == null || this.Length != sb.Length)
                        return false;
                      if (sb == this)
                        return true;
                      StringBuilder stringBuilder1 = this;
                      int index1 = stringBuilder1.m_ChunkLength;
                      StringBuilder stringBuilder2 = sb;
                      int index2 = stringBuilder2.m_ChunkLength;
                      do
                      {
                        --index1;
                        --index2;
                        for (; index1 < 0; index1 = stringBuilder1.m_ChunkLength + index1)
                        {
                          stringBuilder1 = stringBuilder1.m_ChunkPrevious;
                          if (stringBuilder1 == null)
                            break;
                        }
                        for (; index2 < 0; index2 = stringBuilder2.m_ChunkLength + index2)
                        {
                          stringBuilder2 = stringBuilder2.m_ChunkPrevious;
                          if (stringBuilder2 == null)
                            break;
                        }
                        if (index1 < 0)
                          return index2 < 0;
                      }
                      while (index2 >= 0 && (int) stringBuilder1.m_ChunkChars[index1] == (int) stringBuilder2.m_ChunkChars[index2]);
                      return false;
                    }
                 *
                 */
            }

            #endregion

            #region Equality and Cumstom Types

            {
                //仔细看示列，涉及STRUCT的继承，定义，==符号重载
                Area a1 = new Area(5, 10);
                Area a2 = new Area(10, 5);
                Console.WriteLine(a1.Equals(a2));    // True
                Console.WriteLine(a1 == a2);          // True
            }

            #endregion
        }
    }
    class Foo { public int X; }
    // 这个接口本质上就是实现了一个EQUALS方法，这个方法可以返回BOOL
    class Boo<T> : IEquatable<T>
    {
        // 理解OVERRIDE, 虽然OBJECT上有个VIRTUAL EQUALS,但它的参数TYPE是OBJECT,如果用不同的参数类型，就不用OVERRIDE
        public bool Equals(T? other)
        {
            throw new NotImplementedException();
        }

    }


    //  FRAMEWORK中已经自带了很一些EQUALS的方法，可以直接使用
    //这个例子说明如何利用EQUALS方法，只有与原值不同时，才改变状态并触发EVENT
    class Test<T>
    {
        T _value;
        public void SetValue(T newValue)
        {
            //if (newValue!= _value)  // Generic type cannot use == or !==
            //因为对于==，！=是在编译时STATIC 绑定的，那时编译器不清楚T是什么类型，因而报错
            if (!object.Equals(newValue, _value))  // 但却可以用EQUALS
            {
                _value = newValue;
                OnValueChanged();
            }
        }

        protected virtual void OnValueChanged() { /*...*/ }
    }

    // 下例适于GENERIC,避免了BOXING,直接使用框架提供的EQUALITYCOMPARER<T>来比较
    // A more efficient version of the previous method, when you're dealing with generics:

    class NewTest<T>
    {
        T _value;
        public void SetValue(T newValue)
        {
            //EqualityComparer<T>是个ABSTRACT CLSSS
            //Default 是STATIC PROPERTY, 它是一个 EqualityComparer<T>默认实例的引用
            //DEFAULT: public static EqualityComparer<T> Default { [Intrinsic] get; } = (EqualityComparer<T>) ComparerHelpers.CreateDefaultEqualityComparer(typeof (T));
            //可以直接拿这个DEFAULT来用，就不用自己实例了
            if (!EqualityComparer<T>.Default.Equals(newValue, _value))
            {
                _value = newValue;
                OnValueChanged();
            }
        }
        
        
   /*==================================源码的实现方式
         int IComparer.Compare(object x, object y)
	{
		if (x == null)
		{
			if (y != null)
			{
				return -1;
			}
			return 0;
		}
		if (y == null)
		{
			return 1;
		}
		if (x is T && y is T)
		{
			return Compare((T)x, (T)y);
		}
		ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
		return 0;
	}
    =====================================*/

        protected virtual void OnValueChanged() { /*...*/ }
    }

    class Widget
    {
        // Let's suppose Widget overrides its Equals method and overloads its == operator such
        // that w1.Equals (w2) would return true if w1 and w2 were different objects.
        /*...*/
    }
// 注意这个例子，这并不是指这个CLASS要继承IEQUATABLE<T>接口，而是限制T必须是实现这个接口的
    class TestNewOne<T> where T : IEquatable<T>
    {
        public bool IsEqual(T a, T b) => a.Equals(b);     // No boxing with generic T
    }
    //效果不如上面这个，如果T是个没有继承IEQUATABLE接口的，T身上就没有EQUALS方法
    //就用调用OBJECT的public virtual bool Equals(object? obj);
    //但对于VALUE类型，就有个BOXING的动作，直到RUNTIME才UNBOXING
    class TestNewTWO<T> 
    {
        public bool IsEqual(T a, T b) => a.Equals(b);    
    }


    public struct Area : IEquatable<Area>
    {
        public readonly int Measure1;
        public readonly int Measure2;

        public Area(int m1, int m2)
        {
            Measure1 = Math.Min(m1, m2);
            Measure2 = Math.Max(m1, m2);
        }

        public override bool Equals(object other)   // Override OBJECT'S EQUALS 只对OBJECT 进行定义
        {
            if (!(other is Area)) return false;
            return Equals((Area)other);        // Calls method below
        }

        public bool Equals(Area other)            // Implements IEquatable<Area>这里继承接口
            => Measure1 == other.Measure1 && Measure2 == other.Measure2;

        public override int GetHashCode()//  EQUALS如果OVERRIDE,则这里要重写
            => HashCode.Combine(Measure1, Measure2);


        // == != RELOAD
        public static bool operator ==(Area a1, Area a2) => a1.Equals(a2);

        public static bool operator !=(Area a1, Area a2) => !a1.Equals(a2);
    }
}

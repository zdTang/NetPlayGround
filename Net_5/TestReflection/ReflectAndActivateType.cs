using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestReflection
{
    class ReflectAndActivateType
    {
        public static void Test()
        {
            #region Obtain a Type

            {
                //Type t1 = DateTime.Now.GetType();        // Type obtained at runtime
                //Type t2 = typeof(DateTime);             // Type obtained at compile time
                //Type t3 = typeof(DateTime[]);           // 1-d Array type
                //Type t4 = typeof(DateTime[,]);         // 2-d Array type
                //Type t5 = typeof(Dictionary<int, int>); // Closed generic type
                //Type t6 = typeof(Dictionary<,>);        // Unbound generic type

                //t1.Dump(1);
                //t2.Dump(1);
                //t3.Dump(1);
                //t4.Dump(1);
                //t5.Dump(1);
                //t6.Dump(1);
                //// Assembly有一大堆STATIC 方法
                //// 根据TYPE的名字取回TYPE信息，要加NAMESPACE
                //Type t = Assembly.GetExecutingAssembly().GetType("Foo.Bar");
                //t.Dump(1);
            }

            #endregion

            #region Simple Type Properties

            {
                //Type stringType = typeof(string);

                //string name = stringType.Name;                // String
                //Type baseType = stringType.BaseType;   // typeof(Object)
                //Assembly assem = stringType.Assembly;     // System.Private.CoreLib
                //bool isPublic = stringType.IsPublic;      // true
            }

            #endregion

            #region Obtain array type

            {
                //Type simpleArrayType = typeof(int).MakeArrayType();
                //Console.WriteLine(simpleArrayType == typeof(int[]));     // True

                ////MakeArrayType can be passed an integer argument to make multidimensional rectangular arrays:
                //Type cubeType = typeof(int).MakeArrayType(3);        // cube shaped
                //Console.WriteLine(cubeType == typeof(int[,,]));     // True

                ////GetElementType does the reverse: it retrieves an array type’s element type:
                //Type e = typeof(int[]).GetElementType();      // e == typeof (int)

                ////GetArrayRank returns the number of dimensions of a rectangular array:
                //int rank = typeof(int[,,]).GetArrayRank();   // 3

            }

            #endregion

            #region Obtain nested Type
            //可以找到NESTED TYPE

            {
                //int[] a = {1, 2, 3};
                //Type tt=12.GetType();
                //Type ttt = a.GetType();
                //Type ns= typeof(System.Environment);
                //foreach (Type t in typeof(System.Environment).GetNestedTypes())
                //    Console.WriteLine(t.FullName);

                //// The CLR treats a nested type as having special “nested” accessibility levels:
                //Type sf = typeof(System.Environment.SpecialFolder);
                //Console.WriteLine(sf.IsPublic);                      // False
                //Console.WriteLine(sf.IsNestedPublic);                // True
            }

            #endregion

            #region Type Names

            {
                //{
                //    Type t = typeof(System.Text.StringBuilder);
                //    Type tt= t.GetType();   //拿到TYPE CLASS的信息

                //    Console.WriteLine(t.Namespace);      // System.Text
                //    Console.WriteLine(t.Name);           // StringBuilder
                //    Console.WriteLine(t.FullName);       // System.Text.StringBuilder
                //}

                //// Nested type names
                //{
                //    Type t = typeof(System.Environment.SpecialFolder);

                //    Console.WriteLine(t.Namespace);      // System
                //    Console.WriteLine(t.Name);           // SpecialFolder
                //    Console.WriteLine(t.FullName);       // System.Environment+SpecialFolder
                //}

                //// Generic type names
                //{
                //    Type t = typeof(Dictionary<,>);   // Unbound
                //    Console.WriteLine(t.Name);        // Dictionary'2
                //    Console.WriteLine(t.FullName);    // System.Collections.Generic.Dictionary'2
                //    Console.WriteLine(typeof(Dictionary<int, string>).FullName);
                //}

                //// Array and pointer type names
                //{
                //    Console.WriteLine(typeof(int[]).Name);        // Int32[]
                //    Console.WriteLine(typeof(int[,]).Name);      // Int32[,]
                //    Console.WriteLine(typeof(int[,]).FullName);  // System.Int32[,]
                //}

                //// Pointer types
                //Console.WriteLine(typeof(byte*).Name);     // Byte*

                //// ref and out parameter type names
                //int x = 3;
                //RefMethod(ref x);
            }

            #endregion

            #region IsInstanceOf and IsAssignableForm

            {
                //object obj = Guid.NewGuid();
                //Type target = typeof(IFormattable);

                //bool isTrue = obj is IFormattable;             // Static C# operator
                //bool alsoTrue = target.IsInstanceOfType(obj);   // Dynamic equivalent

                //Debug.Assert(isTrue);
                //Debug.Assert(alsoTrue);
                //var p = new PP();
                //var d = new DD();

                //Type target2 = typeof(IComparable), source = typeof(string);
                ////要注意顺序，这里应该是  SOURCE IsAssignableFrom of Target2
                ////或者是否SOURCE 可以赋值给TARGET2
                //Console.WriteLine(target2.IsAssignableFrom(source));         // True
                ////是否TARGET2 继承自SOURCE
                //Console.WriteLine(target2.IsSubclassOf(source));             // False
                //Console.WriteLine(source.IsSubclassOf(target2));             // false 看来继承INTERFACE，不能说是SUBCLASS
                //Console.WriteLine(d.GetType().IsSubclassOf(p.GetType()));  // True
            }

            #endregion

            #region Instantiating Types

            {
                // 利用TYPE信息构造实例
                {
                    //int i = (int)Activator.CreateInstance(typeof(int));

                    //DateTime dt = (DateTime)Activator.CreateInstance(typeof(DateTime), 2000, 1, 1);
                    //dt.Dump("dt");
                }

                // 取出特定构造函数
                {
                    //// Fetch the constructor that accepts a single parameter of type string:
                    //ConstructorInfo ci = typeof(X).GetConstructor(new[] { typeof(string) });

                    //// Construct the object using that overload, passing in null:
                    //object foo = ci.Invoke(new object[] { null });
                    //foo.Dump();
                }
                //  ARRAYS
                //  STATIC CreateDelegate的源码
                //  代码有问题
                {
                    ///*
                    // public static Delegate? CreateDelegate(
                    //      Type type,
                    //      Type target,
                    //      string method,
                    //      bool ignoreCase,
                    //      bool throwOnBindFailure)
                    //    {
                    //      if (type == (Type) null)
                    //        throw new ArgumentNullException(nameof (type));
                    //      if (target == (Type) null)
                    //        throw new ArgumentNullException(nameof (target));
                    //      if (target.ContainsGenericParameters)
                    //        throw new ArgumentException(SR.Arg_UnboundGenParam, nameof (target));
                    //      if (method == null)
                    //        throw new ArgumentNullException(nameof (method));
                    //      if (!(type is RuntimeType type1))
                    //        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (type));
                    //      if (!(target is RuntimeType methodType))
                    //        throw new ArgumentException(SR.Argument_MustBeRuntimeType, nameof (target));
                    //      Delegate delegate1 = type1.IsDelegate() ? (Delegate) Delegate.InternalAlloc(type1) : throw new ArgumentException(SR.Arg_MustBeDelegate, nameof (type));
                    //      Delegate delegate2 = delegate1;
                    //      string method1 = method;
                    //      int num = 5 | (ignoreCase ? 32 : 0);
                    //      if (delegate2.BindToMethodName((object) null, methodType, method1, (DelegateBindingFlags) num))
                    //        return delegate1;
                    //      if (throwOnBindFailure)
                    //        throw new ArgumentException(SR.Arg_DlgtTargMeth);
                    //      return (Delegate) null;
                    //    }
                    // */

                    ////CreateDelegate 是DELEGATE的一个STATIC 方法

                    //Delegate staticD = Delegate.CreateDelegate
                    //    (typeof(IntFunc), typeof(Program), "Square");

                    //Delegate instanceD = Delegate.CreateDelegate
                    //    (typeof(IntFunc), new Program(), "Cube");

                    //Console.WriteLine(staticD.DynamicInvoke(3));      // 9
                    //Console.WriteLine(instanceD.DynamicInvoke(3));    // 27

                    //IntFunc f = (IntFunc)staticD;
                    //Console.WriteLine(f(3));         // 9 (but much faster!)
                }

                {
                    Type closed = typeof(List<int>);
                    List<int> list = (List<int>)Activator.CreateInstance(closed);  // OK

                    Type unbound = typeof(List<>);
                    try
                    {
                        object anError = Activator.CreateInstance(unbound);    // Runtime error
                    }
                    catch (Exception ex)
                    {
                        ex.Dump("You cannot instantiate an unbound type");
                    }

                    // The MakeGenericType method converts an unbound into a closed generic type.
                    closed = unbound.MakeGenericType(typeof(int));

                    //The GetGenericTypeDefinition method does the opposite:
                    Type unbound2 = closed.GetGenericTypeDefinition();  // unbound == unbound2

                    // The IsGenericType property returns true if a Type is generic, and the
                    // IsGenericTypeDefinition property returns true if the generic type is unbound.
                    // The following tests whether a type is a nullable value type:
                    Type nullable = typeof(bool?);
                    Console.WriteLine(
                        nullable.IsGenericType &&
                        nullable.GetGenericTypeDefinition() == typeof(Nullable<>));   // True

                    //GetGenericArguments returns the type arguments for closed generic types:
                    Console.WriteLine(closed.GetGenericArguments()[0]);     // System.Int32
                    Console.WriteLine(nullable.GetGenericArguments()[0]);   // System.Boolean

                    // For unbound generic types, GetGenericArguments returns pseudotypes that
                    // represent the placeholder types specified in the generic type definition:
                    Console.WriteLine(unbound.GetGenericArguments()[0]);      // T
                }
            }

            #endregion
        }

        public static void RefMethod(ref int p)
        {
            //这个厉害，可以取METHOD的信息
            Type t = MethodInfo.GetCurrentMethod().GetParameters()[0].ParameterType;
            Console.WriteLine(t.Name);    // Int32&
        }

        delegate int IntFunc(int x);

        static int Square(int x) => x * x;        // Static method
        int Cube(int x) => x * x * x;    // Instance method

    }

    class PP { }

    class DD : PP { }
    class X
    {
        public X(string s)
        {
        }

        public X(StringBuilder sb)
        {
        }
    }

}

namespace Foo
{
    public class Bar
    {
        public int Baz;
    }
}
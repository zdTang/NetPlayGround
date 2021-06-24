using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestReflection
{
    delegate string StringToString(string s);
    class ReflectingAndInvokingMember
    {
        public int TestOne { get; init; }//不可以重名，这更证明PROPERTY与METHOD是一种东西
        public static void Test()
        {
            #region Getting Public members
            {
                //MemberInfo[] members = typeof(Walnut).GetMembers();
                //var a=typeof(Walnut).GetMethods();
                ////var b=typeof(Walnut).GetConstructor(Type[]s);
                //var c = typeof(Walnut).GetEvents();
                //var d = typeof(Walnut).GetFields();
                //var f = typeof(Walnut).GetProperties();
                //var g = typeof(Walnut).GetNestedTypes();
     
                //TypeInfo info=typeof(Walnut).GetTypeInfo();
                //foreach (MemberInfo m in members)
                //    Console.WriteLine(m);
            }
            #endregion
            
            #region Test Dynamic Type
            {

                /*dynamic dd;
                Type t = dd.GetType();
                t.Dump();*/

            }
            #endregion

            #region Declaring Type vs Reflected Type
            {
                //// MethodInfo is a subclass of MemberInfo; see Figure 19-1.

                //MethodInfo test = typeof(Program).GetMethod("ToString");
                //MethodInfo obj = typeof(object).GetMethod("ToString");

                //Console.WriteLine(test.DeclaringType);      // System.Object  // 理解，在WRITELINE()中，它是那个OBJECT作为参数的OVERLOAD,
                //                                            // 因而是OBJECT?
                //Console.WriteLine(obj.DeclaringType);       // System.Object  // 理解，在WRITELINE()中，它是那个OBJECT作为参数的OVERLOAD

                //Console.WriteLine(test.ReflectedType);      // Program         // return the type upon which GetMembers was called
                //Console.WriteLine(obj.ReflectedType);       // System.Object   // return the type upon which GetMembers was called

                //Console.WriteLine(test == obj);             // False
                ////MethodHandle 在一个PROCESS中是唯一的
                //Console.WriteLine(test.MethodHandle == obj.MethodHandle);    // True
                ////MetadataToken 在一个ASSEMBLY 中是唯一的
                //Console.WriteLine(test.MetadataToken == obj.MetadataToken    // True
                //                   && test.Module == obj.Module);
            }
            #endregion

            #region c# members vs CLR members
            //这里要理解C#中有些成员有特殊语法
            //比如，PROPERTY,EVENT,INDEXER其实都是METHOD+FIELD，
            //在CLR中，这些都要翻译过来

            {
                //foreach (MethodInfo mi in typeof(Test).GetMethods())
                //    Console.Write(mi.Name + "  ");
                ////拿到CONSOLE中的TITLE属性，顺次拿出它的GETTER, SETTER
                //PropertyInfo pi = typeof(Console).GetProperty("Title");

                //MethodInfo getter = pi.GetGetMethod();   // get_Title
                //MethodInfo setter = pi.GetSetMethod();   // set_Title
                //MethodInfo[] both = pi.GetAccessors();    // Length==2
                ////T反向从GETTER，拿到它的主人，即TITLE这个属性
                //PropertyInfo p = getter.DeclaringType.GetProperties()
                //    .First(x => x.GetAccessors(true).Contains(getter));

                //Debug.Assert(p == pi);
            }

            #endregion
            
            #region Init-only property
            ////init-only 是C# 9的物性，一旦赋值，相当于READ ONLY
            ////这种机制的底层有一个FLAG,在反射时它有一个特别方法，暂时了解一下即可
            ////init-only property
            {
                ////PropertyInfo pi = this.TestOne.GetType().GetProperty("TestOne");
                //PropertyInfo pi = typeof(ReflectingAndInvokingMember).GetProperty("TestOne");
                //IsInitOnly((pi)).Dump("IsInitOnly");
            }

            #endregion

            #region Generic Members
            // Generic 

            {
                //PropertyInfo unbound = typeof(IEnumerator<>).GetProperty("Current");
                //PropertyInfo closed = typeof(IEnumerator<int>).GetProperty("Current");

                //Console.WriteLine(unbound);   // T Current
                //Console.WriteLine(closed);    // Int32 Current

                //Console.WriteLine(unbound.PropertyType.IsGenericParameter);  // True
                //Console.WriteLine(closed.PropertyType.IsGenericParameter);   // False
            }
            //  范形与非范形
            {
                //// The MemberInfo objects returned from unbound and closed generic types are always distinct
                //// — even for members whose signatures don’t feature generic type parameters:

                //PropertyInfo unbound = typeof(List<>).GetProperty("Count");
                //PropertyInfo closed = typeof(List<int>).GetProperty("Count");

                //Console.WriteLine(unbound);   // Int32 Count
                //Console.WriteLine(closed);    // Int32 Count

                //Console.WriteLine(unbound == closed);   // False

                //Console.WriteLine(unbound.DeclaringType.IsGenericTypeDefinition); // True
                //Console.WriteLine(closed.DeclaringType.IsGenericTypeDefinition); // False
            }

            #endregion

            #region Dynamic Invoke
            //取出这个OBJECT的TYPE中的方法，再来处理这个OBJECT
            //如果提前我知道你是STRING,知道你有LENGTH,则可以直接用
            //但如果我提前不知道你是什么TYPE,我可以查，然后拿到你这个TYPE上配备的方法或属性，来处理你
            {
                //object s = "Hello";
                ////PropertyInfo prop = s.GetType().GetProperty("Length");
                //Type t = s.GetType();  //这里，虽然前面S用的是OBJECT代表，但它是个STRING TYPE
                //PropertyInfo prop = t.GetProperty("Length");
                //int length = (int)prop.GetValue(s, null); // 5    // 拿出这个OBJECT的TYPE中的方法，再来处理这个OBJECT

                //length.Dump();
            }

            #endregion

            #region Method Parameters
            //Method Parameters
            {
                //Type type = typeof(string);
                //Type[] parameterTypes = { typeof(int) };         // 注意这种方法，要把参数放入一个TYPE[]中
                //MethodInfo method = type.GetMethod("Substring", parameterTypes); //然后传到这里，取出带某种参数列表的方法

                //object[] arguments = { 2 };

                ////这里，从一个METHODINFO中来INVOKE方法，是无法提前预知参数类型和返回值类型的
                ////这里的INVOKE方法是一个笼统的方法，都用OBJECT TYPE来代替
                ////因为这个METHODINFO可能是任何情况
                //object returnValue = method.Invoke("stamp", arguments);
                //Console.WriteLine(returnValue);                           // "amp"
                ////取出参数列表内容
                //ParameterInfo[] paramList = method.GetParameters();
                //foreach (ParameterInfo x in paramList)
                //{
                //    Console.WriteLine(x.Name);                 // startIndex
                //    Console.WriteLine(x.ParameterType);        // System.Int32
                //}
            }
            //Method Parameters - ref and out
            {
                //object[] args = { "23", 0 };
                //Type[] argTypes = { typeof(string), typeof(int).MakeByRefType() };     //   做一个参数TYPE的[]
                //MethodInfo tryParse = typeof(int).GetMethod("TryParse", argTypes); //   取出特定的METHOD
                //bool successfulParse = (bool)tryParse.Invoke(null, args);           //   用取出的METHOD处理参数
                //                                                                        //   注意，这里将OBJECT EXPLICIT CAST

                //Console.WriteLine(successfulParse + " " + args[1]);       // True 23

                //int willChange = 9;   // tryParse试着将STRING转为INT,成功的话，直接将OUT的参数改写成转化后的结果
                //bool b=Int32.TryParse("100", out willChange);  //由于"100"可以转为100.因而WILLCHANGE将被改写 

            }
            //  TODO: With Generic Method【有空研究一下】
            {
                //取出某个GENERIC下面的GENERIC方法并不容易，这里取Enumerable中的WHERE方法
                //这两个方法是这样的
                /*
                  public static IEnumerable<TSource> Where<TSource>(
                  this IEnumerable<TSource> source,
                  Func<TSource, bool> predicate);

                  public static IEnumerable<TSource> Where<TSource>(
                  this IEnumerable<TSource> source,
                  Func<TSource, int, bool> predicate);
                 */
                //var unboundMethod = (
                //    from m in typeof(Enumerable).GetMethods()
                //    where m.Name == "Where" && m.IsGenericMethod
                //    let parameters = m.GetParameters()
                //    where parameters.Length == 2
                //    let genArg = m.GetGenericArguments().First()
                //    let enumerableOfT = typeof(IEnumerable<>).MakeGenericType(genArg)
                //    let funcOfTBool = typeof(Func<,>).MakeGenericType(genArg, typeof(bool))
                //    where parameters[0].ParameterType == enumerableOfT
                //          && parameters[1].ParameterType == funcOfTBool
                //    select m).Single();

                //unboundMethod.Dump("Unbound method");

                //var closedMethod = unboundMethod.MakeGenericMethod(typeof(int));

                //int[] source = { 3, 4, 5, 6, 7, 8 };
                //Func<int, bool> predicate = n => n % 2 == 1;   // Odd numbers only

                //// We can now invoke the closed generic method as follows:
                //var query = (IEnumerable<int>)closedMethod.Invoke
                //    (null, new object[] { source, predicate });

                //query.Dump();
            }
            #endregion

            #region Using Delegate for Performance
            //
            {
                //// 取出没有参数的那个方法
                //MethodInfo trimMethod = typeof(string).GetMethod("Trim", new Type[0]);

                //// First let's test the performance when calling Invoke
                //var sw = Stopwatch.StartNew();
                //// 这里，直接通过METHODINFO的INVOKE来启动一个方法的LOGIC
                //// 这个慢， 因为LATE BINDING在LOOP中每次都发生
                //for (int i = 0; i < 1000000; i++)
                //    trimMethod.Invoke("test", null);

                //sw.Stop();
                //sw.Dump("Using Invoke");

                //// Now let's test the performance when using a delegate:
                //// 用DELEGATE快很多
                //// 要看懂下面的写法
                //// 第一个参数表示要做一个这种TYPE的，即delegate string StringToString(string s); 即一个STRING进，一个STRING出
                //// 第二个参数表示要用这个方法的LOGIC来INSTANTIATE这个TYPE
                //// 得到的TRIM是一个DELEGATE, 它的类型与StringToString相同，用的trimMetho来实例
                //// 这个快， 因为LATE BINDING只发生一次，在LOOP中就不发生了
                //var trim = (StringToString)Delegate.CreateDelegate(typeof(StringToString), trimMethod);
                //sw.Restart();

                //for (int i = 0; i < 1000000; i++)
                //    trim("test");

                //sw.Stop();
                //sw.Dump("Using a delegate");
            }

            #endregion

            #region Accessing Non-Public Members

            {
                //Type t = typeof(WalnutTwo);
                //WalnutTwo w = new WalnutTwo();
                //w.Crack();
                ////拿出这个叫"cracked"的非PUBLIC, 非STATIC的字段
                //FieldInfo f = t.GetField("cracked", BindingFlags.NonPublic | BindingFlags.Instance);
                ////如果不设置参数，则拿不到这个字段，下面这种取到NULL
                ////FieldInfo f = t.GetField("cracked");
                //f.SetValue(w, false);
                //Console.WriteLine(w);         // False
            }

            #endregion

            #region Accessing Non-Public Members - Binding Flags

            {
                //BindingFlags publicStatic = BindingFlags.Public | BindingFlags.Static;
                //MemberInfo[] members = typeof(object).GetMembers(publicStatic);
                //members.Dump("Public members", 1);

                //// The following example retrieves all the nonpublic members of type object, both static and instance:
                //BindingFlags nonPublicBinding =
                //    BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

                //MemberInfo[] nonPublic = typeof(object).GetMembers(nonPublicBinding);
                //nonPublic.Dump("Non-public members", 1);
            }

            #endregion

            #region Invoking Generic Methods
            //直接INVOKE GENERIC的方法是不可以的，需要传入一个具体的TYPE,让它具体化才可以运行
            {
                //MethodInfo echo = typeof(ReflectingAndInvokingMember).GetMethod("Echo");
                //Console.WriteLine(echo.IsGenericMethodDefinition);    // True

                //try
                //{
                //    echo.Invoke(null, new object[] { 123 });              // Exception
                //}
                //catch (Exception ex)
                //{
                //    ex.Dump("This can't be done");
                //}
                ////用MakeGenericMethod再包一层，并传入具体的参数的TYPE，就可以将这个GENERIC具体化了
                //MethodInfo intEcho = echo.MakeGenericMethod(typeof(int));
                //Console.WriteLine(intEcho.IsGenericMethodDefinition);            // False
                //Console.WriteLine(intEcho.Invoke(null, new object[] { 3 }));   // 3
            }

            #endregion

            #region Anonymously Calling Members of a Generic Interface
            //TODO:有空研究一下这一通什么风骚操作
            // Reflection威力在于不清楚来的类型是什么，要动态的根据类型来操作

            {
                //Console.WriteLine(ToStringEx(new List<int> { 5, 6, 7 }));
                //Console.WriteLine(ToStringEx("xyyzzz".GroupBy(c => c)));
            }

            #endregion

            #region Reflecting Assemblies

            {
                //// Assembly跟TYPE一样，也是一个CLASS,这里获取到一个实例
                //Assembly currentAssem = Assembly.GetExecutingAssembly();
                //// 从ASSEMBLY实例中中拿到指定TYPE
                //var t = currentAssem.GetType("Demos.TestProgram");
                //t.Dump();
                //// 从ASSEMBLY中拿到所有TYPE
                //var allTypes = currentAssem.GetTypes();
                //allTypes.Dump();
            }

            #endregion 

        }
        public static string ToStringEx(object value)
        {
            if (value == null) return "<null>";
            //获取传入参数的TYPE,如果是PRIMITIVE,则直接调用TOSTRING()
            if (value.GetType().IsPrimitive) return value.ToString();

            // 构建一个空的STRING
            StringBuilder sb = new StringBuilder();
            // 如果来的东西继承了ILIST这个接口
            if (value is IList)
                sb.Append("List of " + ((IList)value).Count + " items: "); //显示有几条内容
            // 从TYPE实例中，拿到INTERFACE
            // 再从INTERFACE中，找到是GENERIC，而且继承IGrouping接口的
            Type closedIGrouping = value.GetType().GetInterfaces()
                .Where(t => t.IsGenericType &&
                            t.GetGenericTypeDefinition() == typeof(IGrouping<,>))
                .FirstOrDefault();

            if (closedIGrouping != null)   // Call the Key property on IGrouping<,>
            {
                PropertyInfo pi = closedIGrouping.GetProperty("Key");
                object key = pi.GetValue(value, null);
                sb.Append("Group with key=" + key + ": ");
            }

            if (value is IEnumerable)
                foreach (object element in ((IEnumerable)value))
                    sb.Append(ToStringEx(element) + " ");

            if (sb.Length == 0) sb.Append(value.ToString());

            return "\r\n" + sb.ToString();
        }
        public static T Echo<T>(T x) { return x; }
        public void TestTwo()
        {
            //init-only property
            //PropertyInfo pi = this.TestOne.GetType().GetProperty("TestOne");
            PropertyInfo pi = typeof(ReflectingAndInvokingMember).GetProperty("TestOne");
            IsInitOnly((pi));
        }
   

        static bool IsInitOnly(PropertyInfo pi) => pi
            .GetSetMethod().ReturnParameter.GetRequiredCustomModifiers()
            .Any(t => t.Name == "IsExternalInit");
    }

    class Test { public int X { get { return 0; } set { } } }
    class Walnut
    {
        private bool cracked;
        public void Crack() { cracked = true; }
    }

    class WalnutTwo
    {
        private bool cracked;
        public void Crack() { cracked = true; }
        public override string ToString() { return cracked.ToString(); }
    }
}

namespace Demos
{
    class TestProgram
    {
    }

    class SomeOtherType
    {
    }
}

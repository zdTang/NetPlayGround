using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.testType
{
    class TestDynamic
    {
		public static void Test()
        {

			// Custom binding occurs when a dynamic object implements IDynamicMetaObjectProvider:
			// WHAT IS BINDING : mapping a name to a specific Method
			// Map Quack ===> solid code 
			// 这里的CUSTOM 可否理解为定制化,见DUCK CLASS
			#region Custom Binding
			{ // Custom binding occurs when a dynamic object implements IDynamicMetaObjectProvider:


				//Calling Quack gives us a compilation error, because although the value stored in d
				//can contain a method called Quack, the compiler cannot know it, because the only
				//information it has is the type of the variable, which in this case is object.
				dynamic d = new Duck();
				//object d = new Duck();    //如果用OBJECT,则编译出错，因为STATIC BINDING要根据OBJECT的TYPE
				                            //而OBJECT这个TYPE中没有QUACK()这个方法，绑不上去
				d.Quack();                  // Quack method was called, will call Duck's TryInvokeMember
				d.Waddle();                 //will call Duck's TryInvokeMember
			}
			#endregion

			#region Language binding
			{
				// Language binding occurs when a dynamic object does not implement IDynamicMetaObjectProvider:

				//int x = 3, y = 4;
				dynamic x = 3, y = 4;
				//这个MEAN()的返回值是个DYNAMIC
				//如果X,Y提前写死，则在COMPILE时，就会知道TYPE
				//如果是在RUNTIME临时写入会如何？
				Console.WriteLine(Mean(x, y));

				static dynamic Mean(dynamic x, dynamic y) => (x + y) / 2;
			}
            #endregion

            #region Dynamic basic
            {
				// The following expression is true, although the compiler does not permit it:
				// typeof CANNOT use on dynamic type
				//var c = (typeof(dynamic) == typeof (object));

				// This principle extends to constructed types and array types:
				var a=(typeof(List<dynamic>) == typeof(List<object>));     // True

				var b=(typeof(dynamic[]) == typeof(object[]));     // True
				//注意这里DYNAMIC 这个帽子很像OBJECT TYPE,像一个ROOT CLASS,什么TYPE都可以代表
				// Like an object reference, a dynamic reference can point to an object of any type (except pointer types):
				dynamic x = "hello";
				Console.WriteLine(x.GetType().Name);  // String

				x = 123;  // No error (despite same variable)
				Console.WriteLine(x.GetType().Name);  // Int32

				// You can convert from object to dynamic to perform any dynamic operation you want on an object:
				object o = new System.Text.StringBuilder();
				//o.Append("mike");//直接用O是不可以的，这成了STATIC BINDING,COMPILER找不到APPEND
				dynamic d = o;
				d.Append("hello");// 转成DYNAMIC就可以随便来了，
				//d.Laugh("hello"); // 写什么都行，但在RUNTIME时，如果D没有这个方法，则出错.这一句一定出错
								  // 到RUNTIME时，会知道D的TYPE, 报错System.Text.StringBuilder' does not contain a definition for 'Laugh''
				Console.WriteLine(o);   // hello
			}
			#endregion

			#region  The dynamic type has implicit conversions to and from all other types:
			{
				// The dynamic type has implicit conversions to and from all other types:
				// 如同OBJECT一样，DYNAMIC 有资格引用一切其它TYPE,只是引用而已

				int i = 7;
				dynamic d = i;
				int j = d;        // Implicit conversion (or more precisely, an *assignment conversion*)



				// The following throws a RuntimeBinderException because an int is not implicitly convertible to a short:
				// 由于D 是 DYNAMIC, 因而在编译时不会报错
				// RUNTIME: Cannot implicitly convert type 'int' to 'short'. An explicit conversion exists (are you missing a cast?)'

				//short s = d;
			}
            #endregion
            
			#region var vs dynamic
            {
				// var says, “let the compiler figure out the type”.
				// dynamic says, “let the runtime figure out the type”.

				dynamic x = "hello";  // Static type is dynamic, runtime type is string
				var y = "hello";      // Static type is string, runtime type is string
				//int i = x;            // Run-time error
				//int j = y;            // Compile-time error
			}
			#endregion
			
			#region static type vs dynamic type
			{
				// The static type of a variable declared of type var can be dynamic:

				dynamic x = "hello";
				var y = x;            // Static type of y is dynamic // VAR的TYPE由COMPILER时认定，因而在COMPILE时发现Y=X是DYNAMIC,拖后处理
				//int z = y;            // Run-time error // 由于Y是DYNAMIC,在COMPILE时并不理会，因而拖到RUNTIME时发现TYPE冲突
			}
            #endregion

            #region static type and dynamic type conversion
            {
				// Trying to consume the result of a dynamic expression with a void return type is
				// prohibited — just as with a statically typed expression. However, the error occurs at runtime:

				dynamic list = new List<int>();   // 用DYNAMIC,这个错误会拖到RUNTIME.
												  // COMPILER没有检查，不知道LIST是什么东西,因而不知它该不该有返回值
				//var list = new List<int>();     // 如果用VAR, 则COMPILER立刻就会发现这个问题
				//var result = list.Add(5);         // RuntimeBinderException thrown

				// Expressions involving dynamic operands are typically themselves dynamic:
				dynamic x = 2;
				var y = x * 3;       // Static type of y is dynamic

				// However, casting a dynamic expression to a static type yields a static expression:
				dynamic a = 2;
				var b = (int)a;      // Static type of b is int

				// And constructor invocations always yield static expressions:
				// Constructor构造的OBJECT一定是STATIC的，而不是DYNAMIC的，无论它的参数是不是DYNAMIC的
				dynamic capacity = 10;
				var sb = new System.Text.StringBuilder(capacity);
				int len = sb.Length;
			}
            #endregion

            #region dynamic arguments function overloading
            {
				// 通常情况下，一般是先有DYNAMIC OBJECT,然后它再CALL DYNAMIC 方法
				// 这种情况是不经过DYNAMIC OBJECT, 直接CALL一个METHOD,但参数是DYNMAIC的
				// You can also call statically known functions with dynamic arguments.
				// Such calls are subject to dynamic overload resolution:

					dynamic x = 5;
					dynamic y = "watermelon";

					Foo(x);                // 1  => will call the Foo which consume the INT
					Foo(y);                // 2  => will call the Foo which consume the String
				
			}
			#endregion

			#region Static types are also used — wherever possible — in dynamic binding
			{
				object o = "hello";
				dynamic d = "goodbye";   //在绑定时，根据实际的TYPE去查找最接近的FOO
				Foo(o, d);               // os
			}
            #endregion

            #region
            {
				// You cannot dynamically call:
				//  • Extension methods (via extension method syntax)
				//  • Any member of an interface
				//  • Base members hidden by a subclass
				

				//这里是不是说，戴什么帽子通过哪个镜子看对象，是在COMPILE时期决定，并绑定的
				//等到了RUNTIME时，这就不好用了。
				//如同本例，如果D不是DYNAMIC,则COMPILE时，就可以定下来绑定哪个TEST了
				//但由于是DYNAMIC的，到了RUNTIME时，要确定D的真实身份，就不管他的帽子了，那是编译期的事情
				//因此，下面书上说， lens is lost at runtime....

				//The implicit cast shown in bold tells the compiler to bind subsequent member calls
				//on f to IFoo rather than Foo, in other words, to view that object through the lens
				//of the IFoo interface. However, that lens is lost at runtime, so the DLR cannot com‐
				//plete the binding.The loss is illustrated as follows:

				IFoo f = new Foo();//这里很TRICKY,F本质是个FOO,但戴上IFOO的帽子，因而它可以用IFOO的方法
								   //然后，此时F转为DYNAMIC, 这个D在RUNTIME时，却引用的是FOO,而不是IFOO
								   //DYNAMIC在最后RUNTIME绑定时，要绑定本质上的那个对象，这里就是FOO
								   //而不是绑定中间那些帽子们TYPE
				dynamic d = f;
				//d.Test();             // Exception thrown
				f.Test();
			}
            #endregion


        }

        static void Foo(int x) { Console.WriteLine("1"); }
		static void Foo(string x) { Console.WriteLine("2"); }
		// Static types are also used — wherever possible — in dynamic binding:
		// Note: the following sometimes throws a RuntimeBinderException in Framework 4.0 beta 2. This is a bug.
		static void Foo(object x, object y) { Console.WriteLine("oo"); }
		static void Foo(object x, string y) { Console.WriteLine("os"); }
		static void Foo(string x, object y) { Console.WriteLine("so"); }
		static void Foo(string x, string y) { Console.WriteLine("ss"); }
	

	}

	interface IFoo { void Test(); }
	class Foo : IFoo { void IFoo.Test() { } }  // 注意这里是EXPLICIT继承，要用IFOO的帽子才可以用这个TEST

	//  实现IDynamicMetaObjectProvider这个接口，就是CUSTOM BINDING
	//  Duck Inherit a DynamicObject
	public class Duck : System.Dynamic.DynamicObject
	{
		// Custom binding occurs when a dynamic object implements IDynamicMetaObjectProvider:
		// 这个DUCK实现了这个接口，意味着当一个DUCK对象被用作DYNAMIC时，当这个对象要调用方法时，DUCK TYPE会用这个接口应对
		// 即当这个DUCK对象被作为DYNAMIC时，无集结调用它的任何方法，都由这个INTERFACE来执行
		public override bool TryInvokeMember(
			InvokeMemberBinder binder, object[] args, out object result)
		{
			Console.WriteLine(binder.Name + " method was called");
			result = null;
			return true;
		}
	}
}

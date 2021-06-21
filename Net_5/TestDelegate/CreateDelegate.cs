using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestDelegate
{
    // Declare three delegate types for demonstrating the combinations
    // of static versus instance methods and open versus closed
    // delegates.
    // 这里要明白一个道理，DELEGATE是一个签名，只规定返回值，参数的类型，不规定具体的逻辑
    public delegate void D1(C c, string s);
    public delegate void D2(string s);
    public delegate void D3();



    class CreateDelegate
    {
        public static void Test()
        {
            C c1 = new C(42);

            // Get a MethodInfo for each method.
           
            MethodInfo mi1 = typeof(C).GetMethod("M1",BindingFlags.Public | BindingFlags.Instance);
            MethodInfo mi2 = typeof(C).GetMethod("M2",BindingFlags.Public | BindingFlags.Static);

            D1 d1;
            D2 d2;
            D3 d3;

            Console.WriteLine("\nAn instance method closed over C.");
            // In this case, the delegate and the
            // method must have the same list of argument types; use
            // delegate type D2 with instance method M1.
            //



            // Summary:
            //     Creates a delegate of the specified type that represents the specified static
            //     or instance method, with the specified first argument and the specified behavior
            //     on failure to bind.
            //
            // Parameters:
            //   type:
            //     A System.Type representing the type of delegate to create.

            //     这里指要建一个 typeof(D2)

            //   firstArgument:
            //     An System.Object that is the first argument of the method the delegate represents.
            //     For instance methods, it must be compatible with the instance type.

            //     c1是个C CLASS的实例

            //   method:
            //     The System.Reflection.MethodInfo describing the static or instance method the
            //     delegate is to represent.
            
            //     mi1是实例METHOD的METHODINFO
            
            //   throwOnBindFailure:
            //     true to throw an exception if method cannot be bound; otherwise, false.
            //
            // Returns:
            //     A delegate of the specified type that represents the specified static or instance
            //     method, or null if throwOnBindFailure is false and the delegate cannot be bound
            //     to method.

            //这里并不是生成了一个DELEGATE的TYPE，而是用一个DELEGETE去启动一个签名相同的方法？？？
            Delegate test = Delegate.CreateDelegate(typeof(D2), c1, mi1, false);
            //     1，这里指要建一个 Delegate的实例，它的TYPE是typeof(D2) // public delegate void D2(string s); D2要一个STRING,没有返回值
            //     2，c1是个C CLASS的实例// 由于是个实例方法，这里要传入一个实例
            //     3，mi1是实例METHOD的METHODINFO， //MI1也是要一个STRING,没有返回值
            // Because false was specified for throwOnBindFailure 
            // in the call to CreateDelegate, the variable 'test'
            // contains null if the method fails to bind (for 
            // example, if mi1 happened to represent a method of  
            // some class other than C).
            //
            if (test != null)
            {
                d2 = (D2)test;

                // The same instance of C is used every time the 
                // delegate is invoked.
                d2("Hello, World!");  //注意， 这个方法是绑定在 c1 这个实例上的，因此，才会有42这个值，它来自于实例  C c1 = new C(42);
                d2("Hi, Mom!");       //注意， 这个方法是绑定在 c1 这个实例上的，因此，才会有42这个值，它来自于实例  C c1 = new C(42);
            }

            Console.WriteLine("\nAn open instance method.");
            // In this case, the delegate has one more 
            // argument than the instance method; this argument comes
            // at the beginning, and represents the hidden instance
            // argument of the instance method. Use delegate type D1
            // with instance method M1.
            //
            d1 = (D1)Delegate.CreateDelegate(typeof(D1), null, mi1);

            // An instance of C must be passed in each time the 
            // delegate is invoked.
            //
            d1(c1, "Hello, World!");
            d1(new C(5280), "Hi, Mom!");

            Console.WriteLine("\nAn open static method.");
            // In this case, the delegate and the method must 
            // have the same list of argument types; use delegate type
            // D2 with static method M2.
            //
            d2 = (D2)Delegate.CreateDelegate(typeof(D2), null, mi2);

            // No instances of C are involved, because this is a static
            // method. 
            //
            d2("Hello, World!");
            d2("Hi, Mom!");

            Console.WriteLine("\nA static method closed over the first argument (String).");
            // The delegate must omit the first argument of the method.
            // A string is passed as the firstArgument parameter, and 
            // the delegate is bound to this string. Use delegate type 
            // D3 with static method M2. 
            //
            d3 = (D3)Delegate.CreateDelegate(typeof(D3),
                "Hello, World!", mi2);

            // Each time the delegate is invoked, the same string is
            // used.
            d3();

        }
    }

    // A sample class with an instance method and a static method.
    //
    public class C
    {
        private int id;
        public C(int id) { this.id = id; }

        public void M1(string s)
        {
            Console.WriteLine("Instance method M1 on C:  id = {0}, s = {1}",
                this.id, s);
        }

        public static void M2(string s)
        {
            Console.WriteLine("Static method M2 on C:  s = {0}", s);
        }
    }

}

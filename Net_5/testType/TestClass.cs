using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Net_5.testType
{
    class TestClass
    {
        public static void Test()
        {
            string a = "mike";
            string b = "mike";
            Console.WriteLine(a==b);    // true

            //两个空对象也不相等
            //这应该是默认的比较方法
            //对于一个具体的OBJECT, 我们要OVERRIDE 这个EQUALS方法，才能定义自己的规则来比较
            //否则对于OJBECT这样比较，是不相等的
            object obj1 = new object();
            object obj2 = new object();
            Console.WriteLine(obj1 == obj2); //false
            Console.WriteLine(obj1.Equals(obj2)); //OJBECT的EQUALS 就是==
        }

    }

    //这个例是就是CLASS中调用自己，熟悉这种用法
    //理解，CLASS是个TYPE
    //真正的每个实例，只是包括了FIELD和PROPERTY（STATE）部分,不要多考虑METHOD,那些只构成LOGIC部分
    //METHOD只是构成了这个TYPE内部的一个逻辑
    //对于实例METHOD,实例自己就是一个参数，需将自己传入（PYTHON中就很直接）
    //对于STATIC METHOD, 没有实例参数
    class Papa
    {
        static private int _age=100;

        private Papa(int age)
        { _age = age;}
        public static Papa Default = new Papa(_age);
    }
    // SEALED,不准别人继承
    //You can also use the sealed modifier on a method or property that overrides a virtual method or property in a base class.
    //This enables you to allow classes to derive from your class and prevent them from overriding specific virtual methods or properties.

    class X
    {
        protected virtual void F() { Console.WriteLine("X.F"); }
        protected virtual void F2() { Console.WriteLine("X.F2"); }
    }

    class Y : X
    {
        sealed protected override void F() { Console.WriteLine("Y.F"); }
        protected override void F2() { Console.WriteLine("Y.F2"); }
    }

    class Z : Y
    {
        // Attempting to override F causes compiler error CS0239.
        //protected override void F() { Console.WriteLine("Z.F"); }

        // Overriding F2 is allowed.
        protected override void F2() { Console.WriteLine("Z.F2"); }
        private MyDelegate a = (i) => { };
    }

    //  DELEGATE

    delegate void MyDelegate(int a);  // 这是定义一个TYPE
   
}

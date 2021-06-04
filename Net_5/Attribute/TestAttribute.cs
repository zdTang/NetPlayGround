using System;

namespace Net_5.Attribute
{
    class TestAttribute
    {
        public static void Test()
        {
            var a =typeof(Foo);
            var b = new Foo();
            var c = b.GetType();
        }

    }


    class Foo
    {
        [field: NonSerialized]
        public int MyProperty { get; set; }
    }
}

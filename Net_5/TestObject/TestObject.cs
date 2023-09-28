using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestObject
{
    public class TestObject
    {
        public static void Test()
        {
            var a = new myObject { Name = "Mike" };
            var b = a.GetHashCode();
            var c = a.GetType();
            var d = a.ToString();
            var anotherA = a;
            var ifSame = a.Equals(b);
        }
    }

    public class myObject
    {
        public string? Name { get; set; }
    }
}

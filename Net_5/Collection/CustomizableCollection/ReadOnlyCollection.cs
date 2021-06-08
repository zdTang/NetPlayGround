using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Collection
{
    class ReadOnlyCollection
    {
        public static void Test()
        {
            Test t = new Test();

            Console.WriteLine(t.Names.Count);       // 0
            t.AddInternally();                      // 在内部直接操作LIST,因而是可以的
            Console.WriteLine(t.Names.Count);       // 1

           // t.Names.Add("test");                    // Compiler error //直接操作READONLY是不可以的
            ((IList<string>)t.Names).Add("test");  // NotSupportedException
        }

    }


    public class Test
    {
        List<string> names;
        public ReadOnlyCollection<string> Names { get; private set; }

        public Test()
        {
            names = new List<string>();                     //普通LIST
            Names = new ReadOnlyCollection<string>(names);  //在原来LIST的基础上包装了一层
        }

        public void AddInternally() { names.Add("test"); }
    }
}

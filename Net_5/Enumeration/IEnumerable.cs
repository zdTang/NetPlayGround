using System;
using System.Collections;
using System.Collections.Generic;  //  For generic
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Enumeration
{
    class TestEnumerable
    {
        public static void Test()
        {
            // FOREACH is a consumer of IEnumerable<T>
            var container = new EnumerableObject();
            foreach (var i in container)
            {
                Console.WriteLine(((int)i).ToString());
            }
        }
    }
    
    
          // A class which inherit IEnumerable can be Consumed by FOREACH() 
    class EnumerableObject:IEnumerable
    {
        private readonly EnumerableCountainer _container = new();
        public IEnumerator GetEnumerator()
        {
            return _container;
        }
    }
}

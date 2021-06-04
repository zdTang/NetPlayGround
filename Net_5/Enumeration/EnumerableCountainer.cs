using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Enumeration
{
    class TestEnumerator
    {
        public static void Test()
        {
            var container = new EnumerableCountainer();
            while (container.MoveNext())
            {
                Console.WriteLine(((int)container.Current).ToString());
            }
        }

    }
    
    
    //this class implement IEnumerator Interface
    
    class EnumerableCountainer:IEnumerator
    {
        private readonly object[] _container = {0,1,2,3,4,5,6,7,8,9};
        private int _index = -1;
        public bool MoveNext()
        {
            _index++;
            return _index <= 9;
        }

        public void Reset()
        {
            _index=-1;
        }

        public object Current => _container[_index];

    }
}

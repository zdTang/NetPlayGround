using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Enumeration
{
    class ENumerationDrill
    {
        public static void Test()
        {
            
            // Test a collection derived from Enumerator
            
            int[] arr = { 1, 2, 3, 4 };
            MyCollection m = new MyCollection(arr);
            while (m.MoveNext())
            {
                Console.WriteLine(m.Current);
            }
            m.Reset();
            //Test a collection derived from Enumerable
            MycollectionTwo t = new MycollectionTwo(m);
            foreach (int item in t)
            {
                Console.WriteLine(item);
            }


            // Test my third 
            MyCollectionThree three = new MyCollectionThree();
            
            foreach (int item in three)
            {
                Console.WriteLine(item);
            }

        }





    }


    //class which derived from IEnumerable
    class MyCollection : IEnumerator<int>
    {
        int[] _collection;
        int _cursor = -1;

        public MyCollection(int[] arr)
        {
            _collection = arr;
        }

        //public int Current => { get {_collection[_cursor];} }
        public int Current => _collection[_cursor];

        public bool MoveNext()
        {
            if (_collection.Length > 0 && _cursor < _collection.Length - 1)
            {
                _cursor++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _cursor = -1;
        }



        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {

        }


    }

    class MycollectionTwo : IEnumerable<int>
    {

        IEnumerator<int> _collection;
        public MycollectionTwo(IEnumerator<int> i)
        {
             _collection=i;

        }

        public IEnumerator<int> GetEnumerator()
        {
            //;
            return _collection;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    class MyCollectionThree : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            int[] arr= { 1, 2, 3, 4, 5 };
            for (int i = 0; i < arr.Length; i++)
                yield return arr[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }



}

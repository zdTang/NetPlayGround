using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Collection.ImmutableCollection
{
    class ImmutableCollection
    {
        
        public static void Test()
        {
            #region Creating
            {
                ImmutableArray<int> array = ImmutableArray.Create<int>(1, 2, 3);
                var list = new[] { 1, 2, 3 }.ToImmutableList();
                array.Dump();
                list.Dump();
            }
            #endregion


            #region Manipulating

            {
                var oldList = ImmutableList.Create<int>(1, 2, 3);

                ImmutableList<int> newList = oldList.Add(4);

                Console.WriteLine(oldList.Count);     // 3  (unaltered)
                Console.WriteLine(newList.Count);     // 4

                var anotherList = oldList.AddRange(new[] { 4, 5, 6 });
                anotherList.Dump();
            }


            #endregion

            #region Builder

            {
                ImmutableArray<int>.Builder builder = ImmutableArray.CreateBuilder<int>();
                builder.Add(1);
                builder.Add(2);
                builder.Add(3);
                builder.RemoveAt(0);
                ImmutableArray<int> myImmutable = builder.ToImmutable();

                myImmutable.Dump();

                var builder2 = myImmutable.ToBuilder();
                builder2.Add(4);      // Efficient
                builder2.Remove(2);   // Efficient
                // ...                  // More changes to builder...
                // Return a new immutable collection with all the changes applied:
                //ImmutableArray<int> myImmutable2 = builder2.ToImmutable().Dump();
            }

            #endregion
        }
  
        

       
        
    }
}

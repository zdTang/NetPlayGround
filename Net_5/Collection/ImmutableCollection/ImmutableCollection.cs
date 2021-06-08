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
        //IMMUTABLE COLLECTION 初始化之后，即不可再改动
        //与普通COLLECTION的不同是，它们的一些可能导致改变的方法，如ADD,REMOVE
        //不会直接改变原COLLECTION.而是返回一个新的COLLECTION,然后加以改变
        public static void Test()
        {
            #region Creating
            {
                ImmutableArray<int> array = ImmutableArray.Create<int>(1, 2, 3);
                //array[1] = 9;                                 //不可以
                var list = new[] { 1, 2, 3 }.ToImmutableList(); //从普通COLLECTION直接转化
                var list2 = list.Add(3);                        //只能产生一个新的LIST
                array.Dump("array");
                list.Dump("list");
                list2.Dump("list2");
            }
            #endregion


            #region Manipulating

            {
                var oldList = ImmutableList.Create<int>(1, 2, 3);

                ImmutableList<int> newList = oldList.Add(4);

                Console.WriteLine(oldList.Count);     // 3  (unaltered)
                Console.WriteLine(newList.Count);     // 4
                //ADDRANGE只重建一个LIST就行，不用重新三个，用ADD的话，添一次就要新建一个LIST
                var anotherList = oldList.AddRange(new[] { 4, 5, 6 });
                anotherList.Dump();
            }


            #endregion

            #region Builder

            {
                //BUILDER可以自由添加，转成IMMUTABLE
                ImmutableArray<int>.Builder builder = ImmutableArray.CreateBuilder<int>();
                builder.Add(1);
                builder.Add(2);
                builder.Add(3);
                builder.RemoveAt(0);
                ImmutableArray<int> myImmutable = builder.ToImmutable();
                builder.Add(8);
                builder.Add(9);
                ImmutableArray<int> myImmutable2 = builder.ToImmutable();
                var myImmutableCopy=myImmutable.Add(88);//仅能建一个新的，不能直接修改
                myImmutable.Dump("myImmutable");
                myImmutable2.Dump("myImmutable2");   //Builder又添加了几个元素后，转变IMMUTABLE
                myImmutableCopy.Dump("myImmutableCopy");
                //IMMUTABLE可以转成普通的
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

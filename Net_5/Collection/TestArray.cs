using System;
using System.Collections;
using System.Text;

namespace Net_5.Collection
{
    class TestArray
    {
        public static void Test()
        {
            #region Referential and Structural comparison

            {
                object[] a1 = { "string", 123, true };
                object[] a2 = { "string", 123, true };

                Console.WriteLine(a1 == a2);                          // False
                Console.WriteLine(a1.Equals(a2));                    // False

                IStructuralEquatable se1 = a1;

                Console.WriteLine(se1.Equals(a2, StructuralComparisons.StructuralEqualityComparer));   // True
                int[] array = new int[5];
                var pp = Array.CreateInstance(typeof(Int32), 10);
            }

            #endregion

            #region Shallow Clone

            {
                StringBuilder[] builders = new StringBuilder[5];
                builders[0] = new StringBuilder("builder1");
                builders[1] = new StringBuilder("builder2");
                builders[2] = new StringBuilder("builder3");

                StringBuilder[] builders2 = builders;
                StringBuilder[] shallowClone = (StringBuilder[])builders.Clone();

                Console.WriteLine(builders);
                Console.WriteLine(builders2);


                Console.WriteLine(builders[0] == builders2[0]);
            }

            #endregion
        }
    }
}

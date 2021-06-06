using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Nullable
{
    class NullableReference
    {
        public static void Test()
        {
#nullable enable   
            // 加上这个后，REFERENCE不可以为NULL,想为NULL,则加？
            // 这个是跨{}的

            #region Basic
            {

                string s1 = null;   // Generates a compiler warning!
                string? s2 = null;  // OK: s2 is nullable reference type
            }
            #endregion

            #region
            {
 

                // Enable nullable reference types

                // 由于S不可以为NULL,因而S.LENGTH没有一点风险
                void Foo(string s) => Console.Write(s.Length);

                // This generates a warning:

                void Foo1(string? s) => Console.Write(s.Length);

                // which we can remove with the null-forgiving operator:
                //  !就是NULL-FORGIVING,只有消除COMPILER的警告
                void Foo2(string? s) => Console.Write(s!.Length);

                // If we add a check, we no longer need the null-forgiving operator in this case:
                void Foo3(string? s)
                {
                    if (s != null) Console.Write(s.Length);// COMPILER足够聪明
                }
            }
            #endregion


            #region Annotation and Warning Context
            {

            }
            #endregion
        }


        class Foo
        {
            string x;     // Generates a warning
        }


            // 这几种指示要弄清楚，其主要目的是决定是否进行警示
#nullable enable annotations   // Enable just the nullable annotation context

        // Because we've enabled the annotation context, s1 is non-nullable, and s2 is nullable:
        public void FooSecond(string s1, string? s2)
        {
            // Our use of s2.Length doesn't generate a warning, however,
            // because we've enabled just the annotation context:
            Console.Write(s2.Length);
        }

        void MainTest()
        {
            // Now let's enable the warning context, too  
#nullable enable warnings

            // Notice that this now generates a warning:  
            FooSecond(null, null);
        }

    }
       
    


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.TestReflection
{
    class ReflectingAndInvokingMember
    {
        public static void Test()
        {
            #region Getting Public members
            {
                //MemberInfo[] members = typeof(Walnut).GetMembers();
                //foreach (MemberInfo m in members)
                //    Console.WriteLine(m);
            }
            #endregion

            #region Declaring Type vs Reflected Type
            {
                // MethodInfo is a subclass of MemberInfo; see Figure 19-1.

                MethodInfo test = typeof(Program).GetMethod("ToString");
                MethodInfo obj = typeof(object).GetMethod("ToString");

                Console.WriteLine(test.DeclaringType);      // System.Object
                Console.WriteLine(obj.DeclaringType);       // System.Object

                Console.WriteLine(test.ReflectedType);      // Program
                Console.WriteLine(obj.ReflectedType);       // System.Object

                Console.WriteLine(test == obj);             // False

                Console.WriteLine(test.MethodHandle == obj.MethodHandle);    // True

                Console.WriteLine(test.MetadataToken == obj.MetadataToken    // True
                                   && test.Module == obj.Module);
            }
            #endregion
        }
    }

    class Walnut
    {
        private bool cracked;
        public void Crack() { cracked = true; }
    }
}

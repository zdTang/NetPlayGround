using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.testType
{
    class Anonyemous
    {
        public static void Test()
        {
            {
                // An anonymous type is a simple class created by the compiler on the fly to store a set of values

                var dude = new { Name = "Bob", Age = 23 };


                // The ToString() method is overloaded:
                var a = dude.ToString();
                var b = dude.Age;
                var c = dude.Name;

                
            }
            int Age = 23;
            // The following:
            {
                var dude = new { Name = "Bob", Age, Age.ToString().Length };
     
            }
            // is shorthand for:
            {
                var dude = new { Name = "Bob", Age = Age, Length = Age.ToString().Length };
 
            }

            {
                // Two anonymous type instances will have the same underlying type if their elements are 
                // same-typed and they’re declared within the same assembly

                var a1 = new { X = 2, Y = 4 };
                var a2 = new { X = 2, Y = 4 };
                Console.WriteLine(a1.GetType() == a2.GetType());   // True

                // Additionally, the Equals method is overridden to perform equality comparisons:

                Console.WriteLine(a1 == a2);         // False   ：Referential Comparison(compare reference)
                Console.WriteLine(a1.Equals(a2));   // True     : Structural Equality Comparison(compare data)
            }

            {
                //return Nameless object
                //var Foo()=>new { X = 2, Y = 4 };          // CANNOT RETURN VAR
                dynamic Boo() => new { X = 2, Y = 4 };      //
            }
        }
    }

    
}

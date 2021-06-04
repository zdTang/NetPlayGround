using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Linq
{
    class TestLinq
    {
        public static void Test()
        {
            {
                int[] v1 = { 1, 2, 3 }; //First vector
                int[] v2 = { 3, 2, 1 }; //Second vector
                //dot product of vector
                var a = v1.Zip(v2, (a, b) => a + b);

                var b = Enumerable.Range(2, 10)
                    .Select(c => new { Length = 2 * c, Height = c * c - 1, Hypotenuse = c * c + 1 });
            }
           
        
            
            {
                int[] values = { 1, 2, 3 };
                int[] weights = { 3, 2, 1 };
                //dot product of vector
                var a = values.Zip(weights, (value, weight) => value * weight); //same as a dot product;
                var b = values.Zip(weights, (value, weight) => value * weight).Sum();//same as a dot product

            }
            {
                int[] nums = { 20, 15, 31, 34, 35, 40, 50, 90, 99, 100 };
                var a=nums
                    .ToLookup(k => k, k => nums.Where(n => n < k))
                    .Select(k => new KeyValuePair<int, double>
                        (k.Key, 100 * ((double) k.First().Count() / (double) nums.Length)));
                var b = nums
                    .ToLookup(k => k, k => nums.Where(n => n < k));

            }

        }
      

}
}

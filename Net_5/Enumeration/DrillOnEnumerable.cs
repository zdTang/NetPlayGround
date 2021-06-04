using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.Enumeration
{
    class DrillOnEnumerable
    {
        public static void Test()
        {

            foreach (int i in Tang(true))
            {
                Console.WriteLine(i);
            }


        }
        static IEnumerable<int> Tang(bool b)
        {
            yield return 1;
            yield return 2;
            if (b)
                yield break;
            yield return 3;
        } 

    }
}

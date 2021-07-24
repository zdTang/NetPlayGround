using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_5.FeatureVersion9
{
    class TestNewFeature
    {
        public static void Test()
        {
            var testInit = new InfoMessage
            {
                ID = 100,
                Message = "Mike",
                Class = "conestoga"
            };

            Console.WriteLine(testInit.Message);
            //testInit.Message = "tom";
            testInit.Class = "Yale University";// OK
        }
    }

    class InfoMessage
    {
        public int ID { get; init; }
        public string Message { get; init; }
        public string Class { get; set; }
    }
}


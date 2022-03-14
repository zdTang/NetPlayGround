using System;

namespace Net_5.testType
{
    public class TestDateTime
    {
        public static void Test()
        {
            var end = new DateTime(2022, 2, 14);//
            var start = new DateTime(2022, 2, 10);
            var range1 = (end - start).TotalDays;//4
            var range2=(end - start).Days;//4
            var range3 = (end.Date - start.Date).Days; //4
            Console.WriteLine("ok");
        }
    }
}
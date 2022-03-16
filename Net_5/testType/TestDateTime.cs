using System;
using System.Collections.Generic;

namespace Net_5.testType
{
    public class TestDateTime
    {
        public static void Test()
        {
            DateTime departure = new DateTime(2010, 6, 12, 18, 32, 0);
            DateTime arrival = new DateTime(2010, 6, 13, 22, 47, 0);
            TimeSpan travelTime = arrival - departure;  
            Console.WriteLine("{0} - {1} = {2}", arrival, departure, travelTime); 
            Console.WriteLine(travelTime.Days);
            
        }
        
        
        
        
        public static void Test2()
        {
            /*var end = new DateTime(2022, 2, 14);//
            var start = new DateTime(2022, 2, 10);
            var range1 = (end - start).TotalDays;//4
            var range2=(end - start).Days;//4
            var range3 = (end.Date - start.Date).Days; //4*/

            foreach (DateTime day in EachDay(new DateTime(2022, 3, 1), DateTime.Now))
            {
                Console.WriteLine(day);
            }
                // print it or whatever
            
        }
        
        
        //https://stackoverflow.com/questions/1847580/how-do-i-loop-through-a-date-range
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for(var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
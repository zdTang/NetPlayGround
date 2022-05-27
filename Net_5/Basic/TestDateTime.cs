using System;
using Net_5.Disposal;

namespace Net_5.Basic
{
    public static class TestDateTime
    {

        public static void Test()
        {
            var date = new DateTime(2022, 5, 22);
            var newDate = date.AddMonths(-1);
            string month_name = newDate.ToString("MMMM");
        }
    }
}
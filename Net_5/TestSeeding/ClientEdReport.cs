

using System;
using System.Collections.Generic;

namespace  Net_5.TestSeeding
{
    public class ClientEdReport : IReport
    {

        public string? CustomerID { get; set; }
        public ReportCustomer? Customer { get; set; }
        public TimeSpan? ReportDate { get; set; }
        
        // The summary Report based on "Reports" of given time span
        public ClientEdDailyReport SummaryClientEdDailyReport { set; get; }
        
        // A Collection of ClientEdDailyReport
        public List<ClientEdDailyReport>? Reports { get; set; }
        public List<IReport> GetReports()
        {
            // Gather ClientEdDailyReport instances for given timespan (this.ReportDate)
            // and populate this.Reports.
            return null;
        }

        // Just an example >{
        /*
        public int TotalViews
        {
            get
            {
                //pseudo-code. :-P
                return 99;
               // return Reports.TotalViews.Sum();
            }
            set
            {
            }
        }
        
        */
    }
}

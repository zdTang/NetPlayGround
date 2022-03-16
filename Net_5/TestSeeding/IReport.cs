using System;
using System.Collections.Generic;

namespace  Net_5.TestSeeding
{
    // Interface TBD.  Need Interface to abstract out report types.
    public interface IReport
    {
        public string CustomerID { get; set; }
        //public DateTime? ReportDate { get; set; }
        public TimeSpan? ReportDate { get; set; }

        public List<IReport> GetReports();
        
    }
}

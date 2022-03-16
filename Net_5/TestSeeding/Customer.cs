using System.Collections.Generic;

namespace  Net_5.TestSeeding
{
    /// <summary>
    /// Mocked-up Customer.  For reference only.
    /// </summary>
    /// Changed to CustomerSecond --name conflict
    public class ReportCustomer
    {
        public int CustomerID { get; set; }
        public string? ZuoraID { get; set; } // Add to existing model?
        public List<ReportCustomer>? Children { get; set; }
        //public List<IReport>? ClientReports { get; set; }
        public ClientEdReport? ClientEdReport { get; set; }
    }
}

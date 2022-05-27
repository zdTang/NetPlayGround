

using System;
using System.Collections.Generic;
using System.Linq;

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
        
        /*==========================================================================
         * Name: GetSummaryClientEdReportAsParent
         * Description: As a parent, Summarize all children's ClientEdDailyReport into a summary report 
         * Parameters: List<ClientEdDailyReport> reports
         * Return: A summary ClientEdDailyReport which will be used as ViewModel when user
         *         chose a date range.
         ============================================================================*/
        
        
        
        
        
        /*==========================================================================
         * Name: GetSummaryClientEdReport
         * Description: Summarize a list of ClientEdDailyReport into a summary report 
         * Parameters: List<ClientEdDailyReport> reports
         * Return: A summary ClientEdDailyReport which will be used as ViewModel when user
         *         chose a date range.
         ============================================================================*/
        
        private  ClientEdDailyReport GetSummaryClientEdReport(List<ClientEdDailyReport> reports)
        {
            if (reports == null) return null;
            var summaryReport = new ClientEdDailyReport
            {
                CustomerId = (reports.First().CustomerId) , // Mike added
                ArticlesEdited = reports.Sum(item => item.ArticlesEdited),
                NewArticles = reports.Sum(item => item.NewArticles),
                ReportDate = DateTime.Today,
                TopSearchNumQueries = reports.Sum(item => item.TopSearchNumQueries),
                TopSearchTerm = "Need to do research for this property!",
                TotalSearches = reports.Sum(item => item.TotalSearches),
                // For daily report, the following data are retrived from ArticleViews collection
                // For summary report, the following data can come from the sum every daily report 
                TotalDesktop = reports.Sum(item => item.TotalDesktop),
                TotalEmail = reports.Sum(item => item.TotalEmail),
                TotalFavorite = reports.Sum(item => item.TotalFavorite),
                TotalMobile = reports.Sum(item => item.TotalMobile),
                TotalPIMSAccess = reports.Sum(item => item.TotalPIMSAccess),
                TotalPortalAccess = reports.Sum(item => item.TotalPortalAccess),
                TotalPrint = reports.Sum(item => item.TotalPrint),
                TotalTablet = reports.Sum(item => item.TotalTablet),
                TotalViews = reports.Sum(item => item.TotalViews),
                TotalWebAccess = reports.Sum(item => item.TotalWebAccess),
                ArticleViews = GetSummaryArticleViews(reports),
            };
            
            return summaryReport;
        }

        private List<ClientEdArticleViews> GetSummaryArticleViews(List<ClientEdDailyReport> reports)
        {
            // retrieve ArticleViews List from each ClientEdDailyReport
            List<List<ClientEdArticleViews>> list = new List<List<ClientEdArticleViews>>();
            foreach (var clientEdDailyReport in reports)
            {
                list.Add(clientEdDailyReport.ArticleViews);
            }

            // Create a summary "ArticleViews"
            List<ClientEdArticleViews> summaryArticleView = new List<ClientEdArticleViews>();
            /*===============================================================================
             * TODO: How to summary a List of List with LINQ?
             * Will study the following code and try to find a concise and efficient approach
             * This approach uses two-level loops
             * Loop level one: i is one Articles[i], loop all Articles
             * Loop level two: j is one list[j]
             * Take Articles[i] from all lists and summary each value of this Article
             ===============================================================================*/
            for (int i = 0; i < list.First().Count; i++) // loop by article Number
            {
                var item = new ClientEdArticleViews(); // Create a empty object
                //these four fields are not need to be summarized!
                item.ArticleName = list.First().First().ArticleName;
                item.CreatedDate = DateTime.Now;
                item.CustomerId = list.First().First().CustomerId;
                item.ReportID = list.First().First().ReportID;

                // Summarize those Data//May provide article id?

                for (int j = 0; j < list.Count; j++) // loop by dailyReport
                {
                    item.NumViews += list[j][i].NumViews;
                    item.TotalPrint += list[j][i].TotalPrint;
                    item.TotalFavorite += list[j][i].TotalFavorite;
                    item.TotalEmail += list[j][i].TotalEmail;
                    item.TotalPortalAccess += list[j][i].TotalPortalAccess;
                    item.TotalPIMSAccess += list[j][i].TotalPIMSAccess;
                    item.TotalWebAccess += list[j][i].TotalPIMSAccess;
                    item.TotalTablet += list[j][i].TotalTablet;
                    item.TotalDesktop += list[j][i].TotalDesktop;
                    item.TotalMobile += list[j][i].TotalMobile;
                }
                summaryArticleView.Add(item);
            }
            return summaryArticleView;
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

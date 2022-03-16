using System;
using System.Collections.Generic;

namespace  Net_5.TestSeeding
{
    public class ClientEdDailyReport
    {
        public int CustomerId { get; set; }    // Mike added
        public DateTime ReportDate { get; set; }
        public int TotalSearches { get; set; }
        public int TotalViews { get; set; }
        public int NewArticles { get; set; }
        public int ArticlesEdited { get; set; }
        public string? TopSearchTerm { get; set; }
        public int TopSearchNumQueries { get; set; }
        
        public int TotalPrint { get; set; } // Could be sum of ArticleViews.TotalPrint...
        public int TotalFavorite { get; set; }
        public int TotalEmail { get; set; }
        
        public int TotalPortalAccess { get; set; }
        public int TotalPIMSAccess { get; set; }
        public int TotalWebAccess { get; set; }
        
        public int TotalTablet { get; set; }
        public int TotalDesktop { get; set; }
        public int TotalMobile { get; set; }


        public List<ClientEdArticleViews>? ArticleViews { get; set; }
       
    }
}

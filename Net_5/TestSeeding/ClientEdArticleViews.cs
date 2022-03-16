using System;

namespace Net_5.TestSeeding
{
    public class ClientEdArticleViews
    {
        public int CustomerId { get; set; }    // Mike added
        public DateTime? CreatedDate { get; set; } // Mike added
        
        public int ReportID { get; set; }
        public string? ArticleName { get; set; }
        // May provide article id?
        public int NumViews { get; set; }

        #region Engagements
        public int TotalPrint { get; set; }
        public int TotalFavorite { get; set; }
        public int TotalEmail { get; set; }
        #endregion

        #region Content Access
        public int TotalPortalAccess { get; set; }
        public int TotalPIMSAccess { get; set; }
        public int TotalWebAccess { get; set; }
        #endregion


        #region Portal Access
        public int TotalTablet { get; set; }
        public int TotalDesktop { get; set; }
        public int TotalMobile { get; set; }
        #endregion
    }

}

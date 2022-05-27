using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Net_5.Disposal;

namespace Net_5.TestSeeding
{
    // Todo: 
    // 1. disperse Array

    public class Seed
    {
        static Random rd = new Random();

        public static void Test()
        {
            // var data=CreateClientArticle(2, 1, (2022, 3, 13));
            // var dailyReport = CreatClientEdDailyReport(data);
            List<ReportCustomer> customerList = new List<ReportCustomer>();
            for (int i = 0; i < 5; i++)
            {
                #region Create Children ClientEdReport

                var childReports = CreatClientEdReports(5, i, new DateTime(2022, 3, 14));
                var clientEdReport = new ClientEdReport();
                clientEdReport.Reports = childReports;
                clientEdReport.SummaryClientEdDailyReport = CreateSummaryClientEdDailyReport(childReports);
                #endregion
                
                #region Create Children reportCustomer
                var childReportCustomer = new ReportCustomer();
                childReportCustomer.ClientEdReport = clientEdReport;  
                childReportCustomer.CustomerID = i;    
                #endregion
                
                customerList.Add(childReportCustomer);
            }

            #region Create Parent ClientEdReport
            var parentReports=CreatClientEdReports(5, 100, new DateTime(2022, 3, 14));
            var parentClientEdReport = new ClientEdReport();
            parentClientEdReport.Reports = parentReports;
            parentClientEdReport.SummaryClientEdDailyReport = CreateSummaryClientEdDailyReport(parentReports);
            #endregion


            var parentReportCustomer = new ReportCustomer();

            parentReportCustomer.CustomerID = 100;
            parentReportCustomer.ZuoraID = "dummyId";
            parentReportCustomer.Children = customerList;
            parentReportCustomer.ClientEdReport = parentClientEdReport;


            
            //var reports=CreatClientEdReports(10, 1, new DateTime(2022, 3, 1));
            Console.WriteLine(parentReportCustomer);
        }

        public static List<ClientEdArticleViews> CreateClientArticle(int numOfArticle, int userId, (int, int, int) date)
        {
            List<ClientEdArticleViews> list = new List<ClientEdArticleViews>();
            for (int i = 0; i < numOfArticle; i++)
            {
                var newlist = new ClientEdArticleViews()
                {
                    CustomerId = userId, // Mike added
                    CreatedDate = new DateTime(date.Item1, date.Item2, date.Item3), // Mike added
                    ReportID = i,
                    ArticleName = $"ArticleName--{i}",

                    //May provide article id?
                    NumViews = rd.Next(50),

                    #region Engagements

                    TotalPrint = rd.Next(50),
                    TotalFavorite = rd.Next(50),
                    TotalEmail = rd.Next(50),

                    #endregion

                    #region Content Access

                    TotalPortalAccess = rd.Next(50),
                    TotalPIMSAccess = rd.Next(50),
                    TotalWebAccess = rd.Next(50),

                    #endregion


                    #region Portal Access

                    TotalTablet = rd.Next(50),
                    TotalDesktop = rd.Next(50),
                    TotalMobile = rd.Next(50),

                    #endregion

                };
                list.Add(newlist);
            }

            return list;
        }


/// <summary>
/// Development Mode:   when number is -1, means something is wrong
/// </summary>
/// <param name="articleViews"></param>
/// <returns></returns>


        public static ClientEdDailyReport CreatClientEdDailyReport(List<ClientEdArticleViews>? articleViews)
        {
            var dailyReport = new ClientEdDailyReport()
            {
                CustomerId = (articleViews?.First().CustomerId)??1, // Mike added
                ArticlesEdited = rd.Next(20),
                NewArticles = rd.Next(50),
                ReportDate = (articleViews?.First().CreatedDate)??DateTime.Today,
                TopSearchNumQueries = rd.Next(50),
                TopSearchTerm = CreateString(5),
                TotalSearches = rd.Next(50),
                // the following data are retrived from ArticleViews collection
                TotalDesktop = articleViews?.Sum(item=>item.TotalDesktop)??-1,
                TotalEmail = articleViews?.Sum(item=>item.TotalEmail)??-1,
                TotalFavorite = articleViews?.Sum(item=>item.TotalFavorite)??-1,
                TotalMobile = articleViews?.Sum(item=>item.TotalMobile)??-1,
                TotalPIMSAccess = articleViews?.Sum(item=>item.TotalPIMSAccess)??-1,
                TotalPortalAccess = articleViews?.Sum(item=>item.TotalPortalAccess)??-1,
                TotalPrint = articleViews?.Sum(item=>item.TotalPrint)??-1,
                TotalTablet = articleViews?.Sum(item=>item.TotalTablet)??-1,
                TotalViews = articleViews?.Sum(item=>item.NumViews)??-1,
                TotalWebAccess = articleViews?.Sum(item=>item.TotalWebAccess)??-1,
                ArticleViews = articleViews,
            };
                
                
                
        
            
            return dailyReport;
        }


public static ClientEdDailyReport CreateSummaryClientEdDailyReport(List<ClientEdDailyReport> reports)
{
    var summaryReport = new ClientEdDailyReport
    {
        CustomerId = (reports?.First().CustomerId) ?? 1, // Mike added
        ArticlesEdited = reports.Sum(item => item.ArticlesEdited),
        NewArticles = reports.Sum(item => item.NewArticles),
        ReportDate = DateTime.Today,
        TopSearchNumQueries = reports.Sum(item => item.TopSearchNumQueries),
        TopSearchTerm = CreateString(5),
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
        ArticleViews = SummaryArticleViews(reports),
    };
    
    return summaryReport;
}

public static List<ClientEdArticleViews> SummaryArticleViews(List<ClientEdDailyReport> reports)
{
    List<List<ClientEdArticleViews>> list = new List<List<ClientEdArticleViews>>();
    foreach (var clientEdDailyReport in reports)
    {
        list.Add(clientEdDailyReport.ArticleViews);
    }

    List<ClientEdArticleViews> SummaryArticleView = new List<ClientEdArticleViews>();
    //List<ClientEdArticleViews> SummaryAritcleViews = new List<ClientEdArticleViews>();
    for (int i = 0; i < list.First().Count; i++) // loop by article Number
    {
        var item = new ClientEdArticleViews();
        item.ArticleName = list.First().First().ArticleName;
        item.CreatedDate=DateTime.Now;
        item.CustomerId=list.First().First().CustomerId;
        item.ReportID=list.First().First().ReportID;
        // Summary those Data//May provide article id?
        int NumViewsSummary = 0;
        int TotalPrintSummary = 0;
        int TotalFavoriteSummary = 0;
        int TotalEmailSummary = 0;
        int TotalPortalAccessSummary = 0;
        int TotalPIMSAccessSummary = 0;
        int TotalWebAccessSummary = 0;
        int TotalTabletSummary = 0;
        int TotalDesktopSummary = 0;
        int TotalMobileSummary = 0;
        
        
        for (int j = 0; j < list.Count; j++)    // loop by dailyReport
        {
            NumViewsSummary += list[j][i].NumViews;
            TotalPrintSummary += list[j][i].TotalPrint;
            TotalFavoriteSummary += list[j][i].TotalFavorite;
            TotalEmailSummary += list[j][i].TotalEmail;
            TotalPortalAccessSummary += list[j][i].TotalPortalAccess;
            TotalPIMSAccessSummary += list[j][i].TotalPIMSAccess;
            TotalWebAccessSummary += list[j][i].TotalPIMSAccess;
            TotalTabletSummary += list[j][i].TotalTablet;
            TotalDesktopSummary += list[j][i].TotalDesktop;
            TotalMobileSummary += list[j][i].TotalMobile;
        }

        item.NumViews = NumViewsSummary;
        item.TotalPrint = TotalPrintSummary;
        item.TotalFavorite = TotalFavoriteSummary;
        item.TotalEmail = TotalEmailSummary;
        item.TotalPortalAccess = TotalPortalAccessSummary;
        item.TotalPIMSAccess = TotalPIMSAccessSummary;
        item.TotalWebAccess = TotalWebAccessSummary;
        item.TotalTablet = TotalTabletSummary;
        item.TotalDesktop = TotalDesktopSummary;
        item.TotalMobile = TotalMobileSummary;
        
        SummaryArticleView.Add(item);
    }
    
    
    
    
    
    return SummaryArticleView;
}



public static List<ClientEdDailyReport> CreatClientEdReports(int numOfArticles,int userId, DateTime start)
{

    var reports = new List<ClientEdDailyReport>();
    foreach (DateTime day in EachDay(start, DateTime.Now))
    {
        List<ClientEdArticleViews> articleViews = CreateClientArticle(numOfArticles, userId, (day.Year, day.Month, day.Day));
        ClientEdDailyReport dailyReport = CreatClientEdDailyReport(articleViews);
        reports.Add(dailyReport);
    }   
    
    return reports;
}

        //https://stackoverflow.com/questions/4616685/how-to-generate-a-random-string-and-specify-the-length-you-want-or-better-gene
        internal static string CreateString(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }


        //https://stackoverflow.com/questions/1847580/how-do-i-loop-through-a-date-range
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for(var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        
        
        /*
         *
         foreach (DateTime day in EachDay(new DateTime(2022, 3, 1), DateTime.Now))
            {
                Console.WriteLine(day);
            }
         * 
         */


    }

}
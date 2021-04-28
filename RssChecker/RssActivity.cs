using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RssChecker
{
    /// <summary>
    /// Contains Functions related Rss Activity
    /// </summary>
     static public class RssActivity
    {
        /// <summary>
        /// This is the function that finds a List of Company Names with no Rss Activity for a given number of days
        /// </summary>
        /// <param name="rssDictionary">Dictionary containing Company Names as keys, List Of Rss Feed URLs as values</param>
        /// <param name="daysInactive">Number of Days with no Rss Activity</param>
        /// <returns>List of strings that are the Company Names with no Rss Activity for a given number of days</returns>
        static public List<string>  CompanysInactiveForXDays(Dictionary<string, List<string>> rssDictionary, double daysInactive)
        {
            try
            {   // create a list of company objects - 1 for each keyValuePair
                // Parallelize the Query for better performance
                List<Company> Companys = rssDictionary.AsParallel()
                                                      .Select(kvp => new Company(kvp))
                                                      .ToList();

                // return a list of ordered company names that have been Inactive for daysInactive parameter
                return Companys.Where(c => c.LatestPublishDateForAllRssFeeds != null)
                        .Where(c => DateTime.Now.Subtract((DateTime)c.LatestPublishDateForAllRssFeeds).TotalDays >= daysInactive)
                        .Select(c => c.Name)
                        .OrderBy(n => n)
                        .ToList();
            }
            catch
            {
                // return empty list if exception occurs
                // this can happen if rssDictionary is null
                return new List<string>();
            }
        }

        /// <summary>
        /// Gets the most recent PublishDate from all Rss feed items 
        /// </summary>
        /// <param name="rssURL">The URL of the Rss feed</param>
        /// <returns>Returns most recent PublishDate from all Rss feed items.  Returns null if any Exception Occurs.</returns>
        static public DateTime? RssLatestPublishDate(string rssURL)
        {
            try
            {
                //create an XMLReader with the rss URL
                using XmlReader rssReader = XmlReader.Create(rssURL);
                {
                    //load SyndicationFeed from rssReader
                    SyndicationFeed rssFeed = SyndicationFeed.Load(rssReader);

                    // get the max of the PublishDate from all Feed Items in rssFeed   
                    return rssFeed.Items.Select(fi => fi.PublishDate.UtcDateTime.ToLocalTime()).Max();
                }
            }
            catch
            {
                //return null if any exception occurs
                return null;
            }
        }
    }
}

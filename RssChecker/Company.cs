using System;
using System.Collections.Generic;
using System.Linq;

namespace RssChecker
{
    /// <summary>
    /// Class representing a Company
    /// </summary>
    class Company
    {
        /// <summary>
        /// Name of the Company
        /// </summary>
        public string Name { get; set; }   
        /// <summary>
        /// List of the Company's RssFeed Objects
        /// </summary>
        public List<RssFeed> RssFeeds { get; set; }
        /// <summary>
        /// Max of all Company's Rss Feed's Item's Publish Dates
        /// </summary>
        public DateTime? LatestPublishDateForAllRssFeeds;    

        /// <summary>
        /// Constructor for Company using a KeyValuePair from a rssDictionary
        /// </summary>
        /// <param name="rssDictionaryEntry"></param>
        public Company(KeyValuePair<string, List<string>> rssDictionaryEntry)
        {
            //Get company name from key
            Name = rssDictionaryEntry.Key;

            //create a new list of RssFeed objects
            RssFeeds = new();
            
            //create a new RssFeed object for each Company's rssURLs
            //Parallelize for better performance
            rssDictionaryEntry.Value.AsParallel()
                                    .ForAll(url =>  RssFeeds.Add(new RssFeed(url)));

            try
            {
                // Get the Max of all non-null RssFeed LatestPublishDate values
                LatestPublishDateForAllRssFeeds = RssFeeds.Where(rf => rf.LatestPublishDate != null)
                                                          .Select(rf => (DateTime)rf.LatestPublishDate)
                                                          .Max();
            }
            catch
            {
                // Set to null if can't find the max without an exception
                // this can happen if all company's feeds are unavailable
                LatestPublishDateForAllRssFeeds = null;
            }
        }       
    }
}

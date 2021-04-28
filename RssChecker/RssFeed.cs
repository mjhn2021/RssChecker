using System;

namespace RssChecker
{
    /// <summary>
    /// Class representing a Rss Feed
    /// </summary>
    public class RssFeed
    {
        /// <summary>
        /// Url of the Rss Feed
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// Max of all Rss Feed's Item's PublishDates
        /// </summary>
        public DateTime? LatestPublishDate { get; set; }

        /// <summary>
        /// Constructor for the RssFeed object using the Rss Feed's url
        /// </summary>
        /// <param name="url"></param>
        public RssFeed(string url)
        {
            Url = url;
            LatestPublishDate = RssActivity.RssLatestPublishDate(url);
        }
    }
}

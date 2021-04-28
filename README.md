# RssChallenge solution overview:

* function RssActivity.CompanysInactiveForXDays (rssDictionary, daysInactive) - Returns List of strings that are the Company Names with no Rss Activity for a given number of days.

* function RssActivity.RssLatestPublishDate (rssURL) - Returns most recent PublishDate from all Rss feed items.

* class RssActivity - static class containing functions related to Rss Activity

* class Company - class representing a Company

* class RssFeed - class representing a Rss Feed

# Assumptions made:

* A Company's Rss Activity is considered active within a time period if atleast one of its Rss Feeds is active.
* If an Rss Feed can not be accessed, it's activity is considered unknown - neither active or inactive.
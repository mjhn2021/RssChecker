
# RssChecker problem:

* Given a Dictionary keyed by Company and valued by RSS feed url, write a function that determines which companies had no activity for a given number of days.

# RssChecker solution overview:

* function RssActivity.CompanysInactiveForXDays (rssDictionary, daysInactive) - Returns List of strings that are the Names of the Companys Names with no Rss Activity for a given number of days.

* function RssActivity.RssLatestPublishDate (rssURL) - Returns most recent PublishDate from all Rss feed items.

* class RssActivity - static class containing functions related to Rss Activity

* class Company - class representing a Company

* class RssFeed - class representing a Rss Feed

# Technologies Used

* .NET5 , C#, LINQ, PLINQ, NUnit 3 Tests

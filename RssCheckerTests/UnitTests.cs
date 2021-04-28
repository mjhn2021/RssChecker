using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using RssChecker;
using System.Diagnostics;
using System.Linq;

namespace RssCheckerTests
{
    /// <summary>
    /// Unit Tests for Rss Challenge
    /// </summary>
    public class UnitTests
    {
        //Latest Publish Dates for Test Rss Feed Files:
        //Test1Rss.xml-3/29/2021 3:00:00 AM
        //Test2Rss.xml-3/10/2021 3:00:00 AM
        //Test3Rss.xml-4/24/2021 12:00:00 AM
        //Test4Rss.xml-4/19/2021 3:00:00 AM
        //Test5Rss.xml-4/24/2021 3:30:00 AM
        //Test6Rss.xml-4/24/2021 3:16:25 PM
        //Test7Rss.xml-4/22/2021 11:48:00 PM
        //Test8Rss.xml-4/21/2021 6:00:00 AM
        //Test9Rss.xml-4/23/2021 10:38:40 AM
        //Test10Rss.xml-4/24/2021 12:00:00 PM

        // test dictionary
        Dictionary<string, List<string>> rssTestDictionary;
        // string for baseURL of Test Files 
        readonly string baseURLTestFiles = Directory.GetParent(
                                            Directory.GetParent(
                                             Directory.GetParent(
                                              Directory.GetParent(
                                               AppDomain.CurrentDomain.BaseDirectory)
                                              .FullName)
                                             .FullName)
                                            .FullName)
                                           .FullName + "\\TestFiles";

        /// <summary>
        /// Set up a Test Dictionary for the tests
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // start a new TestDictionary
            rssTestDictionary = new();

            //populate first 10 Companys in TestDictionary from 10 raw TestFiles on GitHub
            // 1 rss feed for each company
            foreach (int number in Enumerable.Range(1, 10))
            {
                rssTestDictionary.Add("TestCompany" + number.ToString(),
                                        new List<string>()
                                        { $"{baseURLTestFiles}/Test{number}Rss.xml" });
            }

            // TestCompany 11 - 2 Rss Feeds
            rssTestDictionary.Add("TestCompany11",
                                    new List<string>()
                                    { $"{baseURLTestFiles}/Test1Rss.xml" ,      // 3/29/2021 3:00:00 AM (latest)
                                      $"{baseURLTestFiles}/Test2Rss.xml" });    // 3/10/2021 3:00:00 AM                                                                            

            // TestCompany 12 - 4 Rss Feeds
            rssTestDictionary.Add("TestCompany12",
                                    new List<string>()
                                    { $"{baseURLTestFiles}/Test2Rss.xml" ,      // 3/10/2021 3:00:00 AM
                                      $"{baseURLTestFiles}/Test4Rss.xml" ,      // 4/19/2021 3:00:00 AM
                                      $"{baseURLTestFiles}/Test5Rss.xml" ,      // 4/24/2021 3:30:00 AM
                                      $"{baseURLTestFiles}/Test6Rss.xml" });    // 4/24/2021 3:16:25 PM (latest)

            // TestCompany 13 - 6 Rss Feeds - 2 Invalid URL's
            rssTestDictionary.Add("TestCompany13",
                                    new List<string>()
                                    { $"{baseURLTestFiles}/Test2Rss.xml" ,      // 3/10/2021 3:00:00 AM
                                      $"{baseURLTestFiles}/Test8Rss.xml" ,      // 4/21/2021 6:00:00 AM
                                      $"{baseURLTestFiles}/Test9Rss.xml" ,      // 4/23/2021 10:38:40 AM
                                      $"{baseURLTestFiles}/Test10Rss.xml" ,     // 4/24/2021 12:00:00 PM (latest)
                                       "InvalidURL1",                           // Unknown
                                       "InvalidURL2"});                         // Unknown


            // TestCompany 14 - 4 Rss Feeds - 4 Invalid URL's
            // This company's Rss Activity is unknowm - it will not be considered inactive
            rssTestDictionary.Add("TestCompany14",
                                    new List<string>()
                                    { "InvalidURL1" ,
                                      "InvalidURL2" ,
                                      "InvalidURL3" ,
                                      "InvalidURL4" });
        }

        /// <summary>
        /// Testing the RssActivity.RssLatestPublishDate() method with a Valid URL
        /// </summary>
        [Test]
        public void Test01_RssLatestPublishDate_ValidURL1()
        {
            //arrange
            // expected datetime from XML: <pubDate>Mon, 29 Mar 2021 07:00:00 -0000</pubDate>
            DateTime? expected = DateTime.Parse("Mon, 29 Mar 2021 07:00:00 -0000");

            //act
            DateTime? actual = RssActivity.RssLatestPublishDate($"{baseURLTestFiles}/Test1Rss.xml");

            //assert
            Assert.AreEqual(expected, actual, expected.ToString() + " != " + actual.ToString());
        }

        /// <summary>
        /// Testing the RssActivity.RssLatestPublishDate() method with a Valid URL
        /// </summary>
        [Test]
        public void Test02_RssLatestPublishDate_ValidURL2()
        {
            //arrange
            // expected datetime from XML: <pubDate>Wed, 10 Mar 2021 08:00:00 GMT</pubDate>
            DateTime? expected = DateTime.Parse("Wed, 10 Mar 2021 08:00:00 GMT");

            //act
            DateTime? actual = RssActivity.RssLatestPublishDate($"{baseURLTestFiles}/Test2Rss.xml");

            //assert
            Assert.AreEqual(expected, actual, expected.ToString() + " != " + actual.ToString());
        }

        /// <summary>
        /// Testing the RssActivity.RssLatestPublishDate() method with a Valid URL
        /// </summary>
        [Test]
        public void Test03_RssLatestPublishDate_ValidURL3()
        {
            //arrange
            // expected datetime from XML: <pubDate>Sat, 24 Apr 2021 00:00:00 -0400</pubDate>
            DateTime? expected = DateTime.Parse("Sat, 24 Apr 2021 00:00:00 -0400");

            //act
            DateTime? actual = RssActivity.RssLatestPublishDate($"{baseURLTestFiles}/Test3Rss.xml");

            //assert
            Assert.AreEqual(expected, actual, expected.ToString() + " != " + actual.ToString());
        }


        /// <summary>
        /// Testing the RssActivity.RssLatestPublishDate() method with an Invalid URL
        /// </summary>
        [Test]
        public void Test04_RssLatestPublishDate_InvalidURL()
        {
            //arrange
            // expected value is null because of Invalid URL
            DateTime? expected = null;

            //act
            DateTime? actual = RssActivity.RssLatestPublishDate("MyInvalidURL");

            //assert
            Assert.AreEqual(expected, actual, expected.ToString() + " != " + actual.ToString());
        }

        /// <summary>
        /// Testing the RssActivity.CompanysInactiveForXDays function with 14 Test Companys
        /// Testing for inactive companies since 1000 days before 4-25-21 (date of test creation)
        /// </summary>
        [Test]
        public void Test05_CompanysInactiveForXDays_1000days()
        {
            //arrange
            // expected value is empty List of strings
            List<string> expected = new() { };

            //act  
            // look for inactive companies since 1000 days before 4-25-21 (date of test creation)
            List<string> actual = RssActivity.CompanysInactiveForXDays(rssTestDictionary,
                                    1000 + DateTime.Now.Subtract(new DateTime(2021, 4, 25, 21, 12, 0)).TotalDays);

            //assert
            Assert.AreEqual(expected, actual, string.Join(",", expected) + " != " + string.Join(",", actual));
        }

        /// <summary>
        /// Testing the RssActivity.CompanysInactiveForXDays function with 14 Test Companys
        /// Testing for inactive companies since 30 days before 4-25-21 (date of test creation)
        /// </summary>
        [Test]
        public void Test06_CompanysInactiveForXDays_30days()
        {
            //arrange
            // expected value is TestCompany2
            List<string> expected = new() { "TestCompany2" };

            //act  
            // look for inactive companies since 30 days before 4-25-21 (date of test creation)
            List<string> actual = RssActivity.CompanysInactiveForXDays(rssTestDictionary,
                                    30 + DateTime.Now.Subtract(new DateTime(2021, 4, 25, 21, 17, 0)).TotalDays);

            //assert
            Assert.AreEqual(expected, actual, string.Join(",", expected) + " != " + string.Join(",", actual));
        }

        /// <summary>
        /// Testing the RssActivity.CompanysInactiveForXDays function with 14 Test Companys
        /// Testing for inactive companies since 5 days before 4-25-21 (date of test creation)
        /// </summary>
        [Test]
        public void Test07_CompanysInactiveForXDays_5days()
        {
            //arrange
            // expected value is "TestCompany1", "TestCompany11", "TestCompany2", "TestCompany4"
            List<string> expected = new() { "TestCompany1", "TestCompany11", "TestCompany2", "TestCompany4" };

            //act  
            // look for inactive companies since 5 days before 4-25-21 (date of test creation)
            List<string> actual = RssActivity.CompanysInactiveForXDays(rssTestDictionary,
                                    5 + DateTime.Now.Subtract(new DateTime(2021, 4, 25, 21, 22, 0)).TotalDays);

            //assert
            Assert.AreEqual(expected, actual, string.Join(",", expected) + " != " + string.Join(",", actual));
        }


        /// <summary>
        /// Testing the RssActivity.CompanysInactiveForXDays function with 14 Test Companys
        /// Testing for inactive companies since 0 days before 4-25-21 (date of test creation)
        /// </summary>
        [Test]
        public void Test08_CompanysInactiveForXDays_0days()
        {
            //arrange
            // expected value is all company names (keys in dictionary) except TestCompany14 in ascending order
            List<string> expected = rssTestDictionary.Keys
                                                     .Where(n => n != "TestCompany14")
                                                     .OrderBy(n => n)
                                                     .ToList();
            //act  
            // look for inactive companies since 0 days before 4-25-21 (date of test creation)
            List<string> actual = RssActivity.CompanysInactiveForXDays(rssTestDictionary,
                                    0 + DateTime.Now.Subtract(new DateTime(2021, 4, 25, 21, 27, 0)).TotalDays);

            //assert
            Assert.AreEqual(expected, actual, string.Join(",", expected) + " != " + string.Join(",", actual));
        }

        /// <summary>
        /// Testing the RssActivity.CompanysInactiveForXDays function with Empty rssDictionary
        /// Testing for inactive companies since 0 days before 4-25-21 (date of test creation)
        /// </summary>
        [Test]
        public void Test09_CompanysInactiveForXDays_EmptyDictionary()
        {
            //arrange
            // expected value is an empty List of strings
            List<string> expected = new();

            //act  
            // look for inactive companies since 0 days before 4-25-21 (date of test creation)
            // pass empty rssDictionary
            List<string> actual = RssActivity.CompanysInactiveForXDays(new Dictionary<string, List<string>>(),
                                    0 + DateTime.Now.Subtract(new DateTime(2021, 4, 25, 21, 27, 0)).TotalDays);

            //assert
            Assert.AreEqual(expected, actual, string.Join(",", expected) + " != " + string.Join(",", actual));
        }
        /// <summary>
        /// Testing the RssActivity.CompanysInactiveForXDays function with null rssDictionary
        /// Testing for inactive companies since 0 days before 4-25-21 (date of test creation)
        /// </summary>
        [Test]
        public void Test10_CompanysInactiveForXDays_NullDictionary()
        {
            //arrange
            // expected value is an empty List of strings
            List<string> expected = new();

            //act  
            // look for inactive companies since 0 days before 4-25-21 (date of test creation)
            // pass null rssDictionary
            List<string> actual = RssActivity.CompanysInactiveForXDays(null,
                                    0 + DateTime.Now.Subtract(new DateTime(2021, 4, 25, 21, 27, 0)).TotalDays);

            //assert
            Assert.AreEqual(expected, actual, string.Join(",", expected) + " != " + string.Join(",", actual));
        }
    }
}


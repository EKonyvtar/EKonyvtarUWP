using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestRssService()
        {
            var feed = EKonyvtarUW.Services.RssFeedService.GetFeedAsync("http://mek.oszk.hu/mek2.rss").Result;
            Assert.IsNotNull(feed);
        }

        [TestMethod]
        public void TestSearchService()
        {
            var results = EKonyvtarUW.Services.OnlineMekService.SearchBookAsync("horthy").Result;
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestGetBook()
        {
            var results = EKonyvtarUW.Services.OnlineMekService.GetBookByUid("2397").Result;
            Assert.IsNotNull(results);
        }


        [TestMethod]
        public void TestLatin2()
        {
            var results = EKonyvtarUW.Services.OnlineMekService.StringConvert("aáÁeéÉiíÍoóÓoöÖoőŐuúÚuüÜuűŰ");
            Assert.IsNotNull(results);
        }
    }
}

using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using telegramBot_BL.Enums;
using telegramBot_BL.Helpers;

namespace telegramBot_BL_Test
{
    [TestClass]
    public class GitHubSearchQueryBuilderTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string expected = "query{search(query:\"PHP+in:topics\",type:REPOSITORY,first:10){nodes {... on Repository {name}}}}";
            expected = Regex.Replace(expected, @"\s+", "");

            GithubSearchQueryBuilder qb = new GithubSearchQueryBuilder();
            qb.AddCondition("topics","PHP");
            qb.SetSearchObejctType(GithubSearchType.REPOSITORY);
            qb.SetFirst(10);
            qb.AddReturnNodeProperty("name");
            string res =qb.build();
            res = Regex.Replace(res, @"\s+", "");
            Assert.AreEqual(expected, res);
        }
    }
}

using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using telegramBot_BL.Helpers;
using telegramBot_Common;

namespace telegramBot_BL.Managers
{
    public class GitHubManager
    {
        private string token = CredentialsManager.Credentials.GithubAppToken;


        private GitHubClient getClient()
        {
            string jwtToken = JWTs.Create(new object(), token);
            // Use the JWT as a Bearer token
            var appClient = new GitHubClient(new ProductHeaderValue("teleBot"))
            {
                Credentials = new Octokit.Credentials(jwtToken, AuthenticationType.Bearer)
            };
            return appClient;
        }

        public async Task<IReadOnlyList<Repository>> GetRepositoriesByUser(string userName)
        {
            var client = getClient();
            return await client.Repository.GetAllForUser(userName);
        }

        public async Task<string> GetRepositoriesByTopics(string token,params string[] topics)
        {
            //TODO bulid class and change return type
            GithubSearchQueryBuilder qb = new GithubSearchQueryBuilder();
            foreach (string topic in topics)
                qb.AddCondition("topic", topic);
            qb.AddPropertyFillter("viewerHasStarred", "false");
            qb.AddPropertyFillter("viewerPermission", "READ");
            qb.SetSortCretiria("stars");
            qb.AddReturnNodeProperty("name");
            qb.AddReturnNodeProperty("description");
            qb.AddReturnNodeProperty("url");
            qb.AddReturnNodeProperty("stargazers{totalCount}");
            string query = qb.build();
            return await GithubClient.ApiV4Call(query, token);
        }

        public async Task<string> GetUserTopics(string token)
        {
            string query = "{viewer{repositories(first: 100){nodes{" +
                "repositoryTopics(first: 100){nodes{topic{name}}}}}" +
                "starredRepositories(first: 100,orderBy:" +
                "{field: STARRED_AT, direction: DESC}){nodes{" +
                "repositoryTopics(first: 100){nodes{topic{name}}}}}}";
            return await GithubClient.ApiV4Call(query, token);
        }

        public async Task<string> GetFollowedRepositories(string token)
        {
            string query = "{viewer{following(last: 100){nodes{repositories{" +
                "nodes{name,url,description,viewerHasStarred,viewerPermission}}}}}}";
            return await GithubClient.ApiV4Call(query, token);
        }
  

    }
}

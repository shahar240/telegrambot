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

        //public async Task<IReadOnlyList<Repository>> GetRepositoriesByTopics(params string[] topics)
        //{
        //    var client = getClient();
            
        //    var req = new SearchRepositoriesRequest();
        //    {InQualifier. }
        //    req.In
        //    client.Search.SearchRepo();
        //}

        private async Task<HttpResponseMessage> QueryGit(string query)
        {
            using (var http = new HttpClient())
            {
                var res = await http.PostAsync("https://api.github.com/graphql",new StringContent(query));
                return res;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace telegramBot_BL.Helpers
{
    public static class GithubClient
    {
        private const string githubApiV4Url = "https://api.github.com/graphql"; 
        public static async Task<string> ApiV4Call(string query,string token)
        {
            using(HttpClient client = createClient(token))
            {
                var content = new StringContent(query, Encoding.UTF8, "application/json");
                var res = await client.PostAsync("", content);
                if(res.StatusCode == System.Net.HttpStatusCode.OK)
                    return res.Content.ToString();
                if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    throw new ArgumentException(res.Content.ToString());
                throw new Exception(res.Content.ToString());
            }
        }

        private static HttpClient createClient(string token)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(githubApiV4Url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}

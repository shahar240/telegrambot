using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegramBot_Common
{
    public static class CredentialsManager
    {
        private static Credentials creds = null;

        public static Credentials Credentials
        {
            get
            {
                if (creds == null)
                    ReadCredentials();
                return creds;
            }
        }

        private static void ReadCredentials()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "creds.json");
            string json = File.ReadAllText(file);
            creds = JsonConvert.DeserializeObject<Credentials>(json);
        }

    }

}

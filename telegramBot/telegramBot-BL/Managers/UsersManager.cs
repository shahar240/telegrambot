using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using telegramBot_Common;
using telegramBot_DAL.Repositories.MsSql;

namespace telegramBot_BL.Managers
{
    public class UsersManager
    {
        private SqlUsersRepository _usersRepo;

        public UsersManager()
        {
            _usersRepo = new SqlUsersRepository();
        }

        public async Task<bool> EnableGitHub(int telgramId, string gitToken)
        {
            User user = new User()
            {
                gitToken = gitToken,
                Id =telgramId
            };
            return await _usersRepo.UpdateOrCreate(user);
        }

        public async Task<bool> DisableGitHub(int telgramId)
        {
            User user = new User()
            {
                gitToken = "",
                Id = telgramId
            };
            return await _usersRepo.UpdateOrCreate(user);
        }

        public async Task<bool> SetUpdateFrequency(int telgramId, int frequency)
        {
            User user = new User()
            {
                updateFrequency = frequency,
                Id = telgramId
            };
            return await _usersRepo.UpdateOrCreate(user);
        }

        public async Task<string> GetUserSettingsString(int telegramId)
        {
            User user = await _usersRepo.FindUser(telegramId);
            if (user == null)
                return "user has not registered to any service or set update frequency";
            StringBuilder settings = new StringBuilder();
            settings.Append("update frequncy: ");
            settings.AppendLine(user.updateFrequency.ToString());
            settings.AppendLine("resitered services:");

            var props = user.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name != nameof(user.Id) && prop.Name!= nameof(user.updateFrequency) &&
                    prop.Name != nameof(user.lastUpdate)&& prop.PropertyType.IsValueType &&
                    prop.CanWrite && prop.CanRead)
                {
                    object value = prop.GetValue(user);
                    if (value != null)
                    {
                        object obj = Activator.CreateInstance(prop.PropertyType);
                        if (!obj.Equals(value))
                        {
                            settings.Append("-");
                            settings.AppendLine(value.ToString());
                        }
                    }
                }
            }
            return settings.ToString();

        }
    }
}

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
    }
}

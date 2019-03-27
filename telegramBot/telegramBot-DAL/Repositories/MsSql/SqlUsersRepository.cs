using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using telegramBot_Common;

namespace telegramBot_DAL.Repositories.MsSql
{
    public class SqlUsersRepository
    {
        public async Task<bool> UpdateOrCreate(User user)
        {
            using (var ctx = new TeleBotDataContainer())
            {
                var entry = ctx.Entry(user);
                switch (entry.State)
                {
                    case EntityState.Detached:
                    case EntityState.Added:
                        ctx.Users.Add(user);
                        break;
                    case EntityState.Modified:
                        update(user, ctx);
                        break;                
                    case EntityState.Unchanged:
                        //item already in db no need to do anything  
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                int staus = await ctx.SaveChangesAsync();
                return staus > 0;
            }
        }

        private void update(User update, TeleBotDataContainer ctx)
        {
            User user = ctx.Users.Find(update.Id);
            var props = update.GetType().GetProperties();
            foreach(var prop in props)
            {
                if(prop.Name!="Id" &&prop.CanWrite && prop.CanRead)
                {
                    object value = prop.GetValue(update);
                    if (value != null)
                    {
                        object obj = Activator.CreateInstance(prop.PropertyType);
                        if (!obj.Equals(value))
                            prop.SetValue(user, value);
                    }
                }
            }
            
        }
    }
}

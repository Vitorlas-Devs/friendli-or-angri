using FriendliOrAngri.WebAPI.Data.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendliOrAngri
{
    public class Database
    {
        SQLiteAsyncConnection database;

        public Database()
        {
            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            database.CreateTableAsync<UserModel>().Wait();
        }

        public Task<List<UserModel>> GetItemsAsync()
        {
            return database.Table<UserModel>().ToListAsync();
        }

        public Task<UserModel> GetItemAsync(int id)
        {
            return database.Table<UserModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(UserModel item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(UserModel item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> DeleteAllItemsAsync()
        {
            return database.DeleteAllAsync<UserModel>();
        }

        public Task<UserModel> GetUserByToken(string token) =>
            database.Table<UserModel>().Where(u => u.Token == token).FirstOrDefaultAsync();
    }
}

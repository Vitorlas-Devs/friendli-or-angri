using FriendliOrAngri.WebAPI.Data.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FriendliOrAngri
{
    public class Database
    {
        readonly SQLiteAsyncConnection connection;

        public Database(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            //connection.CreateTableAsync<UserModel>().Wait();
        }

        public async Task<UserModel> GetUserAsync()
        {
            return await connection.Table<UserModel>().FirstOrDefaultAsync();
        }

        public async Task<int> SaveUserAsync(UserModel user)
        {
            if (user.Id != 0)
            {
                return await connection.UpdateAsync(user);
            }
            else
            {
                return await connection.InsertAsync(user);
            }
        }

        public async void DeleteDataAsync()
        {
            await connection.DropTableAsync<UserModel>();
            await connection.CreateTableAsync<UserModel>();
        }


    }
}

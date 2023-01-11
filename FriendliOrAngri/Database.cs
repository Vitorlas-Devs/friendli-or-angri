using FriendliOrAngri.Models;
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
        public readonly SQLiteAsyncConnection connection;

        public Database(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            connection.CreateTableAsync<AltUserModel>().Wait();
        }

        public async Task<AltUserModel> GetUserAsync()
        {
            return await connection.Table<AltUserModel>().FirstOrDefaultAsync();
        }

        public async Task<int> SaveUserAsync(AltUserModel user)
        {
            if (await GetUserAsync() == null)
            {
                return await connection.InsertAsync(user);
            }
            else
            {
                return await connection.UpdateAsync(user);
            }
           
        }

        public async void DeleteDataAsync()
        {
            await connection.DropTableAsync<AltUserModel>();
            await connection.CreateTableAsync<AltUserModel>();
        }


    }
}

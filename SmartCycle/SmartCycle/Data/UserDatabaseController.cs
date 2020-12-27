using SmartCycle.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartCycle.Data
{
    public class UserDatabaseController
    {

        static object locker = new object();

        SQLiteConnection database;

        // DB Setup
        public UserDatabaseController()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<User>();
        }

        // Get user details
        public User GetUser()
        {
            lock(locker)
            {
                // Check if user exists via ID
                if (database.Table<User>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return database.Table<User>().First();
                }
            }
        }

        // Save user details
        public int SaveUser(User user)
        {
            lock (locker)
            {
                // Check user via ID
                if(user.Id != 0)
                {
                    database.Update(user);
                    return user.Id;
                }
                else
                {
                    return database.Insert(user);
                }
            }
        }

        // Delete user via ID
        public int DeleteUser(int id)
        {
            lock (locker)
            {
                return database.Delete<User>(id);
            }
        }

    }
}

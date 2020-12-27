using SmartCycle.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartCycle.Data
{
    public class TokenDatabaseController
    {

        static object locker = new object();

        SQLiteConnection database;

        // DB Setup
        public TokenDatabaseController()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Token>();
        }

        // Get token details
        public Token GetToken()
        {
            lock (locker)
            {
                // Check if token exists via ID
                if (database.Table<Token>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return database.Table<Token>().First();
                }
            }
        }

        // Save token details
        public int SaveToken(Token token)
        {
            lock (locker)
            {
                // Check token via ID
                if (token.Id != 0)
                {
                    database.Update(token);
                    return token.Id;
                }
                else
                {
                    return database.Insert(token);
                }
            }
        }

        // Delete token via ID
        public int DeleteToken(int id)
        {
            lock (locker)
            {
                return database.Delete<Token>(id);
            }
        }
    }
}

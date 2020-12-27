using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCycle.Models
{
    public class User
    {
        // Assign key to classes to save
        [PrimaryKey]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User() { }
        public User(string Username, string Password) 
        {
            this.Username = Username;
            this.Password = Password;
        }

        public bool CheckInformation()
        {
            // Check if username & password have been entered
            if (!this.Username.Equals("") && !this.Password.Equals(""))
                return true;
            else
                return false;
        }

    }
}

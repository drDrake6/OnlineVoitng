using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class User
    {
        public User() { }
        public User(AccountView accountView) 
        {
            Id = 0;
            Login = accountView.login;
            Password = accountView.passwd;
            votes = new List<Voting>();
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Voting> votes { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class AccountView
    {
        public AccountView() { }
        public AccountView(string _login, string _passwd) 
        {
            login = _login;
            passwd = _passwd;
        }
        public string msg { get; set; }
        public string login { get; set; }
        public string passwd { get; set; }
    }

    public enum AccountEnum
    {
        NONE, USER, ADMIN 
    }
}

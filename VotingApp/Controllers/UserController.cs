using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingApp.Models;

namespace VotingApp.Controllers
{
    public class UserController : Controller
    {
        Context db;
        public UserController(Context context)
        {
            db = context;
        }
        public bool CheckAdmin()
        {
            string login = Content(User.Identity.Name).Content;
            User user = db.Users.FirstOrDefault(a => a.Login == login);
            if (user == null)
            {
                return false;
            }
            return true;
        }
        public IActionResult Vote()
        {
            if (!CheckAdmin())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                return View("Vote", db.Votings.ToList());
            }
        }
    }
}

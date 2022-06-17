using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VotingApp.Models;

namespace VotingApp.Controllers
{
    public class AdminController : Controller
    {
        Context db;
        public AdminController(Context context)
        {
            db = context;
        }
        public bool CheckUser()
        {
            string login = Content(User.Identity.Name).Content;
            Admin admin = db.Admins.FirstOrDefault(a => a.Login == login);
            if (admin == null)
            {
                return false;
            }
            return true;
        }
        public IActionResult Add()
        {
            if(!CheckUser())
                return RedirectToAction("Autorisation", "Account");        
            else
                return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                db.Candidates.Add(new Candidate(name));
                db.SaveChanges();
                return View();
            }
        }

        public IActionResult Change()
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                return View("Change", db.Candidates.ToList());
            }
        }

        [HttpPost]
        public IActionResult Change(int id, string name)
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                var tmp = db.Candidates.Find(id);
                db.Entry(tmp).Entity.Name = name;
                db.SaveChanges();
                return View("Change", db.Candidates.ToList());
            }
        }

        public IActionResult Delete()
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                return View("Delete", db.Candidates.ToList());
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                try
                {
                    db.Candidates.Remove(db.Candidates.Find(id));
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    List<Candidate> list = db.Candidates.ToList();
                    list.Add(null);
                    return View("Delete", list);
                }

                return View("Delete", db.Candidates.ToList());
            }
        }

        public IActionResult Show()
        {
            List <Candidate> candidates = db.Candidates.ToList();
            return View("Show", candidates);
        }

        public IActionResult CreateVoting()
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                return View("CreateVoting", db.Candidates.ToList());
            }
        }

        [HttpPost]
        public IActionResult CreateVoting(int[] ids, string name)
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                Voting voting = new Voting(name);
                for (int i = 0; i < ids.Length; i++)
                {
                    voting.AddCandidate(ids[i], db);
                }

                db.Votings.Add(voting);
                db.SaveChanges();

                return View("CreateVoting", db.Candidates.ToList());
            }
        }

        public IActionResult ShowVoting()
        {
            return View("ShowVoting", db.Votings.ToList());
        }

        public IActionResult ChangeVoting()
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                return View("ChangeVoting", db.Votings.ToList());
            }
        }

        public IActionResult DeleteVoting()
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                return View("DeleteVoting", db.Votings.ToList());
            }
        }

        [HttpPost]
        public IActionResult DeleteVoting(int id)
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                var tmp = db.Votings.Find(id);
                db.Entry(tmp).Collection(v => v.results).Load();
                db.Entry(tmp).Entity.RemoveAllResults(db);
                db.Votings.Remove(tmp);
                db.SaveChanges();

                return View("DeleteVoting", db.Votings.ToList());
            }
        }
    }
}

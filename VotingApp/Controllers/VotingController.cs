using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using VotingApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : Controller
    {
        Context db;
        public VotingController(Context context)
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
        // PosT api/votingapp/
        [HttpPost]
        public async Task<ActionResult<Voting>> Post(VotingView voting)
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                // если есть лшибки - возвращаем ошибку 400
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!db.Votings.Any(x => x.Id == voting.Id))
                {
                    return NotFound();
                }

                var tmp = db.Votings.Find(voting.Id);
                await db.Entry(tmp).Collection(v => v.results).LoadAsync();
                await db.Entry(tmp).Collection(v => v.mates).LoadAsync();

                db.Entry(tmp).Entity.Name = voting.Name;
                if (voting.Reset)
                {
                    foreach (var item in db.Entry(tmp).Entity.results)
                    {
                        item.Votes = 0;
                    }
                    db.Entry(tmp).Entity.mates.Clear();
                }
                await db.SaveChangesAsync();
                return Ok(tmp);
            }
        }
    }
}

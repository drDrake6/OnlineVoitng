using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
    public class ResultController : Controller
    {
        Context db;
        public ResultController(Context context)
        {
            db = context;
        }
        public bool CheckAdmin()
        {
            string login = Content(User.Identity.Name).Content;
            Admin admin = db.Admins.FirstOrDefault(a => a.Login == login);
            if (admin == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckUser()
        {
            string login = Content(User.Identity.Name).Content;
            User user = db.Users.FirstOrDefault(a => a.Login == login);
            if (user == null)
            {
                return false;
            }
            return true;
        }
        // GET api/result/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voting>> Get(int id)
        {
            Voting voting = db.Votings.Find(id);
            await db.Entry(voting).Collection(v => v.results).LoadAsync();
            List<Result> results = voting.results;
            foreach (var item in results)
            {
                await db.Entry(item).Reference(r => r.candidate).LoadAsync();
            }
            if (results == null)
                return NotFound();
            return new ObjectResult(results);
        }

        // PUT api/result/
        [HttpPut]
        public async Task<ActionResult<ResultView>> Put(ResultView result)
        {
            if (!CheckAdmin())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                // если есть лшибки - возвращаем ошибку 400
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!db.Votings.Any(x => x.Id == result.Id))
                {
                    return NotFound();
                }

                Voting voting = db.Votings.Find(result.VotingId);
                await db.Entry(voting).Collection(v => v.results).LoadAsync();
                List<Result> results = voting.results;
                foreach (var item in results)
                {
                    if (item.Id == result.Id)
                    {
                        await db.Entry(item).Reference(r => r.candidate).LoadAsync();
                        item.candidate.Name = result.candidate;
                        item.Votes = result.Votes;
                        break;
                    }
                }

                await db.SaveChangesAsync();
                return Ok(result);
            }
        }

        // DELETE api/result/2
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            if (!CheckAdmin())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                Result result = db.Results.Find(id);
                if (result == null)
                {
                    return NotFound();
                }
                db.Results.Remove(result);
                await db.SaveChangesAsync();
                return Ok(result);
            }
        }

        // POST api/result
        [HttpPost]
        public async Task<ActionResult<Result>> Post(ResultView resultView)
        {
            if (!CheckAdmin())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                Result result = db.Entry(db.Votings.Find(resultView.VotingId)).Entity.AddCandidate(resultView.Id, db);
                await db.SaveChangesAsync();
                return Ok(result);
            }
        }

        //POST api/result/3
        [HttpPost("{id}")]
        public async Task<ActionResult<Result>> Post(int id)
        {
            if (!CheckUser())
                return RedirectToAction("Autorisation", "Account");
            else
            {
                Result result = db.Results.Find(id);
                Voting voting = await db.Votings.FirstOrDefaultAsync(v => v.results.Contains(result));
                string login = Content(User.Identity.Name).Content;
                User user = db.Users.FirstOrDefault(el => el.Login == login);
                await db.Entry(user).Collection(v => v.votes).LoadAsync();
                if (!user.votes.Contains(voting))
                {
                    db.Entry(result).Entity.Vote();
                    user.votes.Add(voting);
                    await db.SaveChangesAsync();
                    return Ok(result);
                }
                else
                {
                    ModelState.AddModelError("overvote", "Вы уже проголосовали!");
                    return BadRequest(ModelState);
                }
            }
        }
    }

}

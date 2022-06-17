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
    public class CandidateController : Controller
    {
        Context db;
        public CandidateController(Context context)
        {
            db = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Candidate>> Get()
        {
            return await db.Candidates.ToListAsync();
        }
    }
}

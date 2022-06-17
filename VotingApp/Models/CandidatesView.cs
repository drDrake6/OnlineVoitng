using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class CandidatesView
    {
        public List<Candidate> candidates { get; set; }
        CandidatesView()
        {
            candidates = new List<Candidate>();
        }
    }
}

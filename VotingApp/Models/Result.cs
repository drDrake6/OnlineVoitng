using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class Result
    {
        public Result() { }
        public Result(Candidate _candidate, int _votes)
        {
            candidate = _candidate;
            Votes = _votes;
        }
        public int Id { get; set; }
        public Candidate candidate { get; set; }
        public int Votes { get; set; }

        public void Vote()
        {
            Votes++;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class Voting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Result> results { get; set; }
        public List<User> mates { get; set; }

        public Voting()
        {
            results = new List<Result>();
            mates = new List<User>();
        }

        public Voting(string name) : this()
        {
            Name = name;
        }

        public Result AddCandidate(int id, Context db)
        {
            Result result = new Result(db.Candidates.Find(id), 0);
            results.Add(result);
            return result;
        }

        public void RemoveAllResults(Context db)
        {

            foreach (var item in results)
            {
                db.Remove(db.Results.Find(item.Id));
            }
        }

        public bool Vote(Candidate candidate, User user)
        {
            if(!user.votes.Contains(this))
            {
                results.Find(x => x.candidate.Id == candidate.Id).Vote();
                user.votes.Add(this);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NoVotes()
        {
            foreach (var item in results)
            {
                if (item.Votes != 0)
                    return false;
            }
            return true;
        }

        public bool DeadHeat()
        {
            for (int i = 1; i < results.Count(); i++)
            {
                if(results[i].Votes != results[0].Votes)
                    return false;
            }
            return true;
        }
    }
}

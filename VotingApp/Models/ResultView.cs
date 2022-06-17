using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class ResultView
    {       
        public int Id { get; set; }
        public int VotingId { get; set; }
        public string candidate { get; set; }
        public int Votes { get; set; }
    }
}

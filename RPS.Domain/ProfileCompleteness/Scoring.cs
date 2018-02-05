using System;
using System.Collections.Generic;
using System.Text;

namespace RPS.Domain.ProfileCompleteness
{
    public class Scoring
    {

        public int ScoringId { get; set; }

        public int CompanyFK { get; set; }

        public DateTime RecordedOn { get; set; }

        public double Score { get; set; }

        public double Change { get; set; }

        public int RuleVersion { get; set; }

        public List<string> Explanations { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modifed { get; set; }


    }
}

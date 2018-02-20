using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Presentation.Server.Models
{
    public class ScoringType : ObjectGraphType<Scoring>
    {

        public ScoringType()
        {
            Field(x => x.Change).Description("Score Change");
            Field(x => x.Score).Description("Score");
        }
    }
}

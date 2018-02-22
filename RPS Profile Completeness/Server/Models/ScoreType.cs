using GraphQL.Types;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Presentation.Server.Models
{
    public class ScoreType : ObjectGraphType<ScoreResult>
    {

        public ScoreType()
        {
            Field(x => x.Change).Description("Score Change");
            Field(x => x.Score).Description("Score");
        }
    }
}

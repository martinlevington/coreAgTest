using GraphQL.Types;

namespace RPS.Presentation.Server.Models
{
    public class ScoringType : ObjectGraphType<ScoreResult>
    {

        public ScoringType()
        {
            Field(x => x.Change).Description("Score Change");
            Field(x => x.Score).Description("Score");
        }
    }
}

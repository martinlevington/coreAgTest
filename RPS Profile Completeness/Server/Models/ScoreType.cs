using GraphQL.Types;

namespace RPS.Presentation.Server.Models
{
    public class ScoreType : ObjectGraphType<ScoreResult>
    {

        public ScoreType()
        {
            Field(x => x.Change).Description("Score Change");
            Field(x => x.Score).Description("Score");
            Field(x => x.RecordedOn).Description("RecordedOn");
        }
    }
}

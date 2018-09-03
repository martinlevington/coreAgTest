namespace RPS.Presentation.Server.Models
{
    public class MonthlyScoreResult
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public ScoreResult[] Scores { get; set; }
    }
}

using System.Collections.Generic;
using RPS.Domain.Core.Time;
using RPS.Domain.Data;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Application.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IScoringRepository _scoringRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DashboardService(IScoringRepository scoringRepository, IDateTimeProvider dateTimeProvider)
        {
            _scoringRepository = scoringRepository;
            _dateTimeProvider = dateTimeProvider;
        }


        public List<Scoring> GetMonthlyScores(MonthlyScoreRequest monthlyScoreRequest)
        {
            var result = _scoringRepository.GetTopImprovers(monthlyScoreRequest.NumberOfRecords, monthlyScoreRequest.StartPeriod);
            return result;
        }

   
        public List<Scoring> GetMonthlyAverageScores(ScoreRequest scoreRequest)
        {
            var result = _scoringRepository.GetMonthlyAverage(scoreRequest.NumberOfRecords, 6);
            return result;
        }

    }
}

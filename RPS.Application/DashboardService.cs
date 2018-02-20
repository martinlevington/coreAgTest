using System.Collections.Generic;
using RPS.Application.Dashboard;
using RPS.Domain.Core.Time;
using RPS.Domain.Data;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Application
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
            return _scoringRepository.GetTopImprovers(monthlyScoreRequest.NumberOfRecords, monthlyScoreRequest.StartPeriod);
        }

    }
}

using System.Collections.Generic;
using RPS.Application.Dashboard;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Application
{
  public interface IDashboardService
  {
    List<Scoring> GetMonthlyScores(MonthlyScoreRequest monthlyScoreRequest);
  }
}
using System.Collections.Generic;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Application.Dashboard
{
  public interface IDashboardService
  {
    List<Scoring> GetMonthlyScores(MonthlyScoreRequest monthlyScoreRequest);
  
  }
}
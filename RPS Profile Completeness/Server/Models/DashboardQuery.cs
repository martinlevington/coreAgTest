using System;
using GraphQL.Types;
using RPS.Application;
using RPS.Application.Dashboard;

namespace RPS.Presentation.Server.Models
{
    public class DashboardQuery : ObjectGraphType
    {

        public DashboardQuery(IDashboardService dashboardService)
        {
            //var range = DateTime.Parse("2017-11-24");
            //var monthlyScoreRequest = new MonthlyScoreRequest()
            //{
            //    StartPeriod  = range,
            //    NumberOfRecords = 10
            //};

            Field<ScoringType>(
                "monthly",
                resolve: context => dashboardService.GetMonthlyScores(new MonthlyScoreRequest()
                {
                    StartPeriod  = DateTime.Parse("2017-11-24"),
                    NumberOfRecords = 10
                })
            );

            Field<ListGraphType<ScoringType>>(
                "monthlys",
                resolve: context => dashboardService.GetMonthlyScores(new MonthlyScoreRequest()
                {
                    StartPeriod  = DateTime.Parse("2017-11-24"),
                    NumberOfRecords = 10
                })
            );
        }

    }
}

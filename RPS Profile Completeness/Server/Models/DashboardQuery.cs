using System;
using System.Collections.Generic;
using AutoMapper;
using GraphQL.Types;
using RPS.Application;
using RPS.Application.Dashboard;

namespace RPS.Presentation.Server.Models
{
    public class DashboardQuery : ObjectGraphType
    {

        public DashboardQuery(IDashboardService dashboardService, IMapper mapper)
        {
            //var range = DateTime.Parse("2017-11-24");
            //var monthlyScoreRequest = new MonthlyScoreRequest()
            //{
            //    StartPeriod  = range,
            //    NumberOfRecords = 10
            //};

            Field<ListGraphType<ScoreType>>(
                "monthly",
                resolve: context =>   
            {
                var results = dashboardService.GetMonthlyScores(new MonthlyScoreRequest()
                {
                    StartPeriod = DateTime.Parse("2017-11-24"),
                    NumberOfRecords = 10
                });

                var mapped = mapper.Map<IEnumerable<ScoreResult>>(results);
                return mapped;
            });



            Field<ScoreType>(
                "monthlyscore");

            //Field<ScoringType>(
            //    "monthlys",
            //    resolve: context =>
            //    {
            //        var results = dashboardService.GetMonthlyScores(new MonthlyScoreRequest()
            //        {
            //            StartPeriod = DateTime.Parse("2017-11-24"),
            //            NumberOfRecords = 10
            //        });

            //        var mapped = mapper.Map<IEnumerable<ScoreResult>>(results);
            //        return mapped;
            //    });


        }

    }
}

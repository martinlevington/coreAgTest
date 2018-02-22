using System;
using System.Collections.Generic;
using AutoMapper;
using GraphQL.Types;
using RPS.Application;
using RPS.Application.Dashboard;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Presentation.Server.Models
{
    public class MonthlyScore : ObjectGraphType<MonthlyScoreResult>
    {

        public MonthlyScore(IDashboardService dashboardService, IMapper mapper)
        {

            Name = "MonthlyScore";


          
            Field<ListGraphType<ScoreType>>(
                "scores",
                resolve: context =>
                {
                    var results = dashboardService.GetMonthlyScores(new MonthlyScoreRequest()
                    {
                        StartPeriod = DateTime.Parse("2017-11-24"),
                        NumberOfRecords = 10
                    });

                    var mapped = mapper.Map<IEnumerable<ScoreResult>>(results);
                    return mapped;
                }
            );
        }
    }
}

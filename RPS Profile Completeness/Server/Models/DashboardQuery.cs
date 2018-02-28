using System;
using System.Collections.Generic;
using AutoMapper;
using GraphQL.Types;
using RPS.Application.Dashboard;

namespace RPS.Presentation.Server.Models
{
    public class DashboardQuery : ObjectGraphType
    {

        public DashboardQuery(IDashboardService dashboardService, IMapper mapper)
        {
     

            Field<ListGraphType<ScoreType>>(
                "monthlyscores",
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



          

        }

    }
}

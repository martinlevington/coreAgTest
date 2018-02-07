using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RPS.Domain.Data;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Data.Elasticsearch.ProfileCompleteness
{
    public class ScoringRepository : IScoringRepository
    {
        private readonly IElasticSearchContext _elasticSearchContext;
        private readonly string _indexName = nameof(Scoring).ToLower();
        private readonly IOptions<ElasticSearchConfiguration> _optionsApplicationConfiguration;


        public ScoringRepository(IElasticSearchContext elasticSearchContext,
            IOptions<ElasticSearchConfiguration> options)
        {
            _elasticSearchContext = elasticSearchContext;
            _optionsApplicationConfiguration = options;
        }


        public void AddAllData(string filePath)
        {
            var format = "dd/MM/yyyy HH:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter {DateTimeFormat = format};

            var data = JsonConvert.DeserializeObject<List<Scoring>>(File.ReadAllText(filePath), dateTimeConverter);

            var waitHandle = new CountdownEvent(1);

            var bulkAll = _elasticSearchContext.GetClient().BulkAll(data, b => b
                .Index(_indexName)
                .BackOffRetries(2)
                .BackOffTime("30s")
                .RefreshOnCompleted(true)
                .MaxDegreeOfParallelism(4)
                .Size(1000)
            );

            bulkAll.Subscribe(new BulkAllObserver(
                b => { Console.Write("."); },
                e => throw e,
                () => waitHandle.Signal()
            ));
            waitHandle.Wait();
        }

        public void UpdateAllData(string filePath)
        {
            var format = "dd/MM/yyyy HH:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

            var data = JsonConvert.DeserializeObject<List<Scoring>>(File.ReadAllText(filePath), dateTimeConverter);

            var waitHandle = new CountdownEvent(1);

  
            
    

            foreach (var item in data)
            {
                var task = _elasticSearchContext.GetClient().Update<Scoring>(
                new DocumentPath<Scoring>(item), u =>
                u.Index(_indexName)
           
                    .Doc(item));
            }

    //var bulkAll = _elasticSearchContext.GetClient().BulkAll(data, b => b
    //             .Refresh(Refresh.True)
    //            .Index(_indexName)
    //            .BackOffRetries(2)
    //            .BackOffTime("30s")
    //            .RefreshOnCompleted(true)
    //            .MaxDegreeOfParallelism(4)
    //            .Size(1000)
    //        );

    //        bulkAll.Subscribe(new BulkAllObserver(
    //            b => { Console.Write("."); },
    //            e => throw e,
    //            () => waitHandle.Signal()
    //        ));
            waitHandle.Wait();
        }

        public void DeleteIndex()
        {
            if (_elasticSearchContext.GetClient().IndexExists(_indexName).Exists)
            {
                _elasticSearchContext.GetClient().DeleteIndex(_indexName);
            }
        }

        public List<Scoring> Get(int resultSize)
        {
            var result = _elasticSearchContext.GetClient().Search<Scoring>(
                s => s.Index(_indexName)
                    .From(0)
                    .Size(resultSize)
                    .Query(q => q.MatchAll()
                    )
                    );

            return result.Documents.ToList();
        }

        public List<Scoring> GetTopImprovers(int resultSize, DateTime StartPeriod)
        {
            var results = new List<Scoring>();

            // get scores imporved in last week

            //var result = _elasticSearchContext.GetClient().Search<Scoring>(
            //    s => s.Index(_indexName)
            //        .From(0)
            //        .Size(resultSize)
            //        .Aggregations(aggs => aggs
            //            .DateRange("max_min_over_range", date => date
            //                    .Field(f => f.RecordedOn)
            //                    .Ranges(
            //                        r => r.From(DateMath.Anchored(StartPeriod).Subtract("30d"))
            //                            .To(DateMath.Anchored(StartPeriod).Add("30d")),
            //                        r => r.To(DateMath.Now.Add(TimeSpan.FromDays(1))))
            //                    .Aggregations(childAggs => childAggs
            //                        .Max("maxScore", avg => avg.Field(p => p.Score))
            //                        .Min("minScore", avg => avg.Field(p => p.Score))
            //                    )

            //                // .Max("maxScore", avg => avg.Field(p => p.Score))
            //                // .Min("minScore", avg => avg.Field(p => p.Score))
            //                .TimeZone("CET")
            //            )
            //        )
            //        .Query(q => q.MatchAll()
            //        )
            //);

            var result = _elasticSearchContext.GetClient().Search<Scoring>(
                s => s.Index(_indexName)
                    .From(0)
                    .Size(resultSize)

                    .Aggregations(aggs => aggs
                        .Terms("group_company", t => t.Field(f => f.CompanyFK))
                        .DateHistogram("serial_diff", 
                            dh => dh
                                .Field(p => p.RecordedOn)
                                .Interval(DateInterval.Week)
                                .Aggregations(aa => aa
                                    .Sum("rpsscore", sm => sm.Field(p => p.Score))
                                .SerialDifferencing("score_diff", d=>d.BucketsPath("rpsscore"))
                                   

                            )
                                )
                        .DateRange("max_min_over_range", date => date
                            .Field(f => f.RecordedOn)
                            .Ranges(
                                r => r.From(DateMath.Anchored(StartPeriod).Subtract("10d"))
                                    .To(DateMath.Anchored(StartPeriod).Add("2d")))
                                
                            .Aggregations(childAggs => childAggs
                                .Max("maxScore", avg => avg.Field(p => p.Score))
                                .Min("minScore", avg => avg.Field(p => p.Score))
                            )


                        )
                        )



                    //.DateRange("max_min_over_range", date => date
                    //    .Field(f => f.RecordedOn)
                    //    .Ranges(
                    //        r => r.From(DateMath.Anchored(StartPeriod).Subtract("30d"))
                    //            .To(DateMath.Anchored(StartPeriod).Add("30d")),
                    //        r => r.To(DateMath.Now.Add(TimeSpan.FromDays(1))))
                    //    .Aggregations(childAggs => childAggs
                    //        .Max("maxScore", avg => avg.Field(p => p.Score))
                    //        .Min("minScore", avg => avg.Field(p => p.Score))
                    //    )

                    //    // .Max("maxScore", avg => avg.Field(p => p.Score))
                    //    // .Min("minScore", avg => avg.Field(p => p.Score))
                    //    .TimeZone("CET")
                    //)

                    .Query(q => q.MatchAll()
                    )
            );


            results.AddRange(result.Documents);


            return results;
        }
    }
}
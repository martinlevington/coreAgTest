using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;



        public ScoringRepository(IElasticSearchContext elasticSearchContext,
            IOptions<ElasticSearchConfiguration> options,
            ILogger<ScoringRepository> logger)
        {
            _elasticSearchContext = elasticSearchContext;
            _optionsApplicationConfiguration = options;
            _logger = logger;

        }

        /// <summary>
        /// Adds all data to db in bulk.
        /// Will also update records if ids math in records
        /// </summary>
        /// <param name="filePath">The file path.</param>
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
            const string format = "dd/MM/yyyy HH:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

            var data = JsonConvert.DeserializeObject<List<Scoring>>(File.ReadAllText(filePath), dateTimeConverter);

            var waitHandle = new CountdownEvent(1);

            foreach (var item in data)
            {
                _elasticSearchContext.GetClient().Update<Scoring>(
                new DocumentPath<Scoring>(item), u =>
                u.Index(_indexName)
           
                    .Doc(item));
            }
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

        public List<Scoring> GetTopImprovers(int resultSize, DateTime startPeriod)
        {
            var results = new List<Scoring>();

            // get scores imporved in last week

            var result = _elasticSearchContext.GetClient().Search<Scoring>(
                search => search.Index(_indexName)
                    .From(0)
                    .Size(resultSize)
                    
                    .Aggregations(a => a
                        // simplify the terms aggregation
                        .Terms("query", tr => tr
                            .Field("CompanyFK")
                            .Size(30)
                        )
                        // Add the top hits aggregation
                        .TopHits("top", th => th
                            .Size(1)
                        )
                  
                    )
                  .Sort(s => s
                    .Field(f => 
                            f.Field(p => p.Change)
                            .Order(SortOrder.Descending)                    
                        )

                    )
                    .Query(q => q
                        .DateRange(r => r
                            .Field(f => f.RecordedOn)
                            .GreaterThanOrEquals(DateMath.Anchored(startPeriod).Subtract("7d"))
                            .LessThanOrEquals(DateMath.Anchored(startPeriod))
                        )
                    
                    )
            );

    
     
            results.AddRange(result.Documents);

            return results;
        }

        public List<Scoring> GetMonthlyAverage(int resultSize, int numberOfMonths)
        {
          

            var results = new List<Scoring>();

            // get scores imporved in last week

            var response = _elasticSearchContext.GetClient().Search<Scoring>(
                search => search.Index(_indexName)
                   // .From(0)
                   // .Size(resultSize)
                    .Size(0)
               //     .TypedKeys(null)
                    .Aggregations(a => a
                        // simplify the terms aggregation
                        //.Terms("query", tr => tr
                        //    .Field("CompanyFK")
                        //    .Size(30)
                        //)
                        
                        // Add the top hits aggregation
                        //.TopHits("top", th => th
                        //    .Size(1)
                        //)
                        .DateHistogram("avg_per_month",
                            ag => ag.Field("recordedOn")
                                .Interval(DateInterval.Month)
                            .Aggregations(
                                aggs => aggs.Average("avg_score", dField => dField.Field(field => field.Score)))
                        )
                    )
                    //.Sort(s => s
                    //    .Field(f => 
                    //        f.Field(p => p.Change)
                    //            .Order(SortOrder.Descending)                    
                    //    )

                    //)
                    //.Query(q => q
                    //    .DateRange(r => r
                    //        .Field(f => f.RecordedOn)
                    //        .GreaterThanOrEquals(DateMath.Anchored(DateTime.Now).Subtract(numberOfMonths+"d"))
                    //        .LessThanOrEquals(DateMath.Anchored(DateTime.Now))
                    //    )
                    
                    //)
            );
     
            results.AddRange(response.Documents);

            _logger.LogDebug(response.ApiCall.DebugInformation);

            var dateHistogram = response.Aggregations.DateHistogram("avg_per_month");

            foreach (var item in dateHistogram.Buckets)
            {
                var avg = item.Average("avg_score");

                if (avg.Value != null)
                {
                    results.Add(new Scoring()
                    {
                        Score = (double) avg.Value,
                        RecordedOn = item.Date
                    });
                }
            }

            return results;
        }
    }
}
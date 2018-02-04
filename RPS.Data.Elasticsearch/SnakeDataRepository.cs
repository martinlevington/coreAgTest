using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using RPS.Domain.Snakes;

namespace RPS.Data.Elasticsearch
{
    public class SnakeDataRepository  : ISnakeDataRepository
    {
        private readonly string _connectionString;
        private IElasticSearchContext _elasticSearchContext;


        private readonly IOptions<ElasticSearchConfiguration> _optionsApplicationConfiguration;

        public SnakeDataRepository(IOptions<ElasticSearchConfiguration> options, IElasticSearchContext elasticSearchContext1)
        {
            _optionsApplicationConfiguration = options;
            _elasticSearchContext = elasticSearchContext1;
            _connectionString = _optionsApplicationConfiguration.Value.ElasticsearchUri;

           
        }


        //public List<GeographicalRegion> GetGeographicalRegions()
        //{
        //    List<GeographicalRegion> geographicalRegions = new List<GeographicalRegion>();
        //    var search = new Search
        //    {
        //        Size = 0,
        //        Aggs = new List<IAggs>
        //        {
        //            new TermsBucketAggregation("getgeographicalregions", "geographicalregion")
        //            {
        //                Aggs = new List<IAggs>
        //                {
        //                    new SumMetricAggregation("countCases", "numberofcaseshigh"),
        //                    new SumMetricAggregation("countDeaths", "numberofdeathshigh")
        //                }
        //            }
        //        }
        //    };

        //    using (var context = new ElasticsearchContext(_connectionString))
        //    {
        //        var items = context.Search<SnakeBites>(search);

        //        try
        //        {
        //            var aggResult =
        //                items.PayloadResult.Aggregations.GetComplexValue<TermsBucketAggregationsResult>(
        //                    "getgeographicalregions");

        //            foreach (var bucket in aggResult.Buckets)
        //            {
        //                var cases = Math.Round(bucket.GetSingleMetricSubAggregationValue<double>("countCases"), 2);
        //                var deaths = Math.Round(bucket.GetSingleMetricSubAggregationValue<double>("countDeaths"), 2);
        //                geographicalRegions.Add(
        //                    new GeographicalRegion
        //                    {
        //                        Countries = bucket.DocCount,
        //                        Name = bucket.Key.ToString(),
        //                        NumberOfCasesHigh = cases,
        //                        NumberOfDeathsHigh = deaths,
        //                        DangerHigh = (deaths > 1000)
        //                    });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }

        //    return geographicalRegions;
        //}

        //public GeographicalCountries GetBarChartDataForRegion(string region)
        //{
        //    GeographicalCountries result = new GeographicalCountries {RegionName = region};

        //    var search = new Search
        //    {
        //        Query = new Query(new MatchQuery("geographicalregion", region)),
        //        Size = 100
        //    };

        //    using (var context = new ElasticsearchContext(_connectionString, _elasticsearchMappingResolver))
        //    {
        //        var items = context.Search<SnakeBites>(search);

        //        result.NumberOfCasesHighData = new BarTrace {Y = new List<double>()};
        //        result.NumberOfCasesLowData = new BarTrace {Y = new List<double>()};
        //        result.NumberOfDeathsHighData = new BarTrace {Y = new List<double>()};
        //        result.NumberOfDeathsLowData = new BarTrace {Y = new List<double>()};
        //        result.X = new List<string>();

        //        foreach (var item in items.PayloadResult.Hits.HitsResult)
        //        {
        //            result.NumberOfCasesHighData.Y.Add(item.Source.NumberOfCasesHigh);
        //            result.NumberOfCasesLowData.Y.Add(item.Source.NumberOfCasesLow);
        //            result.NumberOfDeathsHighData.Y.Add(item.Source.NumberOfDeathsHigh);
        //            result.NumberOfDeathsLowData.Y.Add(item.Source.NumberOfDeathsLow);

        //            result.X.Add(item.Source.Country);
        //        }
        //    }

        //    return result;
        //}

        public List<SnakeBites> GetGeographicalRegions()
        {
            List<SnakeBites> geographicalRegions = new List<SnakeBites>();


            var result = _elasticSearchContext.GetClient().Search<SnakeBites>(
                s => s.From(0)
                .Size(10)
                .Sort(sort => { return sort.Descending("_score"); })
	
				.Aggregations(aggs => aggs
	
			  
									.Sum("countCases", avg => avg.Field(p => p.NumberOfCasesHigh))
					                .Sum("countDeaths", avg => avg.Field(p => p.NumberOfDeathsHigh))
				                )
		                
				.Query( q => q.MatchAll()
	                )
                    
                    );
	     

		 //   var cases = Math.Round(result.Fields["countCases"], 2);
		 //  var deaths = Math.Round(result.GetSingleMetricSubAggregationValue<double>("countDeaths"), 2);

			geographicalRegions.AddRange( result.Documents);


			return geographicalRegions;
        }

        public GeographicalCountries GetBarChartDataForRegion(string machineName)
        {
            throw new NotImplementedException();
        }

        public void AddAllData()
        {
            List<SnakeBites> data = JsonConvert.DeserializeObject<List<SnakeBites>>(File.ReadAllText(_optionsApplicationConfiguration.Value.FilePath));


            var waitHandle = new CountdownEvent(1);

            var bulkAll = _elasticSearchContext.GetClient().BulkAll(data, b => b
                .Index(_elasticSearchContext.CurrentIndexName)
                .BackOffRetries(2)
                .BackOffTime("30s")
                .RefreshOnCompleted(true)
                .MaxDegreeOfParallelism(4)
                .Size(1000)
            );

            bulkAll.Subscribe(new BulkAllObserver(
                onNext: (b) => { Console.Write("."); },
                onError: (e) => { throw e; },
                onCompleted: () => waitHandle.Signal()
            ));
            waitHandle.Wait();


        }

        //private double GetBucketValue(RangeBucket bucket, string key)
        //{
        //    var res = bucket.GetSingleMetricSubAggregationValue<double?>(key);

        //    return (Math.Round(res.GetValueOrDefault(0.0), 2) * 100);
        //}
    }
}
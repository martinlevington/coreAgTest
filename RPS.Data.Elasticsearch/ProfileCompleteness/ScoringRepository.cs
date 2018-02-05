using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
    private readonly IOptions<ElasticSearchConfiguration> _optionsApplicationConfiguration;
    private readonly string _indexName = nameof(Scoring).ToLower();


    public ScoringRepository(IElasticSearchContext elasticSearchContext, IOptions<ElasticSearchConfiguration> options)
    {
      _elasticSearchContext = elasticSearchContext;
      _optionsApplicationConfiguration = options;
    }


    public void AddAllData(string filePath)
    {

      var format = "dd/MM/yyyy HH:mm:ss"; // your datetime format
      var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

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
  }
}
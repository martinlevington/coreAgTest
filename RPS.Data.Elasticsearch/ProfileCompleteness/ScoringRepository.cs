using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Data.Elasticsearch.ProfileCompleteness
{
    public class ScoringRepository
    {
        private readonly IElasticSearchContext _elasticSearchContext;


        public ScoringRepository( IElasticSearchContext elasticSearchContext)
        {
            _elasticSearchContext = elasticSearchContext;
        }


      public void AddAllData(string filePath)
      {
        List<Scoring> data = JsonConvert.DeserializeObject<List<Scoring>>(File.ReadAllText(filePath));


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


  }
}

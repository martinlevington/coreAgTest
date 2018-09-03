using Microsoft.Extensions.Options;
using Nest;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Data.Elasticsearch
{
    public class ElasticSearchContext : IElasticSearchContext
    {
       
        private readonly IElasticClient _client;
 

        public ElasticSearchContext(IElasticClient client, IOptions<ElasticSearchConfiguration> configuration)
        {

            _client = client;
            CurrentIndexName = configuration.Value.IndexName;

        }

        public string CurrentIndexName { get; set; }

        public IElasticClient GetClient()
        {
         
            if (!_client.IndexExists(CurrentIndexName).Exists)
            {
                CreateIndex();
            }

            return _client;
        }

        private void CreateIndex()
        {
            // default


            _client.CreateIndex(nameof(Scoring).ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(2)
                    .NumberOfReplicas(0)
                    .Analysis(ScoringMap.Analysis)
                )
                .Mappings(m => m
                    .Map<Scoring>(ScoringMap.MapPackage)
                )
            );
        }


        public void DeleteIndexIfExists()
        {
            if (_client.IndexExists(CurrentIndexName).Exists)
            {
                _client.DeleteIndex(CurrentIndexName);
            }
        }
    }
}
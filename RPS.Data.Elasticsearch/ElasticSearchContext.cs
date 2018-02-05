using System;
using Microsoft.Extensions.Options;
using Nest;
using RPS.Domain.ProfileCompleteness;
using RPS.Domain.Snakes;

namespace RPS.Data.Elasticsearch
{
  

    public class ElasticSearchContext : IElasticSearchContext
    {

        private  Uri node;
        public ConnectionSettings Settings;
        public string CurrentIndexName { get; set; }
        private ElasticClient _client;

        public ElasticSearchContext(IOptions<ElasticSearchConfiguration> configuration)
        {
            node = new Uri(configuration.Value.ElasticsearchUri);
            Settings = new ConnectionSettings(node);
            CurrentIndexName = configuration.Value.IndexName;

            // make sure index is created

        }

        public  ElasticClient GetClient()
        {
            
            Settings.DefaultIndex(CurrentIndexName);
            _client = new ElasticClient(Settings);

            if (!_client.IndexExists(CurrentIndexName).Exists)
            {
                CreateIndex();
            }

            return _client;
        }

        private void CreateIndex()
        {
            // default
            _client.CreateIndex(CurrentIndexName, i => i
                .Settings(s => s
                    .NumberOfShards(2)
                    .NumberOfReplicas(0)
                    .Analysis(SnakeBitesMap.Analysis)
                )

                .Mappings(m => m
                    .Map<SnakeBites>(SnakeBitesMap.MapPackage)
                )
            );

            
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

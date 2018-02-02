using System;
using Microsoft.Extensions.Options;
using Nest;

namespace RPS.Data.Elasticsearch
{
  

    public class ElasticSearchContext : IElasticSearchContext
    {

        private  Uri node;
        public ConnectionSettings Settings;

        public ElasticSearchContext(IOptions<ElasticSearchConfiguration> configuration)
        {
            node = new Uri(configuration.Value.ElasticsearchUri);
            Settings = new ConnectionSettings(node);


        }

        public  ElasticClient GetClient(string indexName="netcore")
        {
           
            Settings.DefaultIndex(indexName);
            var client = new ElasticClient(Settings);

            return client;
        }

       
    }
}

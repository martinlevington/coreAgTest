using Nest;

namespace RPS.Data.Elasticsearch
{
    public interface IElasticSearchContext
    {
        IElasticClient GetClient();
        string CurrentIndexName { get; set; }
    }
}

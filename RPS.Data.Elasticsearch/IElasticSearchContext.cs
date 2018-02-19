using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace RPS.Data.Elasticsearch
{
    public interface IElasticSearchContext
    {
        IElasticClient GetClient();
        string CurrentIndexName { get; set; }
    }
}

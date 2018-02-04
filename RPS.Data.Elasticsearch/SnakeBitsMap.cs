using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using RPS.Domain.Snakes;

namespace RPS.Data.Elasticsearch
{
    public static class SnakeBitsMap
    {
        public static TypeMappingDescriptor<SnakeBites> MapPackage(TypeMappingDescriptor<SnakeBites> map) => map
            .AutoMap();



        public static AnalysisDescriptor Analysis(AnalysisDescriptor analysis) => analysis
            
            .Analyzers(analyzers => analyzers
                .Standard("standard_english", sa => sa.StopWords("_english_"))
                );


    }
}

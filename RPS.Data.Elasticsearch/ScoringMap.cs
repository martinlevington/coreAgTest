using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using RPS.Domain.ProfileCompleteness;
using RPS.Domain.Snakes;

namespace RPS.Data.Elasticsearch
{
    public static class ScoringMap
    {
        public static TypeMappingDescriptor<Scoring> MapPackage(TypeMappingDescriptor<Scoring> map) => map
            .AutoMap()
        ;



        public static AnalysisDescriptor Analysis(AnalysisDescriptor analysis) => analysis
            
            .Analyzers(analyzers => analyzers
                .Standard("standard_english", sa => sa.StopWords("_english_"))
                );


    }
}

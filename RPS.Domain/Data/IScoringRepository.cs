using System;
using System.Collections.Generic;
using RPS.Domain.ProfileCompleteness;

namespace RPS.Domain.Data
{
  public interface IScoringRepository
  {
    void AddAllData(string filePath);
      List<Scoring> GetTopImprovers(int resultSize, DateTime now);
      List<Scoring> Get(int i);
      void UpdateAllData(string rpsdataJson);
  }
}
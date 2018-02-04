using System;
using System.Collections.Generic;
using System.Text;

namespace RPS.Domain.Snakes
{
    public interface ISnakeDataRepository
    {
        List<SnakeBites> GetGeographicalRegions();

        GeographicalCountries GetBarChartDataForRegion(string machineName);

        void AddAllData();
    }
}

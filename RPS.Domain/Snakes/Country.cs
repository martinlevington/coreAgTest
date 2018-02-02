using System;
using System.Collections.Generic;
using System.Text;

namespace RPS.Domain.Snakes
{
    public class Country
    {
        public string Name { get; set; }
        public double NumberOfCases { get; set; }

        public double NumberOfDeaths { get; set; }

        public double CurrentPopulation { get; set; }
    }
}

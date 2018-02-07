using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RPS.Domain.ProfileCompleteness
{

    public class Scoring
    {

        public int Id { get; set; }

        public int CompanyFK { get; set; }

        public DateTime RecordedOn { get; set; }

        public double Score { get; set; }

        public double Change { get; set; }

        public int RuleVersion { get; set; }

        public List<string> Explanations { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modifed { get; set; }


        public  DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

    }
}

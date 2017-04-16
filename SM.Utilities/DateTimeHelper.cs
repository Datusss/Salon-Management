using System;

namespace SM.Utilities
{
    public class DateTimeHelper
    {
        /// <summary>
        /// Get first date of the quarter
        /// </summary>
        /// <param name="datetime">Date time input</param>
        /// <returns>First date of the month.</returns>
        public static DateTime FirstDateOfQuarter(DateTime? datetime = null)
        {
            var date = datetime ?? DateTime.Now;
            var quarterNumber = (date.Month - 1) / 3 + 1;
            var firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);

            return firstDayOfQuarter;
        }
        /// <summary>
        /// Get last date of the quarter
        /// </summary>
        /// <param name="datetime">Date time input</param>
        /// <returns>First date of the month.</returns>
        public static DateTime LastDateOfQuarter(DateTime? datetime = null)
        {
            var date = datetime ?? DateTime.Now;
            var firstDayOfQuarter = FirstDateOfQuarter(date);
            var lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
            return lastDayOfQuarter;
        }

        /// <summary>
        /// Get first date of the week
        /// </summary>
        /// <param name="datetime">Date time input</param>
        /// <returns>First date of the week.</returns>
        public static DateTime FirstDateOfWeek(DateTime datetime)
        {

            var day = datetime.DayOfWeek == DayOfWeek.Sunday ? 6 : ((int)datetime.DayOfWeek) - 1;
            //   DayOfWeek.Friday
            return datetime.AddDays(-day);
        }

        public static bool Equal(DateTime source, DateTime target)
        {            
            return string.Format("{0} {1}", source.ToLongDateString(), source.ToLongTimeString()).Equals(string.Format("{0} {1}", target.ToLongDateString(), target.ToLongTimeString()));
        }
        /// <summary>
        /// Get first date of the month
        /// </summary>
        /// <param name="datetime">Date time input</param>
        /// <returns>First date of the month.</returns>
        public static DateTime FirstDateOfMonth(DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 01);
        }

        /// <summary>
        /// Get first date of year
        /// </summary>
        /// <param name="datetime">Date time input</param>
        /// <returns>First date of the year.</returns>
        public static DateTime FirstDateOfYear(DateTime datetime)
        {
            return new DateTime(datetime.Year, 01, 01);
        }

        /// <summary>
        /// This function will find the last day of the month for the DateTime passed into it.
        /// </summary>
        /// <param name="dt">A DateTime of the month/year for which you need to find the last day.</param>
        /// <returns>A DateTime of the last day of the month for the month/year entered.</returns>
        public static DateTime LastDayofMonth(DateTime dt)
        {
            //Select the first day of the month by using the DateTime class
            dt = FirstDateOfMonth(dt);
            //Add one month to our adjusted DateTime
            dt = dt.AddMonths(1);
            //Subtract one day from our adjusted DateTime
            dt = dt.AddDays(-1);
            //Return the DateTime, now set to the last day of the month
            return dt;
        }

        public static TimeRange GetTimeRange(string timeRangeLabel)
        {
            if (timeRangeLabel == null)
                return null; 
            var tr = new TimeRange(); 
            if (timeRangeLabel.IndexOf("year") >=0)
            {
                tr.StartDate = DateTimeHelper.FirstDateOfYear(DateTime.Now);
                return tr;
            }
            else if(timeRangeLabel.IndexOf("quarter")>=0)
            {
                tr.StartDate = DateTimeHelper.FirstDateOfQuarter(DateTime.Now);
                return tr;
            }
            else if (timeRangeLabel.IndexOf("month") >=0)
            {
                tr.StartDate = DateTimeHelper.FirstDateOfMonth(DateTime.Now);
                return tr; 
            }

            return null; 
        }

    }

    public class TimeRange
    {
        public DateTime? StartDate;
        public DateTime? EndDate; 
    }
}

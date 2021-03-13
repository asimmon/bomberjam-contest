using System;

namespace Bomberjam.Website.Utils
{
    public static class DateTimeExtensions
    {
        private const int Second = 1;
        private const int Minute = 60 * Second;
        private const int Hour = 60 * Minute;
        private const int Day = 24 * Hour;
        private const int Month = 30 * Day;

        public static string RelativeTo(this DateTime startDate, DateTime endDate)
        {
            var ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            return delta switch
            {
                < 1 * Minute => ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago",
                < 2 * Minute => "a minute ago",
                < 45 * Minute => ts.Minutes + " minutes ago",
                < 90 * Minute => "an hour ago",
                < 24 * Hour => ts.Hours + " hours ago",
                < 48 * Hour => "yesterday",
                < 30 * Day => ts.Days + " days ago",
                < 12 * Month => MonthsAgo(ts),
                _ => YearsAgo(ts),
            };
        }

        private static string YearsAgo(TimeSpan ts)
        {
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }

        private static string MonthsAgo(TimeSpan ts)
        {
            var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }
    }
}
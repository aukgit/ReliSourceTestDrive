using System;
using System.Configuration;

namespace ReliSource.Utility {
    public static class DateTimeExtensions {
        static string _timeZoneId = ConfigurationManager.AppSettings["TimeZoneId"] ?? "W. Europe Standard Time";

        public static DateTime ToLocalTime(this DateTime dt) {
            // dt.DateTimeKind should be Utc!
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dt, DateTimeKind.Utc), tzi);
        }

        public static DateTime ToUtcTime(this DateTime dt) {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(dt, tzi);
        }

        public static DateTime RoundDown(this DateTime dateTime, int minutes) {
            return new DateTime(dateTime.Year, dateTime.Month,
                 dateTime.Day, dateTime.Hour, (dateTime.Minute / minutes) * minutes, 0);
        }
    }
}

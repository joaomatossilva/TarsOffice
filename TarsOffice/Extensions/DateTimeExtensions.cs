using DateTimeExtensions;
using DateTimeExtensions.WorkingDays;
using System;

namespace TarsOffice.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly IWorkingDayCultureInfo cultureInfo = new WorkingDayCultureInfo("pt-PT");

        public static DateTime AddWorkingDays(this DateTime date, int days) => date.AddWorkingDays(days, cultureInfo);

        public static bool IsWorkingDay(this DateTime date) => date.IsWorkingDay(cultureInfo);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorForum.Utils
{
    class TimeStamp
    {
        public static long GetTimeNowInUnix()
        {
            var dateTime = DateTime.Now.ToLocalTime();
            var dateTimeOffset = new DateTimeOffset(dateTime);
            var unixDateTime =dateTimeOffset.ToUnixTimeSeconds();
            return unixDateTime;
        }
        public static DateTime GetDateFromUnix(long unixTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            DateTime dateTime = dateTimeOffset.UtcDateTime;
            return dateTime;
        }
        public static long GetTick()
        {
            var dateTime = DateTime.Now.Ticks;
            return dateTime;
        }
        public static long GetTimeFronUnix(DateTime time)
        {
            var dateTimeOffset = new DateTimeOffset(time);
            var unixDateTime =dateTimeOffset.ToUnixTimeSeconds();
            return unixDateTime;
        }
    }
}

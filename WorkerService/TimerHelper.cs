using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WorkerService
{
    static class TimerHelper
    {
        /// <summary>
        /// Создает таймер который выполняет переданный метод в указанное время, каждый день.
        /// </summary>
        /// <param name="hour">Час в который будет выполнен переданный метод.</param>
        /// <param name="min">Минута в которую будет выполнен переданный метод.</param>
        /// <param name="callback">Делегат, представляющий выполняемый метод.</param>
        /// <returns></returns>
        public static Timer SpecificTime(int hour, int min, TimerCallback callback)
        {
            TimeSpan interval = TimeSpan.FromHours(24);
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);

            if (now > firstRun)
                firstRun = firstRun.AddDays(1);

            TimeSpan timeToGo = firstRun - now;

            if (timeToGo <= TimeSpan.Zero)
                timeToGo = TimeSpan.Zero;

            Timer timer = new Timer(callback, null, timeToGo, interval);

            return timer;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService
{
    interface IWorkerService
    {
        Task CollectDailyStatistics();
        Task NotifySubscribers();
    }
}

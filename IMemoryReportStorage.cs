using System.Collections.Generic;

namespace StudentOrderService
{
    public interface IMemoryReportStorage
    {
        void Add(Report report);
        IEnumerable<Report> Get();
    }
}
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System;
using StudentOrderService;

namespace StudentOrderService
{
    public class ReportDataCollector : IHostedService
    {
        private const int DEFAULT_QUANTITY = 100;
        private readonly ISubscriber subscriber;
        private readonly IMemoryReportStorage memoryReportStorage;

        public ReportDataCollector(ISubscriber subscriber, IMemoryReportStorage memoryReportStorage)
        {
            this.subscriber = subscriber;
            this.memoryReportStorage = memoryReportStorage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(ProcessMessage);
            return Task.CompletedTask;
        }

        private bool ProcessMessage(string message, IDictionary<string, object> headers)
        {
            string connectionstring = ConfigurationManager.AppSettings["ConnectionStrings"];
            DbProviderFactory factory = DbProviderFactories.GetFactory("DataProvider");
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();
            //string messageJson = File.ReadAllText(@"Sample.json"); ;
            Course result = JsonConvert.DeserializeObject<Course>(message);
            List<Subject> listofSubjects = new List<Subject>(result.Subjects);
            SqlCommand command = conn.CreateCommand();
            String sAllQueries = null;
            String str = null;
            foreach (var item in listofSubjects)
            {
                Subject sub = new Subject();
                sub.guid = new Guid();
                sub.Id = item.Id;
                sub.Name = item.Name;
                sub.SemesterOffered = item.SemesterOffered;
                sub.IsScheduled = item.IsScheduled;
                sub.Core = item.Core;
                sub.CourseId = result.Id;
                sub.CourseName = result.Name;
                str = "INSERT INTO GUID, Name,CORE,IsScheduled,SemesterOffered,CourseId,CourseName VALUES (" + sub.guid + "," + sub.Id + "," + sub.Name + "," + sub.SemesterOffered + "," + sub.IsScheduled + "," + sub.Core + "," + sub.CourseId + "," + sub.CourseName + ";";
                sAllQueries = sAllQueries + ";" + str;
            }
            command.CommandText = sAllQueries;
            command.ExecuteNonQuery();

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

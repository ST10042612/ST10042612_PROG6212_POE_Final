using Azure;
using Azure.Data.Tables;

namespace CMCS_WebApplication.Models
{
    public class Claim : ITableEntity
    {
        public string PartitionKey { get; set; } 
        public string RowKey { get; set; }
        public int Lecturer_ID { get; set; }
        public int Claim_ID { get; set; }
        public decimal Hours_Worked { get; set; }
        public decimal Hourly_Rate { get; set; }
        public string Claim_Status { get; set; }
        public decimal Final_Pay { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
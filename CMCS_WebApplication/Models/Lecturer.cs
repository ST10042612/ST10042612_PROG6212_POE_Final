using Azure;
using Azure.Data.Tables;

namespace CMCS_WebApplication.Models
{
    public class Lecturer : ITableEntity
    {
        public string PartitionKey { get; set; } 
        public string RowKey { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
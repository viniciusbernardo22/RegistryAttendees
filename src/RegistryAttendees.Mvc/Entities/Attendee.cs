using Azure;
using Azure.Data.Tables;
using RegistryAttendees.Mvc.Models;

namespace RegistryAttendees.Mvc.Entities;

public class Attendee : ITableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get;  set; }
    public string Industry { get; set; }
    
    /* Azure TbEntity Props */
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
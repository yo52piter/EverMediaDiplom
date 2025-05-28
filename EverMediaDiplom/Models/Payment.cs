using System;
using System.Text.Json.Serialization;

namespace EverMediaDiplom.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } // "Pending", "Completed", "Failed", "Refunded"
        public string TransactionId { get; set; }

        // Foreign key
        public int PhotoSessionId { get; set; }

        // Navigation property
        [JsonIgnore]
        public PhotoSession PhotoSession { get; set; }
    }
}
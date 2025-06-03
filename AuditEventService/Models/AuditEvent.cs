using System;
using System.ComponentModel.DataAnnotations;

namespace AuditEventService.Models
{
    public class AuditEvent
    {
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required DateTimeOffset Timestamp { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(255)]
        public required string ServiceName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(255)]
        public required string EventType { get; set; }
        [Required]
        [StringLength(8000)]
        public required string Payload { get; set; }
    }
}

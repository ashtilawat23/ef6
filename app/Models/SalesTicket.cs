using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF6Demo.Models
{
    [Table("SalesTickets")]
    public class SalesTicket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        [StringLength(20)]
        public string TicketNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }  // New, InProgress, Completed, Cancelled

        [StringLength(500)]
        public string Notes { get; set; }

        [Required]
        [StringLength(50)]
        public string SalesRepresentative { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; }

        [NotMapped]
        public decimal NetAmount => TotalAmount - (DiscountAmount ?? 0) + (TaxAmount ?? 0);

        public SalesTicket()
        {
            CreatedDate = DateTime.UtcNow;
            Status = "New";
            IsPaid = false;
            TicketNumber = GenerateTicketNumber();
        }

        private string GenerateTicketNumber()
        {
            return $"ST-{DateTime.UtcNow:yyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }
    }
} 
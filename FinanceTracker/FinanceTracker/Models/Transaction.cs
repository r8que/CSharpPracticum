using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть положительной")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public int ExpenseItemId { get; set; }

        [ForeignKey(nameof(ExpenseItemId))]
        public ExpenseItem? ExpenseItem { get; set; }
    }
}
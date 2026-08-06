using FinanceTracker.DTO;

namespace FinanceTracker.DTO
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Comment { get; set; }
        public int ExpenseItemId { get; set; }
        public ExpenseItemDto? ExpenseItem { get; set; }
    }
}
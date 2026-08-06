namespace FinanceTracker.DTO
{
    public class ExpenseItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; }  // Только DTO, без обратных ссылок
    }
}
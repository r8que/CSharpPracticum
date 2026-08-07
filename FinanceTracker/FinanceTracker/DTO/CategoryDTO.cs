namespace FinanceTracker.DTO
{ 
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MonthlyBudget { get; set; }
        public bool IsActive { get; set; }
    }
}
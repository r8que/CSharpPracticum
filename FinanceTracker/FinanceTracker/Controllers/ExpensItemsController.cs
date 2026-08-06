using FinanceTracker.Data;
using FinanceTracker.DTO;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.ExpenseItems
                .Include(e => e.Category)
                .Select(e => new ExpenseItemDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    IsActive = e.IsActive,
                    CategoryId = e.CategoryId,
                    Category = e.Category != null ? new CategoryDto
                    {
                        Id = e.Category.Id,
                        Name = e.Category.Name,
                        MonthlyBudget = e.Category.MonthlyBudget,
                        IsActive = e.Category.IsActive
                    } : null
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.ExpenseItems
                .Include(e => e.Category)
                .Where(e => e.Id == id)
                .Select(e => new ExpenseItemDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    IsActive = e.IsActive,
                    CategoryId = e.CategoryId,
                    Category = e.Category != null ? new CategoryDto
                    {
                        Id = e.Category.Id,
                        Name = e.Category.Name,
                        MonthlyBudget = e.Category.MonthlyBudget,
                        IsActive = e.Category.IsActive
                    } : null
                })
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound("Статья расходов не найдена");

            return Ok(item);
        }

        [HttpGet("bycategory/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var items = await _context.ExpenseItems
                .Where(e => e.CategoryId == categoryId)
                .Include(e => e.Category)
                .Select(e => new ExpenseItemDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    IsActive = e.IsActive,
                    CategoryId = e.CategoryId,
                    Category = e.Category != null ? new CategoryDto
                    {
                        Id = e.Category.Id,
                        Name = e.Category.Name,
                        MonthlyBudget = e.Category.MonthlyBudget,
                        IsActive = e.Category.IsActive
                    } : null
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseItem item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var category = await _context.Categories.FindAsync(item.CategoryId);
                if (category == null)
                    return BadRequest(new { error = "Категория не найдена" });

                if (!category.IsActive)
                    return BadRequest(new { error = "Нельзя создать статью для неактивной категории" });

                _context.ExpenseItems.Add(item);
                await _context.SaveChangesAsync();

                await _context.Entry(item).Reference(e => e.Category).LoadAsync();

                var result = new ExpenseItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsActive = item.IsActive,
                    CategoryId = item.CategoryId,
                    Category = new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        MonthlyBudget = category.MonthlyBudget,
                        IsActive = category.IsActive
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT и DELETE остаются такими же, но возвращают DTO
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseItem expenseItem)
        {
            if (id != expenseItem.Id)
                return BadRequest("ID не совпадает");

            var existing = await _context.ExpenseItems.FindAsync(id);
            if (existing == null)
                return NotFound("Статья расходов не найдена");

            var hasTransaction = await _context.Transactions
                .AnyAsync(t => t.ExpenseItemId == id);

            if (existing.IsActive && !expenseItem.IsActive && hasTransaction)
                return BadRequest("Нельзя деактивировать статью с транзакциями");

            existing.Name = expenseItem.Name;
            existing.CategoryId = expenseItem.CategoryId;
            existing.IsActive = expenseItem.IsActive;

            await _context.SaveChangesAsync();

            await _context.Entry(existing).Reference(e => e.Category).LoadAsync();

            var result = new ExpenseItemDto
            {
                Id = existing.Id,
                Name = existing.Name,
                IsActive = existing.IsActive,
                CategoryId = existing.CategoryId,
                Category = existing.Category != null ? new CategoryDto
                {
                    Id = existing.Category.Id,
                    Name = existing.Category.Name,
                    MonthlyBudget = existing.Category.MonthlyBudget,
                    IsActive = existing.Category.IsActive
                } : null
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expenseItem = await _context.ExpenseItems.FindAsync(id);
            if (expenseItem == null)
                return NotFound("Предмет не найден");

            var hasTransactions = await _context.Transactions
                .AnyAsync(t => t.ExpenseItemId == id);

            if (hasTransactions)
                return BadRequest("Нельзя удалить статью с транзакциями");

            _context.ExpenseItems.Remove(expenseItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
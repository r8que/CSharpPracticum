using FinanceTracker.Data;
using FinanceTracker.DTO;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _context.Transactions
                .Include(t => t.ExpenseItem)
                    .ThenInclude(e => e!.Category)
                .OrderByDescending(t => t.Date)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    Comment = t.Comment,
                    ExpenseItemId = t.ExpenseItemId,
                    ExpenseItem = t.ExpenseItem != null ? new ExpenseItemDto
                    {
                        Id = t.ExpenseItem.Id,
                        Name = t.ExpenseItem.Name,
                        IsActive = t.ExpenseItem.IsActive,
                        CategoryId = t.ExpenseItem.CategoryId,
                        Category = t.ExpenseItem.Category != null ? new CategoryDto
                        {
                            Id = t.ExpenseItem.Category.Id,
                            Name = t.ExpenseItem.Category.Name,
                            MonthlyBudget = t.ExpenseItem.Category.MonthlyBudget,
                            IsActive = t.ExpenseItem.Category.IsActive
                        } : null
                    } : null
                })
                .ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.ExpenseItem)
                    .ThenInclude(e => e!.Category)
                .Where(t => t.Id == id)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    Comment = t.Comment,
                    ExpenseItemId = t.ExpenseItemId,
                    ExpenseItem = t.ExpenseItem != null ? new ExpenseItemDto
                    {
                        Id = t.ExpenseItem.Id,
                        Name = t.ExpenseItem.Name,
                        IsActive = t.ExpenseItem.IsActive,
                        CategoryId = t.ExpenseItem.CategoryId,
                        Category = t.ExpenseItem.Category != null ? new CategoryDto
                        {
                            Id = t.ExpenseItem.Category.Id,
                            Name = t.ExpenseItem.Category.Name,
                            MonthlyBudget = t.ExpenseItem.Category.MonthlyBudget,
                            IsActive = t.ExpenseItem.Category.IsActive
                        } : null
                    } : null
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
                return NotFound("Транзакция не найдена");

            return Ok(transaction);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var transactions = await _context.Transactions
                .Include(t => t.ExpenseItem)
                    .ThenInclude(e => e!.Category)
                .Where(t => t.Date >= startDate && t.Date < endDate)
                .OrderByDescending(t => t.Date)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    Comment = t.Comment,
                    ExpenseItemId = t.ExpenseItemId,
                    ExpenseItem = t.ExpenseItem != null ? new ExpenseItemDto
                    {
                        Id = t.ExpenseItem.Id,
                        Name = t.ExpenseItem.Name,
                        IsActive = t.ExpenseItem.IsActive,
                        CategoryId = t.ExpenseItem.CategoryId,
                        Category = t.ExpenseItem.Category != null ? new CategoryDto
                        {
                            Id = t.ExpenseItem.Category.Id,
                            Name = t.ExpenseItem.Category.Name,
                            MonthlyBudget = t.ExpenseItem.Category.MonthlyBudget,
                            IsActive = t.ExpenseItem.Category.IsActive
                        } : null
                    } : null
                })
                .ToListAsync();

            // Считаем сумму за день
            var totalAmount = transactions.Sum(t => t.Amount);

            // Определяем стикер
            string sticker = totalAmount < 500 ? "🟢 Зеленый (экономно)" :
                            totalAmount <= 2000 ? "🟡 Желтый (обычные траты)" :
                            "🔴 Красный (затратный день)";

            return Ok(new
            {
                date = date.ToString("yyyy-MM-dd"),
                totalAmount = totalAmount,
                sticker = sticker,
                transactions = transactions
            });
        }

        [HttpGet("month/{year}/{month}")]
        public async Task<IActionResult> GetByMonth(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var transactions = await _context.Transactions
                .Include(t => t.ExpenseItem)
                    .ThenInclude(e => e!.Category)
                .Where(t => t.Date >= startDate && t.Date < endDate)
                .OrderByDescending(t => t.Date)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Amount = t.Amount,
                    Comment = t.Comment,
                    ExpenseItemId = t.ExpenseItemId,
                    ExpenseItem = t.ExpenseItem != null ? new ExpenseItemDto
                    {
                        Id = t.ExpenseItem.Id,
                        Name = t.ExpenseItem.Name,
                        IsActive = t.ExpenseItem.IsActive,
                        CategoryId = t.ExpenseItem.CategoryId,
                        Category = t.ExpenseItem.Category != null ? new CategoryDto
                        {
                            Id = t.ExpenseItem.Category.Id,
                            Name = t.ExpenseItem.Category.Name,
                            MonthlyBudget = t.ExpenseItem.Category.MonthlyBudget,
                            IsActive = t.ExpenseItem.Category.IsActive
                        } : null
                    } : null
                })
                .ToListAsync();

            var totalAmount = transactions.Sum(t => t.Amount);

            return Ok(new
            {
                year = year,
                month = month,
                totalAmount = totalAmount,
                transactionCount = transactions.Count,
                transactions = transactions
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Проверка, что сумма положительная
                if (transaction.Amount <= 0)
                    return BadRequest(new { error = "Сумма должна быть положительной" });

                // Проверка, что статья существует
                var expenseItem = await _context.ExpenseItems
                    .Include(e => e.Category)
                    .FirstOrDefaultAsync(e => e.Id == transaction.ExpenseItemId);

                if (expenseItem == null)
                    return BadRequest(new { error = "Статья расходов не найдена" });

                // Нельзя выбрать неактивную статью
                if (!expenseItem.IsActive)
                    return BadRequest(new { error = "Нельзя выбрать неактивную статью расходов" });

                // Проверка ограничения 1 000 000 руб в день
                var startDate = transaction.Date.Date;
                var endDate = startDate.AddDays(1);

                var dailyTotal = await _context.Transactions
                    .Where(t => t.Date >= startDate && t.Date < endDate)
                    .SumAsync(t => t.Amount);

                if (dailyTotal + transaction.Amount > 1000000)
                    return BadRequest(new
                    {
                        error = $"Превышен дневной лимит в 1 000 000 руб. " +
                                $"Уже потрачено: {dailyTotal} руб. " +
                                $"Попытка добавить: {transaction.Amount} руб."
                    });

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                await _context.Entry(transaction).Reference(t => t.ExpenseItem).LoadAsync();
                if (transaction.ExpenseItem != null)
                {
                    await _context.Entry(transaction.ExpenseItem).Reference(e => e.Category).LoadAsync();
                }

                var result = new TransactionDto
                {
                    Id = transaction.Id,
                    Date = transaction.Date,
                    Amount = transaction.Amount,
                    Comment = transaction.Comment,
                    ExpenseItemId = transaction.ExpenseItemId,
                    ExpenseItem = transaction.ExpenseItem != null ? new ExpenseItemDto
                    {
                        Id = transaction.ExpenseItem.Id,
                        Name = transaction.ExpenseItem.Name,
                        IsActive = transaction.ExpenseItem.IsActive,
                        CategoryId = transaction.ExpenseItem.CategoryId,
                        Category = transaction.ExpenseItem.Category != null ? new CategoryDto
                        {
                            Id = transaction.ExpenseItem.Category.Id,
                            Name = transaction.ExpenseItem.Category.Name,
                            MonthlyBudget = transaction.ExpenseItem.Category.MonthlyBudget,
                            IsActive = transaction.ExpenseItem.Category.IsActive
                        } : null
                    } : null
                };

                return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Transaction transaction)
        {
            try
            {
                if (id != transaction.Id)
                    return BadRequest(new { error = "ID не совпадает" });

                var existing = await _context.Transactions
                    .Include(t => t.ExpenseItem)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (existing == null)
                    return NotFound("Транзакция не найдена");

                var expenseItem = await _context.ExpenseItems.FindAsync(transaction.ExpenseItemId);
                if (expenseItem == null)
                    return BadRequest(new { error = "Статья расходов не найдена" });

                if (!expenseItem.IsActive)
                    return BadRequest(new { error = "Нельзя использовать неактивную статью расходов" });

                if (transaction.Amount <= 0)
                    return BadRequest(new { error = "Сумма должна быть положительной" });

                if (existing.Date.Date != transaction.Date.Date || existing.Amount != transaction.Amount)
                {
                    var startDate = transaction.Date.Date;
                    var endDate = startDate.AddDays(1);

                    var dailyTotal = await _context.Transactions
                        .Where(t => t.Date >= startDate && t.Date < endDate && t.Id != id)
                        .SumAsync(t => t.Amount);

                    if (dailyTotal + transaction.Amount > 1000000)
                        return BadRequest(new { error = "Превышен дневной лимит в 1 000 000 руб" });
                }

                existing.Date = transaction.Date;
                existing.Amount = transaction.Amount;
                existing.Comment = transaction.Comment;
                existing.ExpenseItemId = transaction.ExpenseItemId;

                await _context.SaveChangesAsync();

                await _context.Entry(existing).Reference(t => t.ExpenseItem).LoadAsync();
                if (existing.ExpenseItem != null)
                {
                    await _context.Entry(existing.ExpenseItem).Reference(e => e.Category).LoadAsync();
                }

                var result = new TransactionDto
                {
                    Id = existing.Id,
                    Date = existing.Date,
                    Amount = existing.Amount,
                    Comment = existing.Comment,
                    ExpenseItemId = existing.ExpenseItemId,
                    ExpenseItem = existing.ExpenseItem != null ? new ExpenseItemDto
                    {
                        Id = existing.ExpenseItem.Id,
                        Name = existing.ExpenseItem.Name,
                        IsActive = existing.ExpenseItem.IsActive,
                        CategoryId = existing.ExpenseItem.CategoryId,
                        Category = existing.ExpenseItem.Category != null ? new CategoryDto
                        {
                            Id = existing.ExpenseItem.Category.Id,
                            Name = existing.ExpenseItem.Category.Name,
                            MonthlyBudget = existing.ExpenseItem.Category.MonthlyBudget,
                            IsActive = existing.ExpenseItem.Category.IsActive
                        } : null
                    } : null
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound("Транзакция не найдена");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
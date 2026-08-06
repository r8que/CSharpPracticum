using FinanceTracker.DTO;
using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    MonthlyBudget = c.MonthlyBudget,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    MonthlyBudget = c.MonthlyBudget,
                    IsActive = c.IsActive
                })
                .FirstOrDefaultAsync();

            if (category == null)
                return NotFound("Категория не найдена");

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                MonthlyBudget = category.MonthlyBudget,
                IsActive = category.IsActive
            };

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest("ID не совпадает");

            var existing = await _context.Categories.FindAsync(id);
            if (existing == null)
                return NotFound("Категория не найдена");

            existing.Name = category.Name;
            existing.MonthlyBudget = category.MonthlyBudget;
            existing.IsActive = category.IsActive;

            await _context.SaveChangesAsync();

            var result = new CategoryDto
            {
                Id = existing.Id,
                Name = existing.Name,
                MonthlyBudget = existing.MonthlyBudget,
                IsActive = existing.IsActive
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Категория не найдена");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
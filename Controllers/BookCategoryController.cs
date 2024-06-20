using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryBackend.Models;
using LibraryBackend.Data;

namespace LibraryBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookCategoryController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public BookCategoryController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookCategory>>> Get()
		{
			return await _context.BookCategories.ToListAsync();
		}

		[HttpPost]
		public async Task<ActionResult<BookCategory>> Post(BookCategory category)
		{
			category.CategoryId = Guid.NewGuid(); // Generowanie UUID
			_context.BookCategories.Add(category);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { id = category.CategoryId }, category);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(Guid id, BookCategory category)
		{
			if (id != category.CategoryId)
			{
				return BadRequest();
			}

			_context.Entry(category).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!BookCategoryExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var category = await _context.BookCategories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			_context.BookCategories.Remove(category);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool BookCategoryExists(Guid id)
		{
			return _context.BookCategories.Any(e => e.CategoryId == id);
		}
	}
}

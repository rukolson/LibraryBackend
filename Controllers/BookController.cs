using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryBackend.Data;
using LibraryBackend.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace LibraryBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _env;

		public BookController(ApplicationDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
		{
			var books = await _context.Books
				.Include(b => b.BookCategory)
				.Select(b => new {
					b.BookId,
					b.BookName,
					CategoryName = b.BookCategory.CategoryName, // Ensure CategoryName is included
					b.DateOfAdding,
					b.CoverPicture
				}).ToListAsync();

			return Ok(books);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutBook(int id, Book book)
		{
			if (id != book.BookId)
			{
				return BadRequest();
			}

			_context.Entry(book).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!BookExists(id))
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
		public async Task<IActionResult> DeleteBook(int id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
			{
				return NotFound();
			}

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[Route("SaveFile")]
		[HttpPost]
		public JsonResult SaveFile()
		{
			try
			{
				var httpRequest = Request.Form;
				var postedFile = httpRequest.Files[0];
				string filename = postedFile.FileName;
				var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

				using (var stream = new FileStream(physicalPath, FileMode.Create))
				{
					postedFile.CopyTo(stream);
				}

				return new JsonResult(filename);
			}
			catch (Exception)
			{
				return new JsonResult("anonymous.png");
			}
		}

		private bool BookExists(int id)
		{
			return _context.Books.Any(e => e.BookId == id);
		}
	}
}
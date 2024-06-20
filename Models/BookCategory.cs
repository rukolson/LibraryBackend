using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryBackend.Models
{
	public class BookCategory
	{
		[Key]
		public Guid CategoryId { get; set; } // Zmiana typu na Guid

		[Required]
		public string CategoryName { get; set; }

		public ICollection<Book> Books { get; set; } = new List<Book>(); // Navigation property
	}
}
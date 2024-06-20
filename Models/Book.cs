using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryBackend.Models
{
	public class Book
	{
		[Key]
		public int BookId { get; set; }

		[Required]
		public string BookName { get; set; }

		[Required]
		public DateTime DateOfAdding { get; set; }

		[Required]
		public string CoverPicture { get; set; }

		[Required]
		public Guid CategoryId { get; set; }  // Foreign key

		[ForeignKey("CategoryId")]
		public BookCategory BookCategory { get; set; }  // Navigation property
	}
}

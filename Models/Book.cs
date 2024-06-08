using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBackend.Models
{
	public class Book
	{
		public int BookId { get; set; }
		public string BookName { get; set; }
		public string Category { get; set; }

		public string DateOfAdding { get; set; }

		public string CoverPicture { get; set; }
	}
}
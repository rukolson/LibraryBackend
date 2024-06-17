using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBackend.Models
{
	public class BookCategory
	{
		[Key]
		public int CategoryId { get; set; }

		[Required]
		public string CategoryName { get; set; }


	}
}
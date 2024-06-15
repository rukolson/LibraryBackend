using Microsoft.EntityFrameworkCore;
using LibraryBackend.Models;

namespace LibraryBackend.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<BookCategory> BookCategories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Define the relationship between Book and BookCategory
			modelBuilder.Entity<Book>()
				.HasOne(b => b.BookCategory)
				.WithMany()
				.HasForeignKey(b => b.CategoryName)
				.HasPrincipalKey(c => c.CategoryName);
		}
	}
}

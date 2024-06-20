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
				.WithMany(c => c.Books)
				.HasForeignKey(b => b.CategoryId)
				.OnDelete(DeleteBehavior.Restrict); // Optional: Define delete behavior
		}
	}
}

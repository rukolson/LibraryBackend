using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryBackend.Migrations
{
    /// <inheritdoc />
    public partial class _2ndMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookCategories_CategoryId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_CategoryId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Books",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "BookCategories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BookCategories_CategoryName",
                table: "BookCategories",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryName",
                table: "Books",
                column: "CategoryName");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookCategories_CategoryName",
                table: "Books",
                column: "CategoryName",
                principalTable: "BookCategories",
                principalColumn: "CategoryName",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookCategories_CategoryName",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_CategoryName",
                table: "Books");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_BookCategories_CategoryName",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "BookCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookCategories_CategoryId",
                table: "Books",
                column: "CategoryId",
                principalTable: "BookCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

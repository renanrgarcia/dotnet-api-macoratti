using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulateCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categories (Name, ImageUrl) VALUES ('Beverages', 'beverages.jpg')");
            migrationBuilder.Sql("INSERT INTO Categories (Name, ImageUrl) VALUES ('Snacks', 'snacks.jpg')");
            migrationBuilder.Sql("INSERT INTO Categories (Name, ImageUrl) VALUES ('Desserts', 'desserts.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Adjust to english
            migrationBuilder.Sql("INSERT INTO Products (Name, Description, Price, ImageUrl, Stock, RegisterDate, CategoryId)" +
                " VALUES('Coca Cola', 'Coca Cola 350 ml', 5.45, 'cocacola.jpg', 50, now(), 1) ");
            migrationBuilder.Sql("INSERT INTO Products (Name, Description, Price, ImageUrl, Stock, RegisterDate, CategoryId)" +
                " VALUES('Tuna sandwich', 'Tuna sandwich with maionese', 8.50, 'tunasandwich.jpg', 10, now(), 2) ");
            migrationBuilder.Sql("INSERT INTO Products (Name, Description, Price, ImageUrl, Stock, RegisterDate, CategoryId)" +
                " VALUES('Pudin', 'Milk pudin', 5.45, 'pudin.jpg', 20, now(), 3) ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Products");
        }
    }
}

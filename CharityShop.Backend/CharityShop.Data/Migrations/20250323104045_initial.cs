using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CharityShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TotalQuantity = table.Column<int>(type: "integer", nullable: false),
                    BookedQuantity = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BookedQuantity", "Name", "Price", "TotalQuantity", "Type" },
                values: new object[,]
                {
                    { 1, 0, "Brownie", 0.65000000000000002, 48, 0 },
                    { 2, 0, "Muffin", 1.0, 36, 0 },
                    { 3, 0, "Cake pop", 1.3500000000000001, 24, 0 },
                    { 4, 0, "Apple tart", 1.5, 60, 0 },
                    { 5, 0, "Water", 1.5, 30, 0 },
                    { 6, 0, "Shirt", 2.0, 0, 1 },
                    { 7, 0, "Pants", 3.0, 0, 1 },
                    { 8, 0, "Jacket", 4.0, 0, 1 },
                    { 9, 0, "Toy", 1.0, 0, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}

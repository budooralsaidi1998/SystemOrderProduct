using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemProductOrder.Migrations
{
    /// <inheritdoc />
    public partial class updateAddProductnames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductNames",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductNames",
                table: "orders");
        }
    }
}

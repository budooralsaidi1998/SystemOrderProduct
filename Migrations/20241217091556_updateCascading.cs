using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemProductOrder.Migrations
{
    /// <inheritdoc />
    public partial class updateCascading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_products_ProductId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_users_UserId",
                table: "reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_products_ProductId",
                table: "reviews",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Pid",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_users_UserId",
                table: "reviews",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_products_ProductId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_users_UserId",
                table: "reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_products_ProductId",
                table: "reviews",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Pid");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_users_UserId",
                table: "reviews",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Uid");
        }
    }
}

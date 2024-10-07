using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CustomerId",
                table: "OrderItems",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Customers_CustomerId",
                table: "OrderItems",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Customers_CustomerId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_CustomerId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OrderItems");
        }
    }
}

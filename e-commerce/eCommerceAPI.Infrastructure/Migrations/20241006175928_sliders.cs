using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sliders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_CompanyInfo_CompanyInfoId",
                table: "Sliders");

            migrationBuilder.DropIndex(
                name: "IX_Sliders_CompanyInfoId",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "CompanyInfoId",
                table: "Sliders");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyInfoId",
                table: "Sliders",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "CompanyInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Sliders_CompanyInfoId",
                table: "Sliders",
                column: "CompanyInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_CompanyInfo_CompanyInfoId",
                table: "Sliders",
                column: "CompanyInfoId",
                principalTable: "CompanyInfo",
                principalColumn: "Id");
        }
    }
}

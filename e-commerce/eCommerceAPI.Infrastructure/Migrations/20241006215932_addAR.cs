using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceAPI.Infrastructure.Migrations
{ 
    /// <inheritdoc />
    public partial class addAR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Policies",
                newName: "TitleEN");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Policies",
                newName: "TitleAR");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Articles",
                newName: "TitleEN");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Articles",
                newName: "TitleAR");

            migrationBuilder.AddColumn<string>(
                name: "FilePathAR",
                table: "Policies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePathEN",
                table: "Policies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentAR",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentEN",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePathAR",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "FilePathEN",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "ContentAR",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ContentEN",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "TitleEN",
                table: "Policies",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleAR",
                table: "Policies",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "TitleEN",
                table: "Articles",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleAR",
                table: "Articles",
                newName: "Content");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandmadeShop.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductImages",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "ApplicationUser",
                newName: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ProductImages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "ApplicationUser",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoUrlToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerDay",
                table: "Cars",
                newName: "PricePerMonth");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "PricePerMonth",
                table: "Cars",
                newName: "PricePerDay");
        }
    }
}

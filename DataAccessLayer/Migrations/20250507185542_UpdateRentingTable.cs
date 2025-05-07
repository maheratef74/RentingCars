using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRentingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalRecords_Customers_CustomerId",
                table: "RentalRecords");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RentalRecords");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "RentalRecords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRecords_Customers_CustomerId",
                table: "RentalRecords",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalRecords_Customers_CustomerId",
                table: "RentalRecords");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "RentalRecords",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RentalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalRecords_Customers_CustomerId",
                table: "RentalRecords",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "UserId");
        }
    }
}

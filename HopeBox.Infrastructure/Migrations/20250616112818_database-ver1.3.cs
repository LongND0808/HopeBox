using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopeBox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class databasever13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ReliefItems");

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraFee",
                table: "ReliefPackages",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ReliefPackages",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraFee",
                table: "ReliefPackages");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ReliefPackages");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ReliefItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

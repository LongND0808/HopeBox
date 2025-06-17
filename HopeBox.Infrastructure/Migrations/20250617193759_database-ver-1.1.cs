using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopeBox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class databasever11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentQuantity",
                table: "ReliefPackages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraFee",
                table: "ReliefPackages",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ReliefPackages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetQuantity",
                table: "ReliefPackages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ReliefPackages",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DonationAmount",
                table: "Donations",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Donations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Donations",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentQuantity",
                table: "ReliefPackages");

            migrationBuilder.DropColumn(
                name: "ExtraFee",
                table: "ReliefPackages");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ReliefPackages");

            migrationBuilder.DropColumn(
                name: "TargetQuantity",
                table: "ReliefPackages");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ReliefPackages");

            migrationBuilder.DropColumn(
                name: "DonationAmount",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Donations");
        }
    }
}

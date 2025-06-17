using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopeBox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dropdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM DonationReliefPackages");
            migrationBuilder.Sql("DELETE FROM ReliefPackageItems");
            migrationBuilder.Sql("DELETE FROM Donations");
            migrationBuilder.Sql("DELETE FROM ReliefItems");
            migrationBuilder.Sql("DELETE FROM ReliefPackages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

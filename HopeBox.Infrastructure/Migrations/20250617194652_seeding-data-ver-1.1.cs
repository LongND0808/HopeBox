using Microsoft.EntityFrameworkCore.Migrations;
using static HopeBox.Common.Enum.Enumerate;

#nullable disable

namespace HopeBox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingdataver11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "ReliefItems",
               columns: new[] { "Id", "ItemName", "Unit", "UnitPrice" },
               values: new object[,]
               {
                    { Guid.NewGuid(), "Gạo", (int)Unit.kg, 15000m },
                    { Guid.NewGuid(), "Mì gói", (int)Unit.Pack, 25000m },
                    { Guid.NewGuid(), "Nước suối", (int)Unit.Bottle, 10000m },
                    { Guid.NewGuid(), "Dầu ăn", (int)Unit.Bottle, 35000m },
                    { Guid.NewGuid(), "Nước tương", (int)Unit.Bottle, 20000m },
                    { Guid.NewGuid(), "Đường", (int)Unit.kg, 18000m },
                    { Guid.NewGuid(), "Muối", (int)Unit.kg, 7000m },
                    { Guid.NewGuid(), "Bánh quy", (int)Unit.Pack, 22000m },
                    { Guid.NewGuid(), "Sữa hộp", (int)Unit.Carton, 120000m },
                    { Guid.NewGuid(), "Bột ngọt", (int)Unit.Bag, 15000m },
                    { Guid.NewGuid(), "Nước rửa tay", (int)Unit.Bottle, 25000m },
                    { Guid.NewGuid(), "Khẩu trang", (int)Unit.Box, 40000m },
                    { Guid.NewGuid(), "Xà phòng", (int)Unit.Box, 30000m },
                    { Guid.NewGuid(), "Trứng", (int)Unit.Box, 35000m },
                    { Guid.NewGuid(), "Thịt hộp", (int)Unit.Box, 50000m },
                    { Guid.NewGuid(), "Cháo ăn liền", (int)Unit.Pack, 17000m },
                    { Guid.NewGuid(), "Tã giấy", (int)Unit.Bag, 90000m },
                    { Guid.NewGuid(), "Kem đánh răng", (int)Unit.Box, 15000m },
                    { Guid.NewGuid(), "Bàn chải", (int)Unit.Pack, 12000m },
                    { Guid.NewGuid(), "Nước lau sàn", (int)Unit.Bottle, 35000m },
                    { Guid.NewGuid(), "Giấy vệ sinh", (int)Unit.Pack, 20000m },
                    { Guid.NewGuid(), "Khăn ướt", (int)Unit.Pack, 18000m },
                    { Guid.NewGuid(), "Ngũ cốc", (int)Unit.Box, 45000m },
                    { Guid.NewGuid(), "Bánh mì", (int)Unit.Bag, 25000m },
                    { Guid.NewGuid(), "Rau củ sấy", (int)Unit.Bag, 30000m },
                    { Guid.NewGuid(), "Nước trái cây", (int)Unit.Bottle, 28000m },
                    { Guid.NewGuid(), "Hạt nêm", (int)Unit.Bag, 22000m },
                    { Guid.NewGuid(), "Chanh", (int)Unit.kg, 12000m },
                    { Guid.NewGuid(), "Ớt", (int)Unit.kg, 10000m },
                    { Guid.NewGuid(), "Mì Ý", (int)Unit.Pack, 27000m },
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

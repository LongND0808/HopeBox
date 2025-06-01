using Microsoft.EntityFrameworkCore.Migrations;
using static HopeBox.Common.Enum.Enumerate;

#nullable disable

namespace HopeBox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEventsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[]
                {
                    "Id", "Title", "Description", "Detail", "BannerImage", "StartDate", "EndDate", "Location",
                    "TargetAmount", "CurrentAmount", "Status", "CreatedBy", "OrganizationId"
                },
                values: new object[,]
                {
                    {
                        Guid.NewGuid(),
                        "Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội",
                        "Chương trình trao quà, phát sách vở và tặng suất ăn cho trẻ em tại các khu vực ngoại thành Hà Nội.",
                        "Sự kiện thiện nguyện tổ chức vào cuối tuần với sự tham gia của hơn 200 tình nguyện viên. Chúng tôi sẽ phát quà cho hơn 500 trẻ em có hoàn cảnh khó khăn, đồng thời tổ chức các hoạt động giao lưu văn nghệ.",
                        "https://cdn.hopebox.org/events/hanoi-charity-2025.jpg",
                        new DateTime(2025, 7, 12, 8, 0, 0),
                        new DateTime(2025, 7, 12, 16, 0, 0),
                        "Trường Tiểu học A, Hoài Đức, Hà Nội",
                        50000000m,
                        0m,
                        (int)EventStatus.Ongoing,
                        Guid.Parse("720CEF3C-4892-4FDB-B4E6-B6CAA8200BB6"),
                        Guid.Parse("5A8FA9F4-5A80-4D23-BE32-F439E8EE708E")
                    },
                    {
                        Guid.NewGuid(),
                        "Ngày hội Hiến Máu Nhân Đạo Đà Nẵng",
                        "Chương trình hiến máu cứu người tại Nhà Văn hoá Thiếu nhi Đà Nẵng.",
                        "Thời gian: 8h-17h ngày 20/7/2025, dự kiến thu hút 800 người tham dự.",
                        "https://cdn.hopebox.org/events/danang-blood-2025.jpg",
                        new DateTime(2025, 7, 20, 8, 0, 0),
                        new DateTime(2025, 7, 20, 17, 0, 0),
                        "Nhà Văn hóa Thiếu nhi Đà Nẵng, 2 Phan Đăng Lưu, Hải Châu",
                        0m,
                        0m,
                        (int)EventStatus.Ongoing,
                        Guid.Parse("720CEF3C-4892-4FDB-B4E6-B6CAA8200BB6"),
                        Guid.Parse("5A8FA9F4-5A80-4D23-BE32-F439E8EE708E")
                    }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

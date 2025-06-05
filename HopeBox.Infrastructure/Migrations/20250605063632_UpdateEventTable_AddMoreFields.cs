using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopeBox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventTable_AddMoreFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.Sql(@"
                UPDATE Events
                SET BannerImage = N'https://media-cdn-v2.laodong.vn/storage/newsportal/2022/12/11/1126243/484Fa6554cc4959accd5.jpg',
                    Description = N'Ngày hội Hiến Máu Nhân Đạo Đà Nẵng - Một hành trình ý nghĩa lan tỏa yêu thương, kêu gọi toàn thể cộng đồng Đà Nẵng cùng chung tay giúp đỡ những bệnh nhân cần máu. Sự kiện diễn ra tại Nhà Văn hóa Thiếu nhi Đà Nẵng với không gian rộng rãi, trang thiết bị hiện đại, tạo điều kiện cho người tham gia có trải nghiệm tốt nhất.',
                    Detail = N'Thời gian diễn ra từ 8h sáng đến 17h chiều ngày 20/7/2025. Mỗi người tham gia hiến máu sẽ được nhận quà tặng, giấy chứng nhận và phần ăn nhẹ. Ban tổ chức bố trí đội ngũ y tế túc trực, hỗ trợ tư vấn sức khỏe, theo dõi sát sao toàn bộ quá trình hiến máu. Đây là sự kiện thường niên, mong muốn kết nối các tình nguyện viên, nhóm máu hiếm và các tổ chức xã hội tại Đà Nẵng.',
                    Location = N'Nhà Văn hóa Thiếu nhi Đà Nẵng, 2 Phan Đăng Lưu, Hải Châu, Đà Nẵng.'
                WHERE Title = N'Ngày hội Hiến Máu Nhân Đạo Đà Nẵng'
            ");

            migrationBuilder.Sql(@"
                UPDATE Events
                SET BannerImage = N'https://images.baodantoc.vn/uploads/2021/Th%C3%A1ng_12/Ng%C3%A0y%203/TRUNG/T%E1%BA%B7ng%20qu%C3%A0/A1%20-%20OK.jpg',
                    Description = N'Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội là chương trình thiện nguyện đầy ý nghĩa, nhằm mang đến niềm vui và sự động viên thiết thực cho các em nhỏ có hoàn cảnh khó khăn tại các vùng ngoại thành Hà Nội. Sự kiện không chỉ là dịp các em nhận được sách vở, quà tặng và bữa ăn dinh dưỡng mà còn là nơi các em giao lưu, tham gia các hoạt động văn nghệ, trò chơi tập thể cùng hơn 200 tình nguyện viên và các nhà hảo tâm.',
                    Detail = N'Sự kiện “Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội” sẽ diễn ra vào ngày 12/7/2025 tại Trường Tiểu học A, Hoài Đức, Hà Nội, dự kiến chào đón hơn 500 em nhỏ thuộc các hộ gia đình khó khăn trên địa bàn. Tại đây, mỗi em sẽ được nhận một phần quà bao gồm sách vở, đồ dùng học tập, sữa, bánh kẹo và một suất ăn trưa dinh dưỡng. Ngoài hoạt động trao quà, chương trình còn tổ chức các tiết mục văn nghệ do chính các em và tình nguyện viên biểu diễn, các trò chơi vận động, đố vui nhận thưởng, giúp các em giao lưu, tự tin hơn và có thêm nhiều kỷ niệm đẹp. Ban tổ chức bố trí đội ngũ y tế hỗ trợ, đảm bảo an toàn và sức khỏe cho tất cả người tham dự. Sự kiện cũng tạo cơ hội cho cộng đồng cùng chung tay chia sẻ, lan tỏa yêu thương và tiếp thêm động lực để các em nhỏ vượt qua khó khăn, vươn lên trong học tập cũng như cuộc sống.',
                    Location = N'Trường Tiểu học A, Thị trấn Trạm Trôi, Huyện Hoài Đức, Hà Nội'
                WHERE Title = N'Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội'
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);
        }
    }
}

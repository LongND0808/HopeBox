using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using static HopeBox.Common.Enum.Enumerate;

#nullable disable

namespace HopeBox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var userAdminId = Guid.NewGuid();
            var userCustomerId = Guid.NewGuid();
            var roleAdminId = Guid.NewGuid();
            var roleCustomerId = Guid.NewGuid();
            var confirmEmailAdminId = Guid.NewGuid();
            var confirmEmailCustomerId = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                   "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
                   "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed",
                   "FullName", "DateOfBirth", "Gender", "AvatarUrl", "Point", "UserStatus",
                   "AccessFailedCount", "LockoutEnabled", "LockoutEnd", "TwoFactorEnabled"
                },
                values: new object[,]
                {
                   {
                       userAdminId, "Nguyễn Đăng Long", "NGUYEN DANG LONG", "admin@hopebox.org", "ADMIN@HOPEBOX.ORG", true,
                       new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Admin@123"),
                       Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "08684532345", true,
                       "Nguyễn Đăng Long", new DateTime(1990,1,1), 1, "images/user/admin.jpg", 100, 1,
                       0, false, null, false
                   },
                   {
                       userCustomerId, "Nguyễn Hoàng Thắng", "NGUYEN HOANG THANG", "customer@hopebox.org", "CUSTOMER@HOPEBOX.ORG", true,
                       new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Customer@123"),
                       Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "08684531111", true,
                       "Nguyễn Hoàng Thắng", new DateTime(1995, 5, 15), 1, null, 50, 1,
                       0, false, null, false
                   }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
               { roleAdminId, "Admin", "ADMIN", Guid.NewGuid().ToString() },
               { roleCustomerId, "Customer", "CUSTOMER", Guid.NewGuid().ToString() }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,] { { userAdminId, roleAdminId }, { userCustomerId, roleCustomerId } });

            migrationBuilder.InsertData(
                table: "ConfirmEmails",
                columns: new[] { "Id", "UserId", "RequestedAt", "ExpiresAt", "ConfirmCode", "IsConfirmed" },
                values: new object[,]
                {
               { confirmEmailAdminId, userAdminId, DateTime.Now, DateTime.Now.AddMinutes(5), "12345678", true },
               { confirmEmailCustomerId, userCustomerId, DateTime.Now, DateTime.Now.AddMinutes(5), "87654321", false }
                });

            var orgId = Guid.NewGuid();
            var userId = userAdminId;
            var causeWaterId = Guid.NewGuid();
            var causeFoodId = Guid.NewGuid();
            var causeMedicineId = Guid.NewGuid();
            var causeEducationId = Guid.NewGuid();
            var causeShelterId = Guid.NewGuid();
            var causeClothingId = Guid.NewGuid();

            var mediaIds = new[]
            {
           Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
           Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
       };

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[]
                {
               "Id", "Name", "Email", "PhoneNumber", "Address", "City", "Description", "Website", "Verified",
               "CreatedAt", "UserId"
                },
                values: new object[]
                {
               orgId, "Global Hope Org", "info@globalhope.org", "0988888888",
               "456 Charity Lane", "HCMC", "Supporting humanitarian efforts",
               "https://globalhope.org", true, DateTime.UtcNow, userId
                });

            migrationBuilder.InsertData(
                table: "Causes",
                columns: new[]
                {
               "Id", "Title", "Description", "Detail", "HeroImage", "Challenge", "ChallengeImage",
               "Summary", "SummaryImage", "Type", "StartDate", "EndDate", "TargetAmount",
               "CurrentAmount", "Status", "CreatedBy", "OrganizationId"
                },
                values: new object[,]
                {
               {
                   causeWaterId, "Nước Sạch Cho Em", "Mang nước sạch đến các bản làng vùng cao",
                   "Xây dựng giếng khoan và hệ thống lọc nước giúp trẻ em có nguồn nước an toàn để sinh hoạt và học tập.",
                   "/images/causes/hero-water.jpg", "Nguồn nước ô nhiễm khiến trẻ em mắc bệnh tiêu hóa và da liễu",
                   "/images/causes/challenge-water.jpg", "Nước sạch là nền tảng cho sức khỏe và sự phát triển",
                   "/images/causes/summary-water.jpg",
                   (int)CauseType.Water, new DateTime(2025,6,1), new DateTime(2025,12,1), 10000m, 0m,
                   (int)CauseStatus.Pending, userId, orgId
               },
               {
                   causeFoodId, "Bữa Ăn Đầy Đủ", "Cung cấp thực phẩm cho trẻ em suy dinh dưỡng",
                   "Hỗ trợ các gia đình vùng cao bằng các phần quà thực phẩm thiết yếu hàng tháng cho trẻ nhỏ.",
                   "/images/causes/hero-food.jpg", "Thiếu ăn kéo dài khiến trẻ còi cọc và học kém",
                   "/images/causes/challenge-food.jpg", "Dinh dưỡng đầy đủ giúp trẻ lớn khôn khỏe mạnh",
                   "/images/causes/summary-food.jpg",
                   (int)CauseType.Food, new DateTime(2025,7,1), new DateTime(2025,11,1), 8000m, 0m,
                   (int)CauseStatus.Pending, userId, orgId
               },
               {
                   causeMedicineId, "Tủ Thuốc Nhân Ái", "Cung cấp thuốc cơ bản cho trẻ em vùng cao",
                   "Phối hợp với trạm y tế để cung cấp thuốc hạ sốt, tiêu hóa, bông băng y tế cho các điểm trường và hộ gia đình có trẻ nhỏ.",
                   "/images/causes/hero-medicine.jpg", "Trẻ em không được điều trị kịp thời vì thiếu thuốc",
                   "/images/causes/challenge-medicine.jpg", "Thuốc cơ bản có thể cứu sống trẻ em",
                   "/images/causes/summary-medicine.jpg",
                   (int)CauseType.Medicine, new DateTime(2025,8,1), new DateTime(2025,12,31), 12000m, 0m,
                   (int)CauseStatus.Pending, userId, orgId
               },
               {
                   causeEducationId, "Cặp Sách Đến Trường", "Tặng dụng cụ học tập và học phí",
                   "Tài trợ cặp sách, bút vở và đồng phục cho học sinh tiểu học và trung học ở vùng sâu vùng xa.",
                   "/images/causes/hero-edu.jpg", "Nhiều em phải nghỉ học vì không đủ điều kiện vật chất",
                   "/images/causes/challenge-edu.jpg", "Giáo dục là con đường thoát nghèo bền vững",
                   "/images/causes/summary-edu.jpg",
                   (int)CauseType.Education, new DateTime(2025,6,15), new DateTime(2025,12,15), 15000m, 0m,
                   (int)CauseStatus.Pending, userId, orgId
               },
               {
                   causeShelterId, "Mái Ấm Vùng Cao", "Sửa chữa và xây mới nhà cho trẻ em khó khăn",
                   "Hỗ trợ xây dựng nhà ở an toàn sau thiên tai và cho những gia đình nghèo có trẻ em.",
                   "/images/causes/hero-shelter.jpg", "Trẻ em sống trong nhà tạm bợ không đảm bảo an toàn",
                   "/images/causes/challenge-shelter.jpg", "Một mái ấm vững chắc mang lại hy vọng và tương lai",
                   "/images/causes/summary-shelter.jpg",
                   (int)CauseType.Shelter, new DateTime(2025,9,1), new DateTime(2026,1,1), 20000m, 0m,
                   (int)CauseStatus.Pending, userId, orgId
               },
               {
                   causeClothingId, "Áo Ấm Mùa Đông", "Gửi quần áo ấm cho trẻ em vùng núi",
                   "Tổ chức quyên góp và trao tặng áo khoác, khăn len, tất và giày dép cho trẻ em vào mùa đông lạnh giá.",
                   "/images/causes/hero-clothing.jpg", "Nhiều em chịu rét trong giá lạnh vì thiếu quần áo",
                   "/images/causes/challenge-clothing.jpg", "Một chiếc áo ấm cũng có thể cứu một sinh mạng",
                   "/images/causes/summary-clothing.jpg",
                   (int)CauseType.Clothing, new DateTime(2025,10,1), new DateTime(2025,12,31), 6000m, 0m,
                   (int)CauseStatus.Pending, userId, orgId
               }
                });

            migrationBuilder.InsertData(
                table: "Medias",
                columns: new[] { "Id", "Url", "Type", "CauseId" },
                values: new object[,]
                {
               { mediaIds[0], "https://cdn.hope.org/media/water.jpg", (int)MediaType.Image, causeWaterId },
               { mediaIds[1], "https://cdn.hope.org/media/food.jpg", (int)MediaType.Image, causeFoodId },
               { mediaIds[2], "https://cdn.hope.org/media/medicine.jpg", (int)MediaType.Image, causeMedicineId },
               { mediaIds[3], "https://cdn.hope.org/media/education.jpg", (int)MediaType.Image, causeEducationId },
               { mediaIds[4], "https://cdn.hope.org/media/shelter.jpg", (int)MediaType.Image, causeShelterId },
               { mediaIds[5], "https://cdn.hope.org/media/clothing.jpg", (int)MediaType.Image, causeClothingId }
                });

            var blogId1 = Guid.NewGuid();
            var blogId2 = Guid.NewGuid();
            var blogId3 = Guid.NewGuid();
            var blogId4 = Guid.NewGuid();
            var blogId5 = Guid.NewGuid();
            var blogId6 = Guid.NewGuid();
            var blogId7 = Guid.NewGuid();
            var blogId8 = Guid.NewGuid();
            var blogId9 = Guid.NewGuid();
            var blogId10 = Guid.NewGuid();
            var blogId11 = Guid.NewGuid();
            var blogId12 = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[]
                {
               "Id", "Title", "Content", "Description", "ImageUrl", "IsPublished", "CreatedAt", "CreatedBy", "UpdatedAt"
                },
                values: new object[,]
                {
               { blogId1, "Giúp Trẻ Em Vùng Cao Có Nước Sạch", "Nội dung chi tiết về việc xây dựng giếng khoan và lọc nước sạch cho các bản làng vùng cao...", "Tổng quan về dự án nước sạch cho trẻ em vùng cao.", "/images/blog/g1.jpg", true, DateTime.UtcNow.AddDays(-10), userAdminId, null },
               { blogId2, "Chương Trình Hỗ Trợ Dinh Dưỡng Cho Trẻ Em", "Chi tiết chương trình cung cấp thực phẩm dinh dưỡng cho trẻ em suy dinh dưỡng vùng cao...", "Giới thiệu chương trình hỗ trợ dinh dưỡng cho trẻ em vùng cao.", "/images/blog/g1.jpg", false, DateTime.UtcNow.AddDays(-5), userAdminId, null },
               { blogId3, "Xây Dựng Lớp Học Vùng Núi", "Thông tin về dự án xây trường học mới cho trẻ em ở vùng sâu vùng xa...", "Cải thiện điều kiện học tập cho trẻ em vùng núi.", "/images/blog/g1.jpg", true, DateTime.UtcNow.AddDays(-20), userAdminId, null },
               { blogId4, "Tặng Quần Áo Ấm Cho Trẻ Em Mùa Đông", "Chiến dịch quyên góp và phân phát áo ấm cho trẻ em vùng lạnh...", "Mang đến hơi ấm mùa đông cho trẻ em vùng cao.", "/images/blog/g1.jpg", true, DateTime.UtcNow.AddDays(-15), userAdminId, null },
               { blogId5, "Cung Cấp Sách Giáo Khoa Và Dụng Cụ Học Tập", "Hỗ trợ sách vở và dụng cụ học tập cho học sinh nghèo...", "Nâng cao chất lượng giáo dục thông qua việc hỗ trợ vật chất học tập.", "/images/blog/g2.jpg", true, DateTime.UtcNow.AddDays(-12), userAdminId, null },
               { blogId6, "Khám Sức Khỏe Định Kỳ Cho Trẻ Em", "Chương trình tổ chức các buổi khám sức khỏe miễn phí cho trẻ em...", "Quan tâm sức khỏe định kỳ cho trẻ em vùng xa.", "/images/blog/g2.jpg", false, DateTime.UtcNow.AddDays(-8), userAdminId, null },
               { blogId7, "Trại Hè Tình Nguyện Cho Trẻ Em", "Tổ chức các hoạt động trại hè bổ ích và vui chơi cho trẻ em có hoàn cảnh khó khăn...", "Mang đến nụ cười và niềm vui cho trẻ em mùa hè.", "/images/blog/g2.jpg", true, DateTime.UtcNow.AddDays(-18), userAdminId, null },
               { blogId8, "Hướng Nghiệp Và Định Hướng Tương Lai", "Chia sẻ kiến thức nghề nghiệp và định hướng tương lai cho học sinh trung học...", "Đồng hành cùng các em trong hành trình tương lai.", "/images/blog/g3.jpg", false, DateTime.UtcNow.AddDays(-6), userAdminId, null },
               { blogId9, "Học Bổng Cho Học Sinh Nghèo Vượt Khó", "Thông tin chi tiết về chương trình học bổng dành cho học sinh có hoàn cảnh khó khăn nhưng học giỏi...", "Khuyến khích học sinh tiếp tục học tập và phát triển.", "/images/blog/g3.jpg", true, DateTime.UtcNow.AddDays(-25), userAdminId, null },
               { blogId10, "Tổ Chức Ngày Hội Đọc Sách", "Khơi dậy niềm yêu thích đọc sách trong trẻ em thông qua các ngày hội đọc sách...", "Góp phần xây dựng văn hóa đọc cho thế hệ tương lai.", "/images/blog/g3.jpg", true, DateTime.UtcNow.AddDays(-7), userAdminId, null },
               { blogId11, "Chiến Dịch Trồng Cây Xanh Ở Trường Học", "Chi tiết về chiến dịch trồng cây xanh tại các điểm trường vùng cao nhằm tạo môi trường học tập xanh - sạch - đẹp...", "Góp phần cải thiện môi trường sống và học tập cho trẻ em.", "/images/blog/g4.jpg", true, DateTime.UtcNow.AddDays(-4), userAdminId, null },
               { blogId12, "Khóa Học Kỹ Năng Sống Cho Học Sinh", "Chương trình giáo dục kỹ năng sống giúp học sinh tự tin, giao tiếp và xử lý tình huống trong cuộc sống...", "Giáo dục toàn diện cho học sinh thông qua kỹ năng mềm.", "/images/blog/g4.jpg", true, DateTime.UtcNow.AddDays(-2), userAdminId, null }
                });

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
                   "Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội là chương trình thiện nguyện đầy ý nghĩa, nhằm mang đến niềm vui và sự động viên thiết thực cho các em nhỏ có hoàn cảnh khó khăn tại các vùng ngoại thành Hà Nội. Sự kiện không chỉ là dịp các em nhận được sách vở, quà tặng và bữa ăn dinh dưỡng mà còn là nơi các em giao lưu, tham gia các hoạt động văn nghệ, trò chơi tập thể cùng hơn 200 tình nguyện viên và các nhà hảo tâm.",
                   "Sự kiện “Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội” sẽ diễn ra vào ngày 12/7/2025 tại Trường Tiểu học A, Hoài Đức, Hà Nội, dự kiến chào đón hơn 500 em nhỏ thuộc các hộ gia đình khó khăn trên địa bàn. Tại đây, mỗi em sẽ được nhận một phần quà bao gồm sách vở, đồ dùng học tập, sữa, bánh kẹo và một suất ăn trưa dinh dưỡng. Ngoài hoạt động trao quà, chương trình còn tổ chức các tiết mục văn nghệ do chính các em và tình nguyện viên biểu diễn, các trò chơi vận động, đố vui nhận thưởng, giúp các em giao lưu, tự tin hơn và có thêm nhiều kỷ niệm đẹp. Ban tổ chức bố trí đội ngũ y tế hỗ trợ, đảm bảo an toàn và sức khỏe cho tất cả người tham dự. Sự kiện cũng tạo cơ hội cho cộng đồng cùng chung tay chia sẻ, lan tỏa yêu thương và tiếp thêm động lực để các em nhỏ vượt qua khó khăn, vươn lên trong học tập cũng như cuộc sống.",
                   "https://images.baodantoc.vn/uploads/2021/Th%C3%A1ng_12/Ng%C3%A0y%203/TRUNG/T%E1%BA%B7ng%20qu%C3%A0/A1%20-%20OK.jpg",
                   new DateTime(2025, 7, 12, 8, 0, 0),
                   new DateTime(2025, 7, 12, 16, 0, 0),
                   "Trường Tiểu học A, Thị trấn Trạm Trôi, Huyện Hoài Đức, Hà Nội",
                   50000000m,
                   0m,
                   (int)EventStatus.Ongoing,
                   userId,
                   orgId
               },
               {
                   Guid.NewGuid(),
                   "Ngày hội Hiến Máu Nhân Đạo Đà Nẵng",
                   "Ngày hội Hiến Máu Nhân Đạo Đà Nẵng - Một hành trình ý nghĩa lan tỏa yêu thương, kêu gọi toàn thể cộng đồng Đà Nẵng cùng chung tay giúp đỡ những bệnh nhân cần máu. Sự kiện diễn ra tại Nhà Văn hóa Thiếu nhi Đà Nẵng với không gian rộng rãi, trang thiết bị hiện đại, tạo điều kiện cho người tham gia có trải nghiệm tốt nhất.",
                   "Thời gian diễn ra từ 8h sáng đến 17h chiều ngày 20/7/2025. Mỗi người tham gia hiến máu sẽ được nhận quà tặng, giấy chứng nhận và phần ăn nhẹ. Ban tổ chức bố trí đội ngũ y tế túc trực, hỗ trợ tư vấn sức khỏe, theo dõi sát sao toàn bộ quá trình hiến máu. Đây là sự kiện thường niên, mong muốn kết nối các tình nguyện viên, nhóm máu hiếm và các tổ chức xã hội tại Đà Nẵng.",
                   "https://media-cdn-v2.laodong.vn/storage/newsportal/2022/12/11/1126243/484Fa6554cc4959accd5.jpg",
                   new DateTime(2025, 7, 20, 8, 0, 0),
                   new DateTime(2025, 7, 20, 17, 0, 0),
                   "Nhà Văn hóa Thiếu nhi Đà Nẵng, 2 Phan Đăng Lưu, Hải Châu, Đà Nẵng",
                   0m,
                   0m,
                   (int)EventStatus.Ongoing,
                   userId,
                   orgId
               }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

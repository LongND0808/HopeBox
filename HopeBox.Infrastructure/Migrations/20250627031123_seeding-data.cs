using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using static HopeBox.Common.Enum.Enumerate;
using static System.Net.Mime.MediaTypeNames;

namespace HopeBox.Infrastructure.Migrations
{
    public partial class seedingdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // User and Role IDs
            var userAdminId = Guid.NewGuid();
            var userCustomer1Id = Guid.NewGuid();
            var userCustomer2Id = Guid.NewGuid();
            var userVolunteerId = Guid.NewGuid();
            var userManagerId1 = Guid.NewGuid();
            var userManagerId2 = Guid.NewGuid();
            var userManagerId3 = Guid.NewGuid();
            var userManagerId4 = Guid.NewGuid();
            var userManagerId5 = Guid.NewGuid();
            var userManagerId6 = Guid.NewGuid();
            var roleAdminId = Guid.NewGuid();
            var roleCustomerId = Guid.NewGuid();
            var roleManagerId = Guid.NewGuid();
            var confirmEmailAdminId = Guid.NewGuid();
            var confirmEmailCustomer1Id = Guid.NewGuid();
            var confirmEmailCustomer2Id = Guid.NewGuid();
            var confirmEmailVolunteerId = Guid.NewGuid();

            // Organization IDs
            var orgId1 = Guid.NewGuid();
            var orgId2 = Guid.NewGuid();

            // Cause IDs
            var causeWaterId = Guid.NewGuid();
            var causeFoodId = Guid.NewGuid();
            var causeMedicineId = Guid.NewGuid();
            var causeEducationId = Guid.NewGuid();
            var causeShelterId = Guid.NewGuid();
            var causeClothingId = Guid.NewGuid();
            var causeHealthId = Guid.NewGuid();
            var causeCommunityId = Guid.NewGuid();

            // Event IDs
            var eventCharityId = Guid.NewGuid();
            var eventBloodDriveId = Guid.NewGuid();
            var eventLaoCaiId = Guid.NewGuid();
            var eventHueId = Guid.NewGuid();
            var eventCanThoId = Guid.NewGuid();
            var eventDakLakId = Guid.NewGuid();

            // Media IDs
            var mediaIds = new Guid[10];
            for (int i = 0; i < 10; i++)
            {
                mediaIds[i] = Guid.NewGuid();
            }

            // Blog IDs
            var blogIds = new Guid[15];
            for (int i = 0; i < 15; i++)
            {
                blogIds[i] = Guid.NewGuid();
            }

            // ReliefItem IDs
            var reliefItemWaterId = Guid.NewGuid();
            var reliefItemFoodId = Guid.NewGuid();
            var reliefItemMedicineId = Guid.NewGuid();
            var reliefItemBookId = Guid.NewGuid();
            var reliefItemClothingId = Guid.NewGuid();
            var additionalReliefItemIds = new Guid[30];
            for (int i = 0; i < 30; i++)
            {
                additionalReliefItemIds[i] = Guid.NewGuid();
            }

            // ReliefPackage IDs
            var reliefPackageWaterId = Guid.NewGuid();
            var reliefPackageFoodId = Guid.NewGuid();
            var reliefPackageMedicineId = Guid.NewGuid();

            // Donation IDs
            var donation1Id = Guid.NewGuid();
            var donation2Id = Guid.NewGuid();
            var donation3Id = Guid.NewGuid();

            // Volunteer IDs
            var volunteer1Id = Guid.NewGuid();
            var volunteer2Id = Guid.NewGuid();

            // Feedback IDs
            var feedback1Id = Guid.NewGuid();
            var feedback2Id = Guid.NewGuid();

            // Sponsor IDs
            var sponsor1Id = Guid.NewGuid();
            var sponsor2Id = Guid.NewGuid();

            // Testimonial IDs
            var testimonial1Id = 1;
            var testimonial2Id = 2;
            var testimonial3Id = 3;

            // Notification IDs
            var notification1Id = Guid.NewGuid();
            var notification2Id = Guid.NewGuid();

            // Insert Organizations
            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[]
                {
                    "Id", "Name", "Email", "PhoneNumber", "Address", "City", "Description", "Website", "Verified", "CreatedAt"
                },
                values: new object[,]
                {
                    {
                        orgId1, "Global Hope Org", "info@globalhope.org", "0988888888",
                        "456 Charity Lane", "HCMC", "Supporting humanitarian efforts worldwide",
                        "https://globalhope.org", true, DateTime.UtcNow
                    },
                    {
                        orgId2, "Vietnam Care Foundation", "contact@vietcare.org", "0909999999",
                        "789 Unity Road", "Hanoi", "Empowering communities through education and health",
                        "https://vietcare.org", true, DateTime.UtcNow
                    }
                });

            // Insert Users
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                    "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
                    "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed",
                    "FullName", "Address", "DateOfBirth", "Gender", "AvatarUrl", "Point", "UserStatus",
                    "OrganizationId", "AccessFailedCount", "LockoutEnabled", "LockoutEnd", "TwoFactorEnabled"
                },
                values: new object[,]
                {
                    {
                        userAdminId, "Nguyễn Đăng Long", "NGUYEN DANG LONG", "admin@hopebox.org", "ADMIN@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Admin@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "08684532345", true,
                        "Nguyễn Đăng Long", "123 Charity Lane, HCMC", new DateTime(1990, 1, 1), (int)Gender.Male,
                        "images/user/admin.jpg", 100, (int)UserStatus.Active, orgId1,
                        0, false, null, false
                    },
                    {
                        userCustomer1Id, "Nguyễn Hoàng Thắng", "NGUYEN HOANG THANG", "customer1@hopebox.org", "CUSTOMER1@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Customer@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "08684531111", true,
                        "Nguyễn Hoàng Thắng", "456 Hope Street, Hanoi", new DateTime(1995, 5, 15), (int)Gender.Male,
                        null, 50, (int)UserStatus.Active, null,
                        0, false, null, false
                    },
                    {
                        userCustomer2Id, "Trần Thị Minh Anh", "TRAN THI MINH ANH", "customer2@hopebox.org", "CUSTOMER2@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Customer@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "09012345678", true,
                        "Trần Thị Minh Anh", "789 Peace Road, Da Nang", new DateTime(1998, 3, 20), (int)Gender.Female,
                        "images/user/customer2.jpg", 75, (int)UserStatus.Active, null,
                        0, false, null, false
                    },
                    {
                        userVolunteerId, "Lê Văn Hùng", "LE VAN HUNG", "volunteer@hopebox.org", "VOLUNTEER@HOPEBOX.ORG", false,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Volunteer@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "09123456789", false,
                        "Lê Văn Hùng", "101 Unity Avenue, Hue", new DateTime(1992, 7, 10), (int)Gender.Male,
                        null, 20, (int)UserStatus.Pending, orgId2,
                        0, false, null, false
                    },
                    {
                        userManagerId1, "Phạm Thị Hồng", "PHAM THI HONG", "manager1_org1@hopebox.org", "MANAGER1_ORG1@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Manager@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "0900000001", true,
                        "Phạm Thị Hồng", "12 Charity Street, HCMC", new DateTime(1987, 2, 20), (int)Gender.Female,
                        "images/user/manager1.jpg", 90, (int)UserStatus.Active, orgId1,
                        0, false, null, false
                    },
                    {
                        userManagerId2, "Ngô Văn Tài", "NGO VAN TAI", "manager2_org1@hopebox.org", "MANAGER2_ORG1@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Manager@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "0900000002", true,
                        "Ngô Văn Tài", "88 Love Road, HCMC", new DateTime(1990, 11, 5), (int)Gender.Male,
                        "images/user/manager2.jpg", 85, (int)UserStatus.Active, orgId1,
                        0, false, null, false
                    },
                    {
                        userManagerId3, "Đinh Thị Mai", "DINH THI MAI", "manager3_org1@hopebox.org", "MANAGER3_ORG1@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Manager@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "0900000003", true,
                        "Đinh Thị Mai", "99 Giving Blvd, HCMC", new DateTime(1992, 8, 12), (int)Gender.Female,
                        "images/user/manager3.jpg", 88, (int)UserStatus.Active, orgId1,
                        0, false, null, false
                    },
                    {
                        userManagerId4, "Trần Quốc Hưng", "TRAN QUOC HUNG", "manager1_org2@hopebox.org", "MANAGER1_ORG2@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Manager@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "0900000004", true,
                        "Trần Quốc Hưng", "10 Peace Alley, Hue", new DateTime(1985, 3, 10), (int)Gender.Male,
                        "images/user/manager4.jpg", 92, (int)UserStatus.Active, orgId2,
                        0, false, null, false
                    },
                    {
                        userManagerId5, "Lê Thị Thu Hà", "LE THI THU HA", "manager2_org2@hopebox.org", "MANAGER2_ORG2@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Manager@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "0900000005", true,
                        "Lê Thị Thu Hà", "55 Volunteer Rd, Hue", new DateTime(1993, 6, 25), (int)Gender.Female,
                        "images/user/manager5.jpg", 87, (int)UserStatus.Active, orgId2,
                        0, false, null, false
                    },
                    {
                        userManagerId6, "Nguyễn Văn Khải", "NGUYEN VAN KHAI", "manager3_org2@hopebox.org", "MANAGER3_ORG2@HOPEBOX.ORG", true,
                        new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Manager@123"),
                        Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "0900000006", true,
                        "Nguyễn Văn Khải", "102 Unity Blvd, Hue", new DateTime(1991, 4, 18), (int)Gender.Male,
                        "images/user/manager6.jpg", 89, (int)UserStatus.Active, orgId2,
                        0, false, null, false
                    }
                });

            // Insert Roles
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Code", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    { roleAdminId, "ADMIN", "Admin", "ADMIN", Guid.NewGuid().ToString() },
                    { roleCustomerId, "CUSTOMER", "Customer", "CUSTOMER", Guid.NewGuid().ToString() },
                    { roleManagerId, "MANAGER", "Manager", "MANAGER", Guid.NewGuid().ToString() }
                });

            // Insert UserRoles
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { userAdminId, roleAdminId },
                    { userCustomer1Id, roleCustomerId },
                    { userCustomer2Id, roleCustomerId },
                    { userVolunteerId, roleCustomerId },
                    { userManagerId1, roleManagerId },
                    { userManagerId2, roleManagerId },
                    { userManagerId3, roleManagerId },
                    { userManagerId4, roleManagerId },
                    { userManagerId5, roleManagerId },
                    { userManagerId6, roleManagerId }
                });

            // Insert ConfirmEmails
            migrationBuilder.InsertData(
                table: "ConfirmEmails",
                columns: new[] { "Id", "UserId", "RequestedAt", "ExpiresAt", "ConfirmCode", "IsConfirmed" },
                values: new object[,]
                {
                    { confirmEmailAdminId, userAdminId, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5), "12345678", true },
                    { confirmEmailCustomer1Id, userCustomer1Id, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5), "87654321", true },
                    { confirmEmailCustomer2Id, userCustomer2Id, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5), "45678912", false },
                    { confirmEmailVolunteerId, userVolunteerId, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(5), "78912345", false }
                });

            // Insert Causes
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
                        (int)CauseType.Water, new DateTime(2025, 6, 1), new DateTime(2025, 12, 1), 20000000m, 2000000m,
                        (int)CauseStatus.Ongoing, userAdminId, orgId1
                    },
                    {
                        causeFoodId, "Bữa Ăn Đầy Đủ", "Cung cấp thực phẩm cho trẻ em suy dinh dưỡng",
                        "Hỗ trợ các gia đình vùng cao bằng các phần quà thực phẩm thiết yếu hàng tháng cho trẻ nhỏ.",
                        "/images/causes/hero-food.jpg", "Thiếu ăn kéo dài khiến trẻ còi cọc và học kém",
                        "/images/causes/challenge-food.jpg", "Dinh dưỡng đầy đủ giúp trẻ lớn khôn khỏe mạnh",
                        "/images/causes/summary-food.jpg",
                        (int)CauseType.Food, new DateTime(2025, 7, 1), new DateTime(2025, 11, 1), 8000000m, 1500000m,
                        (int)CauseStatus.Ongoing, userAdminId, orgId1
                    },
                    {
                        causeMedicineId, "Tủ Thuốc Nhân Ái", "Cung cấp thuốc cơ bản cho trẻ em vùng cao",
                        "Phối hợp với trạm y tế để cung cấp thuốc hạ sốt, tiêu hóa, bông băng y tế cho các điểm trường và hộ gia đình có trẻ nhỏ.",
                        "/images/causes/hero-medicine.jpg", "Trẻ em không được điều trị kịp thời vì thiếu thuốc",
                        "/images/causes/challenge-medicine.jpg", "Thuốc cơ bản có thể cứu sống trẻ em",
                        "/images/causes/summary-medicine.jpg",
                        (int)CauseType.Medicine, new DateTime(2025, 8, 1), new DateTime(2025, 12, 31), 12000000m, 2000000m,
                        (int)CauseStatus.Ongoing, userAdminId, orgId1
                    },
                    {
                        causeEducationId, "Cặp Sách Đến Trường", "Tặng dụng cụ học tập và học phí",
                        "Tài trợ cặp sách, bút vở và đồng phục cho học sinh tiểu học và trung học ở vùng sâu vùng xa.",
                        "/images/causes/hero-edu.jpg", "Nhiều em phải nghỉ học vì không đủ điều kiện vật chất",
                        "/images/causes/challenge-edu.jpg", "Giáo dục là con đường thoát nghèo bền vững",
                        "/images/causes/summary-edu.jpg",
                        (int)CauseType.Education, new DateTime(2025, 6, 15), new DateTime(2025, 12, 15), 15000000m, 5000000m,
                        (int)CauseStatus.Ongoing, userAdminId, orgId1
                    },
                    {
                        causeShelterId, "Mái Ấm Vùng Cao", "Sửa chữa và xây mới nhà cho trẻ em khó khăn",
                        "Hỗ trợ xây dựng nhà ở an toàn sau thiên tai và cho những gia đình nghèo có trẻ em.",
                        "/images/causes/hero-shelter.jpg", "Trẻ em sống trong nhà tạm bợ không đảm bảo an toàn",
                        "/images/causes/challenge-shelter.jpg", "Một mái ấm vững chắc mang lại hy vọng và tương lai",
                        "/images/causes/summary-shelter.jpg",
                        (int)CauseType.Shelter, new DateTime(2025, 9, 1), new DateTime(2026, 1, 1), 20000000m, 8000000m,
                        (int)CauseStatus.Ongoing, userAdminId, orgId2
                    },
                    {
                        causeClothingId, "Áo Ấm Mùa Đông", "Gửi quần áo ấm cho trẻ em vùng núi",
                        "Tổ chức quyên góp và trao tặng áo khoác, khăn len, tất và giày dép cho trẻ em vào mùa đông lạnh giá.",
                        "/images/causes/hero-clothing.jpg", "Nhiều em chịu rét trong giá lạnh vì thiếu quần áo",
                        "/images/causes/challenge-clothing.jpg", "Một chiếc áo ấm cũng có thể cứu một sinh mạng",
                        "/images/causes/summary-clothing.jpg",
                        (int)CauseType.Clothing, new DateTime(2025, 10, 1), new DateTime(2025, 12, 31), 6000000m, 1000000m,
                        (int)CauseStatus.Ongoing, userAdminId, orgId2
                    },
                    {
                        causeHealthId, "Khám Sức Khỏe Miễn Phí", "Tổ chức khám sức khỏe cho trẻ em",
                        "Cung cấp dịch vụ khám sức khỏe định kỳ và tư vấn dinh dưỡng cho trẻ em ở vùng sâu vùng xa.",
                        "/images/causes/hero-health.jpg", "Trẻ em thiếu tiếp cận dịch vụ y tế cơ bản",
                        "/images/causes/challenge-health.jpg", "Sức khỏe là nền tảng cho tương lai trẻ em",
                        "/images/causes/summary-health.jpg",
                        (int)CauseType.Medicine, new DateTime(2025, 11, 1), new DateTime(2026, 3, 1), 10000000m, 2000000m,
                        (int)CauseStatus.Approved, userAdminId, orgId2
                    },
                    {
                        causeCommunityId, "Phát Triển Cộng Đồng", "Hỗ trợ phát triển cộng đồng bền vững",
                        "Xây dựng các chương trình đào tạo nghề và hỗ trợ sinh kế cho các gia đình vùng cao.",
                        "/images/causes/hero-community.jpg", "Thiếu cơ hội việc làm dẫn đến nghèo đói kéo dài",
                        "/images/causes/challenge-community.jpg", "Phát triển cộng đồng là chìa khóa thoát nghèo",
                        "/images/causes/summary-community.jpg",
                        (int)CauseType.Education, new DateTime(2025, 12, 1), new DateTime(2026, 6, 1), 18000000m, 4000000m,
                        (int)CauseStatus.Approved, userAdminId, orgId2
                    }
                });

            // Insert Events
            migrationBuilder.InsertData(
       table: "Events",
       columns: new[]
       {
                    "Id", "Title", "Description", "Detail", "BannerImage",
                    "StartDate", "EndDate", "Location",
                    "Latitude", "Longitude", "FormattedAddress",
                    "Status", "CreatedBy", "OrganizationId", "CauseId"
       },
       columnTypes: new[]
       {
                    "uniqueidentifier", "nvarchar(200)", "nvarchar(2000)", "nvarchar(max)",
                    "nvarchar(max)", "datetime2", "datetime2", "nvarchar(1000)",
                    "decimal(10,8)", "decimal(11,8)", "nvarchar(500)",
                    "int", "uniqueidentifier", "uniqueidentifier", "uniqueidentifier"
       },
       values: new object[,]
       {
                    {
                        eventCharityId,
                        "Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội",
                        "Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội là chương trình thiện nguyện đầy ý nghĩa, nhằm mang đến niềm vui và sự động viên thiết thực cho các em nhỏ có hoàn cảnh khó khăn tại các vùng ngoại thành Hà Nội. Sự kiện không chỉ là dịp các em nhận được sách vở, quà tặng và bữa ăn dinh dưỡng mà còn là nơi các em giao lưu, tham gia các hoạt động văn nghệ, trò chơi tập thể cùng hơn 200 tình nguyện viên và các nhà hảo tâm.",
                        "Sự kiện “Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội” sẽ diễn ra vào ngày 12/7/2025 tại Trường Tiểu học A, Hoài Đức, Hà Nội, dự kiến chào đón hơn 500 em nhỏ thuộc các hộ gia đình khó khăn trên địa bàn. Tại đây, mỗi em sẽ được nhận một phần quà bao gồm sách vở, đồ dùng học tập, sữa, bánh kẹo và một suất ăn trưa dinh dưỡng. Ngoài hoạt động trao quà, chương trình còn tổ chức các tiết mục văn nghệ do chính các em và tình nguyện viên biểu diễn, các trò chơi vận động, đố vui nhận thưởng, giúp các em giao lưu, tự tin hơn và có thêm nhiều kỷ niệm đẹp. Ban tổ chức bố trí đội ngũ y tế hỗ trợ, đảm bảo an toàn và sức khỏe cho tất cả người tham dự. Sự kiện cũng tạo cơ hội cho cộng đồng cùng chung tay chia sẻ, lan tỏa yêu thương và tiếp thêm động lực để các em nhỏ vượt qua khó khăn, vươn lên trong học tập cũng như cuộc sống.",
                        "https://images.baodantoc.vn/uploads/2021/Th%C3%A1ng_12/Ng%C3%A0y%203/TRUNG/T%E1%BA%B7ng%20qu%C3%A0/A1%20-%20OK.jpg",
                        new DateTime(2025, 7, 12, 8, 0, 0),
                        new DateTime(2025, 7, 12, 16, 0, 0),
                        "Trường Tiểu học A, Thị trấn Trạm Trôi, Huyện Hoài Đức, Hà Nội",
                        21.0285,
                        105.8542,
                        "Hoài Đức, Hà Nội, Việt Nam",
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1,
                        causeClothingId
                    },
                    {
                        eventBloodDriveId,
                        "Ngày hội Hiến Máu Nhân Đạo Đà Nẵng",
                        "Ngày hội Hiến Máu Nhân Đạo Đà Nẵng - Một hành trình ý nghĩa lan tỏa yêu thương, kêu gọi toàn thể cộng đồng Đà Nẵng cùng chung tay giúp đỡ những bệnh nhân cần máu. Sự kiện diễn ra tại Nhà Văn hóa Thiếu nhi Đà Nẵng với không gian rộng rãi, trang thiết bị hiện đại, tạo điều kiện cho người tham gia có trải nghiệm tốt nhất.",
                        "Thời gian diễn ra từ 8h sáng đến 17h chiều ngày 20/7/2025. Mỗi người tham gia hiến máu sẽ được nhận quà tặng, giấy chứng nhận và phần ăn nhẹ. Ban tổ chức bố trí đội ngũ y tế túc trực, hỗ trợ tư vấn sức khỏe, theo dõi sát sao toàn bộ quá trình hiến máu. Đây là sự kiện thường niên, mong muốn kết nối các tình nguyện viên, nhóm máu hiếm và các tổ chức xã hội tại Đà Nẵng.",
                        "https://media-cdn-v2.laodong.vn/storage/newsportal/2022/12/11/1126243/484Fa6554cc4959accd5.jpg",
                        new DateTime(2025, 7, 20, 8, 0, 0),
                        new DateTime(2025, 7, 20, 17, 0, 0),
                        "Nhà Văn hóa Thiếu nhi Đà Nẵng, 2 Phan Đăng Lưu, Hải Châu, Đà Nẵng",
                        16.0544,
                        108.2022,
                        "Hải Châu, Đà Nẵng, Việt Nam",
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1,
                        causeHealthId
                    },
                    {
                        eventLaoCaiId,
                        "Chương trình Áo Ấm Cho Em – Lào Cai 2025",
                        "Chương trình Áo Ấm Cho Em mang hơi ấm đến các em nhỏ vùng cao huyện Bắc Hà, Lào Cai, với mong muốn chia sẻ khó khăn mùa đông giá rét, đồng thời truyền cảm hứng học tập và niềm tin vượt khó.",
                        "Ngày 5/12/2025, tại Trường Tiểu học Bản Phố, huyện Bắc Hà, Lào Cai, sự kiện sẽ trao tặng hơn 400 áo ấm, mũ len, dép, sách vở và quà bánh cho các em học sinh dân tộc thiểu số. Ngoài hoạt động tặng quà, các em còn được tham gia trò chơi dân gian, vẽ tranh sáng tạo, giao lưu cùng đội tình nguyện viên miền xuôi. Chương trình huy động sự góp sức của cộng đồng, các doanh nghiệp, cá nhân cùng chung tay giúp các em nhỏ vững vàng đến trường trong mùa lạnh khắc nghiệt. Ban tổ chức cam kết minh bạch, mọi khoản đóng góp đều được cập nhật công khai trên hệ thống. Sự kiện góp phần lan tỏa yêu thương, kết nối trái tim từ miền xuôi đến miền ngược.",
                        "https://images.baodantoc.vn/uploads/2023/Thang-12/Ngay-17/Ngoc-Thu/DSC_7585%203.jpg",
                        new DateTime(2025, 12, 5, 7, 0, 0),
                        new DateTime(2025, 12, 5, 15, 0, 0),
                        "Trường Tiểu học Bản Phố, Bắc Hà, Lào Cai",
                        22.5212,
                        104.2831,
                        "Bắc Hà, Lào Cai, Việt Nam",
                        (int)EventStatus.Ongoing,
                        userManagerId2,
                        orgId1,
                        causeClothingId
                    },
                    {
                        eventHueId,
                        "Khám Sức Khỏe & Tặng Quà Cho Người Cao Tuổi - Huế 2025",
                        "Sự kiện khám sức khỏe định kỳ và trao quà dành cho gần 300 người cao tuổi có hoàn cảnh khó khăn tại phường Thủy Biều, TP Huế, góp phần chăm sóc sức khỏe cộng đồng và nâng cao ý thức phòng bệnh.",
                        "Diễn ra ngày 10/8/2025 tại Nhà Văn hóa Phường Thủy Biều, sự kiện có sự tham gia của đội ngũ y bác sĩ Bệnh viện Trung ương Huế trực tiếp khám tổng quát, đo huyết áp, siêu âm, tư vấn dinh dưỡng và phát thuốc miễn phí. Ngoài ra, mỗi cụ được nhận phần quà gồm sữa, đường, gạo và nhu yếu phẩm. Ban tổ chức tổ chức các tiết mục ca nhạc truyền thống, giao lưu văn nghệ, tạo không khí ấm cúng, gắn kết cộng đồng. Toàn bộ hoạt động, chi phí, nhà tài trợ đều được cập nhật công khai. Đây là sự kiện ý nghĩa tri ân thế hệ đi trước, lan tỏa giá trị sẻ chia trong cộng đồng.",
                        "https://images2.thanhnien.vn/thumb_w/640/528068263637045248/2023/7/14/img-0313-9602-16893259375721943780511.jpg",
                        new DateTime(2025, 8, 10, 7, 30, 0),
                        new DateTime(2025, 8, 10, 12, 0, 0),
                        "Nhà Văn hóa Phường Thủy Biều, TP Huế",
                        16.4478,
                        107.5772,
                        "Thủy Biều, Huế, Việt Nam",
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1,
                        causeHealthId
                    },
                    {
                        eventCanThoId,
                        "Hành Trình Nước Sạch Đến Vùng Sâu - Cần Thơ 2025",
                        "Chương trình lắp đặt hệ thống lọc nước sạch miễn phí cho các điểm trường tiểu học tại xã Trường Long, huyện Phong Điền, Cần Thơ, nhằm nâng cao chất lượng nước sinh hoạt cho học sinh vùng sâu.",
                        "Sự kiện diễn ra xuyên suốt tháng 9/2025 với mục tiêu hoàn thành 6 hệ thống lọc nước sạch cho các điểm trường tiểu học xã Trường Long. Đội kỹ thuật sẽ khảo sát, lắp đặt và bàn giao trực tiếp cho từng trường. Ngoài ra, ban tổ chức phối hợp địa phương tổ chức các buổi hướng dẫn vệ sinh, giáo dục ý thức bảo vệ nguồn nước, phát tờ rơi tuyên truyền và trao tặng bình đựng nước, quà bánh cho học sinh. Dự án kêu gọi các doanh nghiệp, cá nhân đóng góp để mang lại nguồn nước sạch, bảo vệ sức khỏe cho thế hệ trẻ. Tình hình quyên góp và tiến độ thực hiện đều công khai minh bạch trên nền tảng HopeBox.",
                        "https://cdnmedia.baotintuc.vn/Upload/XjfgEPYM30O8z6jY3MHxSw/files/2023/10/0310/beyond1.jpg",
                        new DateTime(2025, 9, 1, 8, 0, 0),
                        new DateTime(2025, 9, 30, 17, 0, 0),
                        "Xã Trường Long, huyện Phong Điền, Cần Thơ",
                        10.0486,
                        105.6195,
                        "Trường Long, Phong Điền, Cần Thơ, Việt Nam",
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1,
                        causeWaterId
                    },
                    {
                        eventDakLakId,
                        "Ngày Hội Trồng Rừng & Trao Sách - Đắk Lắk 2025",
                        "Sự kiện kêu gọi cộng đồng cùng chung tay trồng 2000 cây xanh phủ xanh đồi trọc tại xã Cư Êbur (Buôn Ma Thuột) kết hợp trao tặng sách, dụng cụ học tập cho trẻ em đồng bào Ê Đê.",
                        "Tổ chức vào ngày 14/7/2025, chương trình huy động hơn 300 tình nguyện viên và người dân địa phương tham gia trồng cây bản địa như sao đen, dầu rái, bằng lăng... Mỗi em nhỏ tham dự đều được nhận phần quà là sách thiếu nhi, balo, bút vở, bánh kẹo. Ngoài ra, ban tổ chức còn tổ chức workshop về bảo vệ môi trường, giao lưu văn nghệ dân tộc Ê Đê, trò chơi vận động. Sự kiện kết nối các nhà tài trợ, doanh nghiệp địa phương cùng cộng đồng chung tay vì màu xanh Đắk Lắk, vì tương lai trẻ em vùng cao. Tất cả các khoản chi, số cây trồng và quà tặng đều được cập nhật minh bạch trên nền tảng.",
                        "https://images2.thanhnien.vn/uploaded/thuyngan/2021_01_03/luasach1_LGRN.jpg?width=500",
                        new DateTime(2025, 7, 14, 8, 0, 0),
                        new DateTime(2025, 7, 14, 16, 30, 0),
                        "Xã Cư Êbur, TP. Buôn Ma Thuột, Đắk Lắk",
                        12.6783,
                        108.0494,
                        "Cư Êbur, Buôn Ma Thuột, Đắk Lắk, Việt Nam",
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1,
                        causeEducationId
                    }
       });

            // Insert Blogs
            migrationBuilder.InsertData(
                            table: "Blogs",
                            columns: new[]
                            {
                    "Id", "Title", "Content", "Description", "ImageUrl", "Slug",
                    "ViewCount", "LikeCount", "CommentCount", "ShareCount", "Tags",
                    "MetaDescription", "MetaKeywords", "ReadingTime", "CreatedAt",
                    "CreatedBy", "UpdatedAt", "IsPublished"
                            },
                            values: new object[,]
                            {
                    {
                        blogIds[0],
                        "Giúp Trẻ Em Vùng Cao Có Nước Sạch",
                        "Dự án này nhằm mục đích cung cấp nước sạch và an toàn cho các trẻ em ở vùng cao, nơi mà nguồn nước hiện tại đang bị ô nhiễm và không đảm bảo sức khỏe. Chúng tôi đã tiến hành khảo sát kỹ lưỡng và xây dựng các hệ thống lọc nước hiện đại, đồng thời đào tạo cộng đồng về cách sử dụng và bảo trì. Việc này không chỉ giúp cải thiện sức khỏe mà còn nâng cao chất lượng cuộc sống và khả năng học tập của các em. Chúng tôi cam kết minh bạch trong mọi hoạt động và luôn cập nhật tiến độ dự án cho cộng đồng và các nhà tài trợ. Dự án được thực hiện với sự hỗ trợ của các chuyên gia kỹ thuật và tình nguyện viên địa phương, đảm bảo tính bền vững và hiệu quả lâu dài. Các hệ thống lọc nước được lắp đặt sẽ phục vụ hàng trăm gia đình trong khu vực, mang lại nguồn nước sạch ổn định cho sinh hoạt hàng ngày và học tập của trẻ em.",
                        "Tổng quan về dự án nước sạch cho trẻ em vùng cao.",
                        "https://api.nongthonviet.com.vn/media/60759533068bb739ff924adc_images1452820_nuoc_sach.jpg",
                        "giup-tre-em-vung-cao-co-nuoc-sach",
                        551, 28, 29, 15,
                        "nước sạch, trẻ em, vùng cao",
                        "Tổng quan về dự án nước sạch cho trẻ em vùng cao, cung cấp hệ thống lọc nước hiện đại và bền vững.",
                        "nước sạch, dự án, trẻ em",
                        1,
                        DateTime.UtcNow.AddDays(-20),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[1],
                        "Chương Trình Hỗ Trợ Dinh Dưỡng",
                        "Chương trình hỗ trợ dinh dưỡng được thiết kế để giải quyết tình trạng suy dinh dưỡng ở trẻ em vùng cao thông qua việc cung cấp thực phẩm bổ dưỡng và giáo dục về dinh dưỡng. Chúng tôi làm việc trực tiếp với các gia đình để hiểu rõ nhu cầu và thách thức trong việc đảm bảo dinh dưỡng cho trẻ em. Các gói thực phẩm được cung cấp bao gồm gạo, thịt, rau củ, sữa và các loại vitamin cần thiết cho sự phát triển toàn diện của trẻ. Ngoài ra, chúng tôi còn tổ chức các buổi tập huấn cho phụ huynh về cách chế biến món ăn dinh dưỡng từ các nguyên liệu có sẵn tại địa phương. Chương trình cũng bao gồm việc theo dõi sức khỏe định kỳ để đánh giá hiệu quả và điều chỉnh kế hoạch hỗ trợ phù hợp với từng trẻ em. Sự hợp tác với các trạm y tế địa phương đảm bảo việc theo dõi chặt chẽ tình trạng dinh dưỡng và sức khỏe của các em.",
                        "Giới thiệu chương trình hỗ trợ dinh dưỡng.",
                        "https://thoidai.com.vn/stores/news_dataimages/2024/042024/06/13/vna-potal-bua-an-ban-tru-gop-phan-giam-ty-le-suy-dinh-duong-cho-hoc-sinh-mien-nui-quang-ngai-stand20240406131738.jpg?rt=20240406131743",
                        "chuong-trinh-ho-tro-dinh-duong",
                        811, 52, 9, 3,
                        "dinh dưỡng, trẻ em, suy dinh dưỡng",
                        "Giới thiệu chương trình hỗ trợ dinh dưỡng, cung cấp thực phẩm và giáo dục cho trẻ em vùng cao.",
                        "dinh dưỡng, thực phẩm, trẻ em",
                        1,
                        DateTime.UtcNow.AddDays(-18),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[2],
                        "Xây Dựng Lớp Học Vùng Núi",
                        "Dự án xây dựng lớp học vùng núi nhằm cải thiện điều kiện học tập cho trẻ em ở những vùng xa xôi nhất của đất nước. Chúng tôi không chỉ xây dựng cơ sở vật chất mà còn trang bị đầy đủ bàn ghế, bảng viết, và các dụng cụ học tập cần thiết. Mỗi lớp học được thiết kế để chịu được điều kiện thời tiết khắc nghiệt của vùng núi, đảm bảo an toàn và thoải mái cho các em trong suốt năm học. Chúng tôi cũng hỗ trợ đào tạo giáo viên địa phương, cung cấp tài liệu giảng dạy và phương pháp sư phạm hiện đại phù hợp với đặc điểm văn hóa bản địa. Dự án còn bao gồm việc xây dựng thư viện nhỏ với các sách giáo khoa và sách tham khảo phù hợp với từng độ tuổi. Ngoài ra, chúng tôi lắp đặt hệ thống điện năng lượng mặt trời để đảm bảo ánh sáng đầy đủ cho việc học tập. Sự tham gia của cộng đồng địa phương trong quá trình xây dựng không chỉ giúp giảm chi phí mà còn tạo ra sự gắn kết và trách nhiệm chung trong việc duy trì và bảo vệ ngôi trường.",
                        "Cải thiện điều kiện học tập cho trẻ em vùng núi.",
                        "https://static.kienviet.net/storage/uploads/2024/02/kienviet-truong-mam-non-va-tieu-hoc-lung-vai-1-6-1708770817.jpg",
                        "xay-dung-lop-hoc-vung-nui",
                        954, 65, 50, 4,
                        "giáo dục, xây dựng trường học, vùng núi",
                        "Cải thiện điều kiện học tập cho trẻ em vùng núi với cơ sở vật chất và thư viện hiện đại.",
                        "giáo dục, trường học, vùng núi",
                        2,
                        DateTime.UtcNow.AddDays(-15),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[3],
                        "Tặng Quần Áo Ấm Cho Trẻ Em",
                        "Chiến dịch tặng quần áo ấm cho trẻ em được tổ chức hàng năm nhằm giúp các em vượt qua mùa đông lạnh giá ở vùng cao. Chúng tôi thu thập và phân phát hàng nghìn bộ quần áo ấm, bao gồm áo khoác, áo len, quần dài, tất và giày dép phù hợp với từng độ tuổi. Mỗi bộ quần áo được kiểm tra kỹ lưỡng về chất lượng và độ an toàn trước khi đến tay các em. Chúng tôi cũng tổ chức các hoạt động vui chơi và giáo dục trong các buổi trao quà, tạo không khí ấm áp và niềm vui cho trẻ em. Ngoài việc trao quà trực tiếp, chúng tôi còn hướng dẫn các gia đình cách bảo quản và sử dụng quần áo hiệu quả. Chiến dịch cũng bao gồm việc quyên góp chăn màn, khăn len và các vật dụng giữ ấm khác. Sự tham gia của các tình nguyện viên từ khắp nơi đã tạo nên một mạng lưới yêu thương rộng lớn, lan tỏa tinh thần tương thân tương ái trong cộng đồng. Chúng tôi cũng phối hợp với các trường học địa phương để đảm bảo các em có đủ quần áo ấm để đến trường trong những ngày lạnh nhất.",
                        "Mang đến hơi ấm mùa đông cho trẻ em vùng cao.",
                        "https://media.treemvietnam.net.vn//files/phuongnhung26/2022/12/25/anh-so-1-074258.jpg",
                        "tang-quan-ao-am-cho-tre-em",
                        772, 79, 39, 11,
                        "quần áo ấm, mùa đông, trẻ em vùng cao",
                        "Mang đến hơi ấm mùa đông cho trẻ em vùng cao với quần áo ấm và hoạt động giáo dục.",
                        "quần áo ấm, mùa đông, từ thiện",
                        2,
                        DateTime.UtcNow.AddDays(-12),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[4],
                        "Cung Cấp Sách Giáo Khoa",
                        "Chương trình cung cấp sách giáo khoa nhằm đảm bảo mọi trẻ em đều có cơ hội tiếp cận giáo dục chất lượng bất kể hoàn cảnh gia đình. Chúng tôi thu thập, phân loại và phân phát hàng nghìn cuốn sách giáo khoa từ lớp 1 đến lớp 12, cùng với các sách tham khảo và truyện thiếu nhi. Mỗi bộ sách được đóng gói cẩn thận và giao trực tiếp đến tay học sinh hoặc thông qua các trường học địa phương. Chúng tôi cũng tổ chức các buổi đọc sách cộng đồng, khuyến khích trẻ em phát triển thói quen đọc sách từ sớm. Ngoài sách giáo khoa, chúng tôi còn cung cấp các dụng cụ học tập như bút, vở, thước kẻ, và cặp sách. Chương trình cũng bao gồm việc hỗ trợ xây dựng các thư viện nhỏ tại các điểm trường xa xôi. Sự hợp tác với các nhà xuất bản và tác giả đã giúp chúng tôi có được nguồn sách phong phú và chất lượng. Chúng tôi cũng tổ chức các cuộc thi viết văn và kể chuyện để khuyến khích trẻ em thể hiện tài năng và sự sáng tạo của mình.",
                        "Nâng cao chất lượng giáo dục thông qua hỗ trợ vật chất.",
                        "https://cdn.lawnet.vn/uploads/NewsThumbnail/2019/11/20/090108mohinhvnen.jpg",
                        "cung-cap-sach-giao-khoa",
                        166, 23, 26, 2,
                        "sách giáo khoa, giáo dục, học tập",
                        "Nâng cao chất lượng giáo dục với sách giáo khoa và thư viện cho trẻ em vùng xa.",
                        "sách giáo khoa, giáo dục, trẻ em",
                        2,
                        DateTime.UtcNow.AddDays(-10),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[5],
                        "Khám Sức Khỏe Định Kỳ",
                        "Chương trình khám sức khỏe miễn phí cung cấp dịch vụ y tế định kỳ cho trẻ em ở vùng sâu vùng xa. Chúng tôi phối hợp với các trạm y tế địa phương để tổ chức các buổi khám tổng quát, đo huyết áp, kiểm tra thị lực và tư vấn dinh dưỡng. Mỗi buổi khám đều có đội ngũ y bác sĩ chuyên môn cao, đảm bảo các em nhận được sự chăm sóc tốt nhất. Ngoài ra, chúng tôi cung cấp thuốc cơ bản và tài liệu hướng dẫn chăm sóc sức khỏe cho gia đình. Chương trình nhằm nâng cao nhận thức về sức khỏe và cải thiện chất lượng cuộc sống cho trẻ em.",
                        "Quan tâm sức khỏe định kỳ cho trẻ em vùng xa.",
                        "/images/blog/health.jpg",
                        "kham-suc-khoe-dinh-ky",
                        0, 0, 0, 0,
                        "sức khỏe, trẻ em, khám bệnh",
                        "Quan tâm sức khỏe định kỳ cho trẻ em vùng xa với các buổi khám và tư vấn y tế.",
                        "sức khỏe, trẻ em, từ thiện",
                        1,
                        DateTime.UtcNow.AddDays(-7),
                        userAdminId,
                        null,
                        false
                    },
                    {
                        blogIds[6],
                        "Trại Hè Tình Nguyện",
                        "Trại hè tình nguyện mang đến cho trẻ em khó khăn cơ hội tham gia các hoạt động vui chơi, học tập và giao lưu. Chúng tôi tổ chức các trò chơi đội nhóm, workshop kỹ năng sống và các buổi biểu diễn văn nghệ để khơi dậy sự tự tin và sáng tạo ở các em. Trại hè còn có sự tham gia của các tình nguyện viên, tạo môi trường ấm áp và truyền cảm hứng. Chương trình nhằm mang lại niềm vui và những kỷ niệm đẹp cho trẻ em trong mùa hè.",
                        "Mang đến nụ cười và niềm vui cho trẻ em mùa hè.",
                        "/images/blog/camp.jpg",
                        "trai-he-tinh-nguyen",
                        0, 0, 0, 0,
                        "trại hè, tình nguyện, trẻ em",
                        "Mang đến nụ cười và niềm vui cho trẻ em mùa hè qua các hoạt động trại hè tình nguyện.",
                        "trại hè, tình nguyện, trẻ em",
                        1,
                        DateTime.UtcNow.AddDays(-20),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[7],
                        "Hướng Nghiệp Cho Học Sinh",
                        "Chương trình hướng nghiệp giúp học sinh khám phá các cơ hội nghề nghiệp và định hướng tương lai. Chúng tôi tổ chức các buổi hội thảo với các chuyên gia, chia sẻ kiến thức về các ngành nghề và kỹ năng cần thiết. Học sinh được tham gia các hoạt động thực tế như tham quan doanh nghiệp và thử sức với các công việc đơn giản. Chương trình nhằm xây dựng sự tự tin và chuẩn bị tốt hơn cho các em trong hành trình trưởng thành.",
                        "Đồng hành cùng các em trong hành trình tương lai.",
                        "/images/blog/career.jpg",
                        "huong-nghiep-cho-hoc-sinh",
                        0, 0, 0, 0,
                        "hướng nghiệp, học sinh, tương lai",
                        "Đồng hành cùng học sinh trong hành trình tương lai với các buổi hướng nghiệp.",
                        "hướng nghiệp, học sinh, giáo dục",
                        1,
                        DateTime.UtcNow.AddDays(-5),
                        userAdminId,
                        null,
                        false
                    },
                    {
                        blogIds[8],
                        "Học Bổng Cho Học Sinh Nghèo",
                        "Chương trình học bổng hỗ trợ học sinh có hoàn cảnh khó khăn tiếp tục con đường học vấn. Chúng tôi cung cấp học bổng toàn phần và bán phần, bao gồm học phí, sách vở và dụng cụ học tập. Ngoài ra, chương trình còn tổ chức các buổi tư vấn học tập và định hướng để giúp các em đạt thành tích tốt hơn. Mục tiêu là khuyến khích học sinh vượt qua khó khăn và theo đuổi ước mơ.",
                        "Khuyến khích học sinh tiếp tục học tập.",
                        "/images/blog/scholarship.jpg",
                        "hoc-bong-cho-hoc-sinh-ngheo",
                        0, 0, 0, 0,
                        "học bổng, học sinh, khó khăn",
                        "Khuyến khích học sinh khó khăn tiếp tục học tập với học bổng và tư vấn.",
                        "học bổng, học sinh, giáo dục",
                        1,
                        DateTime.UtcNow.AddDays(-25),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[9],
                        "Ngày Hội Đọc Sách",
                        "Ngày hội đọc sách khơi dậy niềm yêu thích đọc sách ở trẻ em thông qua các hoạt động như đọc truyện, thi kể chuyện và triển lãm sách. Chúng tôi cung cấp sách thiếu nhi, sách giáo khoa và tài liệu tham khảo miễn phí cho các em. Sự kiện còn có sự tham gia của các tác giả và tình nguyện viên, tạo không khí sôi động và truyền cảm hứng. Mục tiêu là xây dựng văn hóa đọc cho thế hệ trẻ.",
                        "Góp phần xây dựng văn hóa đọc cho thế hệ tương lai.",
                        "/images/blog/reading.jpg",
                        "ngay-hoi-doc-sach",
                        0, 0, 0, 0,
                        "đọc sách, trẻ em, văn hóa",
                        "Góp phần xây dựng văn hóa đọc cho trẻ em qua các ngày hội đọc sách.",
                        "đọc sách, trẻ em, giáo dục",
                        1,
                        DateTime.UtcNow.AddDays(-7),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[10],
                        "Trồng Cây Xanh Ở Trường Học",
                        "Chiến dịch trồng cây xanh tại các điểm trường vùng cao nhằm cải thiện môi trường học tập và nâng cao nhận thức về bảo vệ môi trường. Chúng tôi phối hợp với học sinh và giáo viên để trồng các loại cây bản địa, đồng thời tổ chức các buổi giáo dục về tầm quan trọng của cây xanh. Chương trình cũng cung cấp dụng cụ làm vườn và hướng dẫn chăm sóc cây để đảm bảo sự phát triển bền vững.",
                        "Cải thiện môi trường sống và học tập cho trẻ em.",
                        "/images/blog/trees.jpg",
                        "trong-cay-xanh-o-truong-hoc",
                        0, 0, 0, 0,
                        "trồng cây, trường học, trẻ em",
                        "Cải thiện môi trường học tập cho trẻ em qua chiến dịch trồng cây xanh.",
                        "trồng cây, trường học, môi trường",
                        1,
                        DateTime.UtcNow.AddDays(-4),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[11],
                        "Khóa Học Kỹ Năng Sống",
                        "Khóa học kỹ năng sống giúp học sinh phát triển sự tự tin, kỹ năng giao tiếp và giải quyết vấn đề. Chúng tôi tổ chức các buổi workshop về quản lý thời gian, làm việc nhóm và tư duy sáng tạo, với sự hướng dẫn của các chuyên gia. Các hoạt động thực hành giúp học sinh áp dụng kiến thức vào thực tế. Mục tiêu là trang bị cho các em những kỹ năng cần thiết để thành công trong cuộc sống.",
                        "Giáo dục toàn diện cho học sinh.",
                        "/images/blog/skills.jpg",
                        "khoa-hoc-ky-nang-song",
                        0, 0, 0, 0,
                        "kỹ năng sống, học sinh, giáo dục",
                        "Giáo dục toàn diện cho học sinh qua các khóa học kỹ năng sống.",
                        "kỹ năng sống, học sinh, giáo dục",
                        1,
                        DateTime.UtcNow.AddDays(-3),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[12],
                        "Hỗ Trợ Nhà Ở Vùng Cao",
                        "Chương trình hỗ trợ nhà ở xây dựng những ngôi nhà an toàn cho các gia đình nghèo ở vùng cao. Chúng tôi sử dụng vật liệu bền vững và thiết kế phù hợp với điều kiện thời tiết khắc nghiệt. Ngoài ra, chương trình còn hỗ trợ sửa chữa nhà cửa và cung cấp vật dụng sinh hoạt cơ bản. Sự tham gia của cộng đồng địa phương đảm bảo tính bền vững và hiệu quả của dự án.",
                        "Mang lại mái ấm bền vững cho cộng đồng.",
                        "/images/blog/shelter.jpg",
                        "ho-tro-nha-o-vung-cao",
                        0, 0, 0, 0,
                        "nhà ở, vùng cao, từ thiện",
                        "Mang lại mái ấm bền vững cho cộng đồng vùng cao với các ngôi nhà an toàn.",
                        "nhà ở, vùng cao, từ thiện",
                        1,
                        DateTime.UtcNow.AddDays(-2),
                        userAdminId,
                        null,
                        true
                    },
                    {
                        blogIds[13],
                        "Chương Trình Y Tế Cộng Đồng",
                        "Chương trình y tế cộng đồng tổ chức các buổi khám sức khỏe và tư vấn y tế miễn phí cho cộng đồng vùng sâu vùng xa. Chúng tôi cung cấp dịch vụ kiểm tra sức khỏe tổng quát, phát thuốc và hướng dẫn phòng ngừa bệnh tật. Đội ngũ y bác sĩ và tình nguyện viên làm việc chặt chẽ với cộng đồng để nâng cao nhận thức và cải thiện sức khỏe lâu dài.",
                        "Cải thiện sức khỏe cộng đồng vùng sâu vùng xa.",
                        "/images/blog/healthcare.jpg",
                        "chuong-trinh-y-te-cong-dong",
                        0, 0, 0, 0,
                        "y tế, cộng đồng, sức khỏe",
                        "Cải thiện sức khỏe cộng đồng vùng sâu vùng xa với khám sức khỏe miễn phí.",
                        "y tế, cộng đồng, sức khỏe",
                        1,
                        DateTime.UtcNow.AddDays(-6),
                        userAdminId,
                        null,
                        false
                    },
                    {
                        blogIds[14],
                        "Ngày Hội Môi Trường",
                        "Ngày hội môi trường tuyên truyền về bảo vệ môi trường và tổ chức trồng cây xanh tại các cộng đồng địa phương. Chúng tôi phối hợp với học sinh, giáo viên và cư dân để trồng cây, dọn dẹp rác thải và tổ chức các buổi giáo dục môi trường. Sự kiện nhằm nâng cao nhận thức và khuyến khích cộng đồng tham gia vào các hoạt động bảo vệ thiên nhiên.",
                        "Lan tỏa ý thức bảo vệ môi trường trong cộng đồng.",
                        "/images/blog/environment.jpg",
                        "ngay-hoi-moi-truong",
                        0, 0, 0, 0,
                        "môi trường, trồng cây, cộng đồng",
                        "Lan tỏa ý thức bảo vệ môi trường trong cộng đồng qua ngày hội trồng cây.",
                        "môi trường, trồng cây, cộng đồng",
                        1,
                        DateTime.UtcNow.AddDays(-1),
                        userAdminId,
                        null,
                        true
                    }
                            });
            // Insert Shares
            migrationBuilder.InsertData(
                table: "Shares",
                columns: new[]
                {
                    "Id", "UserId", "BlogId", "Platform", "ShareUrl", "ShareCaption", "CreatedAt"
                },
                values: new object[,]
                {
                    {
                        Guid.NewGuid(),
                        userAdminId,
                        blogIds[0],
                        (int)SharePlatform.Facebook,
                        "https://hopebox.long2003-2014.workers.dev/blogs/giup-tre-em-vung-cao-co-nuoc-sach",
                        "Chia sẻ bài viết: Giúp Trẻ Em Vùng Cao Có Nước Sạch",
                        DateTime.UtcNow.AddDays(-19)
                    },
                    {
                        Guid.NewGuid(),
                        userAdminId,
                        blogIds[1],
                        (int)SharePlatform.Twitter,
                        "https://hopebox.long2003-2014.workers.dev/blogs/chuong-trinh-ho-tro-dinh-duong",
                        "Chia sẻ bài viết: Chương Trình Hỗ Trợ Dinh Dưỡng",
                        DateTime.UtcNow.AddDays(-17)
                    },
                    {
                        Guid.NewGuid(),
                        userAdminId,
                        blogIds[2],
                        (int)SharePlatform.Tiktok,
                        "https://hopebox.long2003-2014.workers.dev/blogs/xay-dung-lop-hoc-vung-nui",
                        "Chia sẻ bài viết: Xây Dựng Lớp Học Vùng Núi",
                        DateTime.UtcNow.AddDays(-14)
                    },
                    {
                        Guid.NewGuid(),
                        userAdminId,
                        blogIds[3],
                        (int)SharePlatform.Facebook,
                        "https://hopebox.long2003-2014.workers.dev/blogs/tang-quan-ao-am-cho-tre-em",
                        "Chia sẻ bài viết: Tặng Quần Áo Ấm Cho Trẻ Em",
                        DateTime.UtcNow.AddDays(-11)
                    },
                    {
                        Guid.NewGuid(),
                        userAdminId,
                        blogIds[4],
                        (int)SharePlatform.Twitter,
                        "https://hopebox.long2003-2014.workers.dev/blogs/cung-cap-sach-giao-khoa",
                        "Chia sẻ bài viết: Cung Cấp Sách Giáo Khoa",
                        DateTime.UtcNow.AddDays(-9)
                    }
                });

            // Insert ReliefItems
            migrationBuilder.InsertData(
                table: "ReliefItems",
                columns: new[] { "Id", "ItemName", "Unit", "UnitPrice" },
                values: new object[,]
                {
                    { reliefItemWaterId, "Bình lọc nước", (int)Unit.Bottle, 150000m },
                    { reliefItemFoodId, "Gói thực phẩm", (int)Unit.Pack, 100000m },
                    { reliefItemMedicineId, "Hộp thuốc y tế", (int)Unit.Box, 200000m },
                    { reliefItemBookId, "Bộ sách giáo khoa", (int)Unit.Pack, 50000m },
                    { reliefItemClothingId, "Bộ quần áo ấm", (int)Unit.Pack, 80000m },
                    { additionalReliefItemIds[0], "Gạo", (int)Unit.kg, 15000m },
                    { additionalReliefItemIds[1], "Mì gói", (int)Unit.Pack, 25000m },
                    { additionalReliefItemIds[2], "Nước suối", (int)Unit.Bottle, 10000m },
                    { additionalReliefItemIds[3], "Dầu ăn", (int)Unit.Bottle, 35000m },
                    { additionalReliefItemIds[4], "Nước tương", (int)Unit.Bottle, 20000m },
                    { additionalReliefItemIds[5], "Đường", (int)Unit.kg, 18000m },
                    { additionalReliefItemIds[6], "Muối", (int)Unit.kg, 7000m },
                    { additionalReliefItemIds[7], "Bánh quy", (int)Unit.Pack, 22000m },
                    { additionalReliefItemIds[8], "Sữa hộp", (int)Unit.Carton, 120000m },
                    { additionalReliefItemIds[9], "Bột ngọt", (int)Unit.Bag, 15000m },
                    { additionalReliefItemIds[10], "Nước rửa tay", (int)Unit.Bottle, 25000m },
                    { additionalReliefItemIds[11], "Khẩu trang", (int)Unit.Box, 40000m },
                    { additionalReliefItemIds[12], "Xà phòng", (int)Unit.Box, 30000m },
                    { additionalReliefItemIds[13], "Trứng", (int)Unit.Box, 35000m },
                    { additionalReliefItemIds[14], "Thịt hộp", (int)Unit.Box, 50000m },
                    { additionalReliefItemIds[15], "Cháo ăn liền", (int)Unit.Pack, 17000m },
                    { additionalReliefItemIds[16], "Tã giấy", (int)Unit.Bag, 90000m },
                    { additionalReliefItemIds[17], "Kem đánh răng", (int)Unit.Box, 15000m },
                    { additionalReliefItemIds[18], "Bàn chải", (int)Unit.Pack, 12000m },
                    { additionalReliefItemIds[19], "Nước lau sàn", (int)Unit.Bottle, 35000m },
                    { additionalReliefItemIds[20], "Giấy vệ sinh", (int)Unit.Pack, 20000m },
                    { additionalReliefItemIds[21], "Khăn ướt", (int)Unit.Pack, 18000m },
                    { additionalReliefItemIds[22], "Ngũ cốc", (int)Unit.Box, 45000m },
                    { additionalReliefItemIds[23], "Bánh mì", (int)Unit.Bag, 25000m },
                    { additionalReliefItemIds[24], "Rau củ sấy", (int)Unit.Bag, 30000m },
                    { additionalReliefItemIds[25], "Nước trái cây", (int)Unit.Bottle, 28000m },
                    { additionalReliefItemIds[26], "Hạt nêm", (int)Unit.Bag, 22000m },
                    { additionalReliefItemIds[27], "Chanh", (int)Unit.kg, 12000m },
                    { additionalReliefItemIds[28], "Ớt", (int)Unit.kg, 10000m },
                    { additionalReliefItemIds[29], "Mì Ý", (int)Unit.Pack, 27000m }
                });

            // Insert ReliefPackages
            migrationBuilder.InsertData(
                table: "ReliefPackages",
                columns: new[] { "Id", "CauseId", "Name", "Description", "ExtraFee", "TotalPrice", "CurrentQuantity", "TargetQuantity" },
                values: new object[,]
                {
                    { reliefPackageWaterId, causeWaterId, "Gói Nước Sạch", "Gói hỗ trợ nước sạch cho hộ gia đình", 0m, 300000m, 0, 100 },
                    { reliefPackageFoodId, causeFoodId, "Gói Thực Phẩm", "Gói thực phẩm dinh dưỡng cho trẻ em", 5000m, 500000m, 0, 200 },
                    { reliefPackageMedicineId, causeMedicineId, "Gói Y Tế", "Gói thuốc và vật tư y tế cơ bản", 10000m, 600000m, 0, 150 }
                });

            // Insert ReliefPackageItems
            migrationBuilder.InsertData(
                table: "ReliefPackageItems",
                columns: new[] { "Id", "ReliefPackageId", "ReliefItemId", "Quantity" },
                values: new object[,]
                {
                    { Guid.NewGuid(), reliefPackageWaterId, reliefItemWaterId, 2.0 },
                    { Guid.NewGuid(), reliefPackageFoodId, reliefItemFoodId, 5.0 },
                    { Guid.NewGuid(), reliefPackageMedicineId, reliefItemMedicineId, 3.0 }
                });

            // Insert Donations
            migrationBuilder.InsertData(
                table: "Donations",
                columns: new[] { "Id", "UserId", "CauseId", "DonationAmount", "Amount", "DonationDate", "PaymentMethod", "TransactionId", "TradingCode", "Status", "IsAnonymous" },
                values: new object[,]
                {
                    { donation1Id, userCustomer1Id, causeWaterId, 0m, 1000000m, DateTime.UtcNow.AddDays(-5), (int)PaymentMethod.VNPay, "TXN12345", "CODE123", 1, false },
                    { donation2Id, userCustomer2Id, causeFoodId, 0m, 500000m, DateTime.UtcNow.AddDays(-3), (int)PaymentMethod.VietQR, "TXN67890", "CODE456", 1, false },
                    { donation3Id, userCustomer1Id, causeEducationId, 0m, 2000000m, DateTime.UtcNow.AddDays(-2), (int)PaymentMethod.VNPay, "TXN54321", "CODE789", 1, false }
                });

            // Insert DonationReliefPackages
            migrationBuilder.InsertData(
                table: "DonationReliefPackages",
                columns: new[] { "Id", "DonationId", "ReliefPackageId", "Quantity" },
                values: new object[,]
                {
                    { Guid.NewGuid(), donation1Id, reliefPackageWaterId, 1 },
                    { Guid.NewGuid(), donation2Id, reliefPackageFoodId, 2 },
                    { Guid.NewGuid(), donation3Id, reliefPackageMedicineId, 1 }
                });

            // Insert Volunteers
            migrationBuilder.InsertData(
                table: "Volunteers",
                columns: new[] { "Id", "UserId", "CauseId", "JoinDate", "Status" },
                values: new object[,]
                {
                    { volunteer1Id, userVolunteerId, causeWaterId, DateTime.UtcNow.AddDays(-10), (int)VolunteerStatus.Approved },
                    { volunteer2Id, userCustomer2Id, causeEducationId, DateTime.UtcNow.AddDays(-5), (int)VolunteerStatus.Pending }
                });

            // Insert Feedbacks
            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "Id", "UserId", "CauseId", "Rating", "Comment", "CreatedAt" },
                values: new object[,]
                {
                    { feedback1Id, userCustomer1Id, causeWaterId, 5, "Chương trình rất ý nghĩa, mang lại nguồn nước sạch cho trẻ em.", DateTime.UtcNow.AddDays(-4) },
                    { feedback2Id, userCustomer2Id, causeFoodId, 4, "Gói thực phẩm giúp ích nhiều cho các gia đình nghèo.", DateTime.UtcNow.AddDays(-2) }
                });

            // Insert Sponsors
            migrationBuilder.InsertData(
                table: "Sponsors",
                columns: new[] { "Id", "Name", "LogoUrl", "Description", "WebsiteUrl", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { sponsor1Id, "Hope Corp", "/images/sponsors/hope_corp.jpg", "Tập đoàn hỗ trợ các chương trình từ thiện", "https://hopecorp.com", DateTime.UtcNow, null },
                    { sponsor2Id, "Charity Fund", "/images/sponsors/charity_fund.jpg", "Quỹ từ thiện hỗ trợ giáo dục và y tế", "https://charityfund.org", DateTime.UtcNow, null }
                });

            // Insert Notifications
            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "UserId", "Title", "Message", "IsRead", "SentAt" },
                values: new object[,]
                {
                    { notification1Id, userCustomer1Id, "Cảm ơn bạn đã quyên góp", "Quyên góp của bạn cho chiến dịch Nước Sạch đã được ghi nhận!", false, DateTime.UtcNow.AddDays(-5) },
                    { notification2Id, userVolunteerId, "Đăng ký tình nguyện", "Đăng ký tham gia tình nguyện của bạn đang được xem xét.", false, DateTime.UtcNow.AddDays(-3) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No need to implement Down as per requirement
        }
    }
}
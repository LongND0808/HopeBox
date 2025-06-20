using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using static HopeBox.Common.Enum.Enumerate;

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
            var mediaIds = new[]
            {
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                Guid.NewGuid(), Guid.NewGuid()
            };

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
                    }
                });

            // Insert Additional Manager Users
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
                        (int)CauseType.Water, new DateTime(2025, 6, 1), new DateTime(2025, 12, 1), 10000000m, 2000000m,
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
                        (int)CauseType.Medicine, new DateTime(2025, 8, 1), new DateTime(2025, 12, 31), 12000000m, 3000000m,
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
                    "TargetAmount", "CurrentAmount", "Status", "CreatedBy", "OrganizationId"
                },
                values: new object[,]
                {
                    {
                        eventCharityId,
                        "Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội",
                        "Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội là chương trình thiện nguyện đầy ý nghĩa, nhằm mang đến niềm vui và sự động viên thiết thực cho các em nhỏ có hoàn cảnh khó khăn tại các vùng ngoại thành Hà Nội. Sự kiện không chỉ là dịp các em nhận được sách vở, quà tặng và bữa ăn dinh dưỡng mà còn là nơi các em giao lưu, tham gia các hoạt động văn nghệ, trò chơi tập thể cùng hơn 200 tình nguyện viên và các nhà hảo tâm.",
                        "Sự kiện Ngày hội Trao Quà Cho Trẻ Em Nghèo Hà Nội sẽ diễn ra vào ngày 12/7/2025 tại Trường Tiểu học A, Hoài Đức, Hà Nội, dự kiến chào đón hơn 500 em nhỏ thuộc các hộ gia đình khó khăn trên địa bàn. Tại đây, mỗi em sẽ được nhận một phần quà bao gồm sách vở, đồ dùng học tập, sữa, bánh kẹo và một suất ăn trưa dinh dưỡng. Ngoài hoạt động trao quà, chương trình còn tổ chức các tiết mục văn nghệ do chính các em và tình nguyện viên biểu diễn, các trò chơi vận động, đố vui nhận thưởng, giúp các em giao lưu, tự tin hơn và có thêm nhiều kỷ niệm đẹp. Ban tổ chức bố trí đội ngũ y tế hỗ trợ, đảm bảo an toàn và sức khỏe cho tất cả người tham dự. Sự kiện cũng tạo cơ hội cho cộng đồng cùng chung tay chia sẻ, lan tỏa yêu thương và tiếp thêm động lực để các em nhỏ vượt qua khó khăn, vươn lên trong học tập cũng như cuộc sống.",
                        "https://images.baodantoc.vn/uploads/2021/Th%C3%A1ng_12/Ng%C3%A0y%203/TRUNG/T%E1%BA%B7ng%20qu%C3%A0/A1%20-%20OK.jpg",
                        new DateTime(2025, 7, 12, 8, 0, 0),
                        new DateTime(2025, 7, 12, 16, 0, 0),
                        "Trường Tiểu học A, Thị trấn Trạm Trôi, Huyện Hoài Đức, Hà Nội",
                        21.0285,
                        105.8542,
                        "Hoài Đức, Hà Nội, Việt Nam",
                        50000000m,
                        0m,
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1
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
                        0m,
                        0m,
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1
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
                        80000000m,
                        0m,
                        (int)EventStatus.Ongoing,
                        userManagerId2,
                        orgId1
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
                        60000000m,
                        0m,
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1
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
                        100000000m,
                        0m,
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1
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
                        70000000m,
                        0m,
                        (int)EventStatus.Ongoing,
                        userManagerId1,
                        orgId1
                    }
                });

            // Insert Blogs
            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[]
                {
                    "Id", "Title", "Content", "Description", "ImageUrl", "IsPublished", "CreatedAt", "CreatedBy", "UpdatedAt"
                },
                values: new object[,]
                {
                    { blogIds[0], "Giúp Trẻ Em Vùng Cao Có Nước Sạch", "Nội dung chi tiết về việc xây dựng giếng khoan và lọc nước sạch cho các bản làng vùng cao...", "Tổng quan về dự án nước sạch cho trẻ em vùng cao.", "/images/blog/water.jpg", true, DateTime.UtcNow.AddDays(-10), userAdminId, null },
                    { blogIds[1], "Chương Trình Hỗ Trợ Dinh Dưỡng", "Chi tiết chương trình cung cấp thực phẩm dinh dưỡng cho trẻ em suy dinh dưỡng vùng cao...", "Giới thiệu chương trình hỗ trợ dinh dưỡng.", "/images/blog/food.jpg", false, DateTime.UtcNow.AddDays(-8), userAdminId, null },
                    { blogIds[2], "Xây Dựng Lớp Học Vùng Núi", "Thông tin về dự án xây trường học mới cho trẻ em ở vùng sâu vùng xa...", "Cải thiện điều kiện học tập cho trẻ em vùng núi.", "/images/blog/education.jpg", true, DateTime.UtcNow.AddDays(-15), userAdminId, null },
                    { blogIds[3], "Tặng Quần Áo Ấm Cho Trẻ Em", "Chiến dịch quyên góp và phân phát áo ấm cho trẻ em vùng lạnh...", "Mang đến hơi ấm mùa đông cho trẻ em vùng cao.", "/images/blog/clothing.jpg", true, DateTime.UtcNow.AddDays(-12), userAdminId, null },
                    { blogIds[4], "Cung Cấp Sách Giáo Khoa", "Hỗ trợ sách vở và dụng cụ học tập cho học sinh nghèo...", "Nâng cao chất lượng giáo dục thông qua hỗ trợ vật chất.", "/images/blog/books.jpg", true, DateTime.UtcNow.AddDays(-10), userAdminId, null },
                    { blogIds[5], "Khám Sức Khỏe Định Kỳ", "Chương trình khám sức khỏe miễn phí cho trẻ em...", "Quan tâm sức khỏe định kỳ cho trẻ em vùng xa.", "/images/blog/health.jpg", false, DateTime.UtcNow.AddDays(-7), userAdminId, null },
                    { blogIds[6], "Trại Hè Tình Nguyện", "Tổ chức trại hè bổ ích và vui chơi cho trẻ em khó khăn...", "Mang đến nụ cười và niềm vui cho trẻ em mùa hè.", "/images/blog/camp.jpg", true, DateTime.UtcNow.AddDays(-20), userAdminId, null },
                    { blogIds[7], "Hướng Nghiệp Cho Học Sinh", "Chia sẻ kiến thức nghề nghiệp và định hướng tương lai...", "Đồng hành cùng các em trong hành trình tương lai.", "/images/blog/career.jpg", false, DateTime.UtcNow.AddDays(-5), userAdminId, null },
                    { blogIds[8], "Học Bổng Cho Học Sinh Nghèo", "Chương trình học bổng dành cho học sinh có hoàn cảnh khó khăn...", "Khuyến khích học sinh tiếp tục học tập.", "/images/blog/scholarship.jpg", true, DateTime.UtcNow.AddDays(-25), userAdminId, null },
                    { blogIds[9], "Ngày Hội Đọc Sách", "Khơi dậy niềm yêu thích đọc sách trong trẻ em...", "Góp phần xây dựng văn hóa đọc cho thế hệ tương lai.", "/images/blog/reading.jpg", true, DateTime.UtcNow.AddDays(-7), userAdminId, null },
                    { blogIds[10], "Trồng Cây Xanh Ở Trường Học", "Chiến dịch trồng cây xanh tại các điểm trường vùng cao...", "Cải thiện môi trường sống và học tập cho trẻ em.", "/images/blog/trees.jpg", true, DateTime.UtcNow.AddDays(-4), userAdminId, null },
                    { blogIds[11], "Khóa Học Kỹ Năng Sống", "Giáo dục kỹ năng sống giúp học sinh tự tin và giao tiếp...", "Giáo dục toàn diện cho học sinh.", "/images/blog/skills.jpg", true, DateTime.UtcNow.AddDays(-3), userAdminId, null },
                    { blogIds[12], "Hỗ Trợ Nhà Ở Vùng Cao", "Chi tiết về chương trình xây dựng nhà ở an toàn cho các gia đình nghèo...", "Mang lại mái ấm bền vững cho cộng đồng.", "/images/blog/shelter.jpg", true, DateTime.UtcNow.AddDays(-2), userAdminId, null },
                    { blogIds[13], "Chương Trình Y Tế Cộng Đồng", "Tổ chức khám sức khỏe và tư vấn y tế miễn phí...", "Cải thiện sức khỏe cộng đồng vùng sâu vùng xa.", "/images/blog/healthcare.jpg", false, DateTime.UtcNow.AddDays(-6), userAdminId, null },
                    { blogIds[14], "Ngày Hội Môi Trường", "Sự kiện tuyên truyền bảo vệ môi trường và trồng cây xanh...", "Lan tỏa ý thức bảo vệ môi trường trong cộng đồng.", "/images/blog/environment.jpg", true, DateTime.UtcNow.AddDays(-1), userAdminId, null }
                });

            // Insert ReliefItems
            migrationBuilder.InsertData(
                table: "ReliefItems",
                columns: new[] { "Id", "ItemName", "Unit", "UnitPrice" },
                values: new object[,]
                {
                    { reliefItemWaterId, "Bình lọc nước", (int) Unit.Bottle, 150000m },
                    { reliefItemFoodId, "Gói thực phẩm", (int) Unit.Pack, 100000m },
                    { reliefItemMedicineId, "Hộp thuốc y tế", (int) Unit.Box, 200000m },
                    { reliefItemBookId, "Bộ sách giáo khoa", (int) Unit.Pack, 50000m },
                    { reliefItemClothingId, "Bộ quần áo ấm", (int) Unit.Pack, 80000m }
                });

            // Insert ReliefPackages
            migrationBuilder.InsertData(
                table: "ReliefPackages",
                columns: new[] { "Id", "CauseId", "Name", "Description" },
                values: new object[,]
                {
                    { reliefPackageWaterId, causeWaterId, "Gói Nước Sạch", "Gói hỗ trợ nước sạch cho hộ gia đình" },
                    { reliefPackageFoodId, causeFoodId, "Gói Thực Phẩm", "Gói thực phẩm dinh dưỡng cho trẻ em" },
                    { reliefPackageMedicineId, causeMedicineId, "Gói Y Tế", "Gói thuốc và vật tư y tế cơ bản" }
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
                columns: new[] { "Id", "UserId", "CauseId", "Amount", "DonationDate", "PaymentMethod", "TransactionId", "TradingCode", "Status" },
                values: new object[,]
                {
                    { donation1Id, userCustomer1Id, causeWaterId, 1000000m, DateTime.UtcNow.AddDays(-5), (int)PaymentMethod.VNPay, "TXN12345", "CODE123", 1 },
                    { donation2Id, userCustomer2Id, causeFoodId, 500000m, DateTime.UtcNow.AddDays(-3), (int)PaymentMethod.VietQR, "TXN67890", "CODE456", 1 },
                    { donation3Id, userCustomer1Id, causeEducationId, 2000000m, DateTime.UtcNow.AddDays(-2), (int)PaymentMethod.VNPay, "TXN54321", "CODE789", 1 }
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

            // Insert Testimonials
            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "Name", "Designation", "Description", "ImageUrl", "CreatedAt" },
                values: new object[,]
                {
                    { 1, "Nguyễn Văn A", "Tình nguyện viên", "Tham gia chương trình nước sạch là trải nghiệm đáng nhớ, mang lại giá trị lớn cho cộng đồng.", "/images/testimonials/person1.jpg", DateTime.UtcNow.AddDays(-10) },
                    { 2, "Trần Thị B", "Nhà tài trợ", "Tôi rất tự hào khi góp phần mang giáo dục đến trẻ em vùng cao.", "/images/testimonials/person2.jpg", DateTime.UtcNow.AddDays(-8) },
                    { 3, "Lê Văn C", "Người thụ hưởng", "Nhờ chương trình, gia đình tôi có được ngôi nhà mới an toàn.", "/images/testimonials/person3.jpg", DateTime.UtcNow.AddDays(-6) }
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

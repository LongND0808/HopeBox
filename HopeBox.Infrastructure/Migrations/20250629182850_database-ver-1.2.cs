using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopeBox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class databasever12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organizations_OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_CreatedBy",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Causes_AspNetUsers_CreatedBy",
                table: "Causes");

            migrationBuilder.DropForeignKey(
                name: "FK_Causes_Organizations_OrganizationId",
                table: "Causes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmEmails_AspNetUsers_UserId",
                table: "ConfirmEmails");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReliefPackages_Donations_DonationId",
                table: "DonationReliefPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReliefPackages_ReliefPackages_ReliefPackageId",
                table: "DonationReliefPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Causes_CauseId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_CreatedBy",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organizations_OrganizationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Causes_CauseId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Causes_CauseId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Events_EventId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ReliefPackageItems_ReliefItems_ReliefItemId",
                table: "ReliefPackageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReliefPackageItems_ReliefPackages_ReliefPackageId",
                table: "ReliefPackageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReliefPackages_Causes_CauseId",
                table: "ReliefPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_AspNetUsers_UserId",
                table: "Volunteers");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Causes_CauseId",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "CurrentAmount",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TargetAmount",
                table: "Events");

            migrationBuilder.AddColumn<Guid>(
                name: "CauseId",
                table: "Events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "CommentCount",
                table: "Blogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Blogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Blogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaKeywords",
                table: "Blogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReadingTime",
                table: "Blogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShareCount",
                table: "Blogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Blogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Blogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Blogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    LikeCount = table.Column<int>(type: "integer", nullable: false),
                    ReplyCount = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InkindDonations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    InKind = table.Column<string>(type: "text", nullable: false),
                    DonationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SenderAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ReceiverAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InkindDonations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InkindDonations_AspNetUsers_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InkindDonations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InkindDonations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    Platform = table.Column<int>(type: "integer", nullable: false),
                    ShareUrl = table.Column<string>(type: "text", nullable: true),
                    ShareCaption = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shares_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shares_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetType = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    BlogId = table.Column<Guid>(type: "uuid", nullable: true),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CauseId",
                table: "Events",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InkindDonations_ApprovedBy",
                table: "InkindDonations",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InkindDonations_EventId",
                table: "InkindDonations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_InkindDonations_UserId",
                table: "InkindDonations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_BlogId",
                table: "Likes",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CommentId",
                table: "Likes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_BlogId",
                table: "Shares",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_UserId",
                table: "Shares",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organizations_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_CreatedBy",
                table: "Blogs",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Causes_AspNetUsers_CreatedBy",
                table: "Causes",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Causes_Organizations_OrganizationId",
                table: "Causes",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmEmails_AspNetUsers_UserId",
                table: "ConfirmEmails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReliefPackages_Donations_DonationId",
                table: "DonationReliefPackages",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReliefPackages_ReliefPackages_ReliefPackageId",
                table: "DonationReliefPackages",
                column: "ReliefPackageId",
                principalTable: "ReliefPackages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Causes_CauseId",
                table: "Donations",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_CreatedBy",
                table: "Events",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Causes_CauseId",
                table: "Events",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organizations_OrganizationId",
                table: "Events",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Causes_CauseId",
                table: "Feedbacks",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Causes_CauseId",
                table: "Medias",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Events_EventId",
                table: "Medias",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReliefPackageItems_ReliefItems_ReliefItemId",
                table: "ReliefPackageItems",
                column: "ReliefItemId",
                principalTable: "ReliefItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReliefPackageItems_ReliefPackages_ReliefPackageId",
                table: "ReliefPackageItems",
                column: "ReliefPackageId",
                principalTable: "ReliefPackages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReliefPackages_Causes_CauseId",
                table: "ReliefPackages",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_AspNetUsers_UserId",
                table: "Volunteers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_Causes_CauseId",
                table: "Volunteers",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organizations_OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_CreatedBy",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Causes_AspNetUsers_CreatedBy",
                table: "Causes");

            migrationBuilder.DropForeignKey(
                name: "FK_Causes_Organizations_OrganizationId",
                table: "Causes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmEmails_AspNetUsers_UserId",
                table: "ConfirmEmails");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReliefPackages_Donations_DonationId",
                table: "DonationReliefPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_DonationReliefPackages_ReliefPackages_ReliefPackageId",
                table: "DonationReliefPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Causes_CauseId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_CreatedBy",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Causes_CauseId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organizations_OrganizationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Causes_CauseId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Causes_CauseId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Events_EventId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ReliefPackageItems_ReliefItems_ReliefItemId",
                table: "ReliefPackageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReliefPackageItems_ReliefPackages_ReliefPackageId",
                table: "ReliefPackageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReliefPackages_Causes_CauseId",
                table: "ReliefPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_AspNetUsers_UserId",
                table: "Volunteers");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Causes_CauseId",
                table: "Volunteers");

            migrationBuilder.DropTable(
                name: "InkindDonations");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Events_CauseId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CauseId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CommentCount",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MetaKeywords",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ReadingTime",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ShareCount",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Blogs");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentAmount",
                table: "Events",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TargetAmount",
                table: "Events",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organizations_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_CreatedBy",
                table: "Blogs",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Causes_AspNetUsers_CreatedBy",
                table: "Causes",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Causes_Organizations_OrganizationId",
                table: "Causes",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmEmails_AspNetUsers_UserId",
                table: "ConfirmEmails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReliefPackages_Donations_DonationId",
                table: "DonationReliefPackages",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DonationReliefPackages_ReliefPackages_ReliefPackageId",
                table: "DonationReliefPackages",
                column: "ReliefPackageId",
                principalTable: "ReliefPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Causes_CauseId",
                table: "Donations",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_CreatedBy",
                table: "Events",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organizations_OrganizationId",
                table: "Events",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Causes_CauseId",
                table: "Feedbacks",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Causes_CauseId",
                table: "Medias",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Events_EventId",
                table: "Medias",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReliefPackageItems_ReliefItems_ReliefItemId",
                table: "ReliefPackageItems",
                column: "ReliefItemId",
                principalTable: "ReliefItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReliefPackageItems_ReliefPackages_ReliefPackageId",
                table: "ReliefPackageItems",
                column: "ReliefPackageId",
                principalTable: "ReliefPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReliefPackages_Causes_CauseId",
                table: "ReliefPackages",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_AspNetUsers_UserId",
                table: "Volunteers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_Causes_CauseId",
                table: "Volunteers",
                column: "CauseId",
                principalTable: "Causes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using HopeBox.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HopeBox.Infrastructure.DataContext
{
    public partial class HopeBoxDataContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>,
        UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IHopeBoxDataContext
    {
        public HopeBoxDataContext(DbContextOptions<HopeBoxDataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        public override DatabaseFacade Database => base.Database;

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Cause> Causes { get; set; }
        public virtual DbSet<ConfirmEmail> ConfirmEmails { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<DonationReliefPackage> DonationReliefPackages { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Media> Medias { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<ReliefItem> ReliefItems { get; set; }
        public virtual DbSet<ReliefPackage> ReliefPackages { get; set; }
        public virtual DbSet<ReliefPackageItem> ReliefPackageItems { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Sponsor> Sponsors { get; set; }
        public virtual DbSet<Testimonial> Testimonials { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<InKindDonation> InKindDonations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var foreignKey in entity.GetForeignKeys())
                {
                    bool allColumnsNullable = foreignKey.Properties.All(p => p.IsNullable);

                    foreignKey.DeleteBehavior = allColumnsNullable
                        ? DeleteBehavior.SetNull
                        : DeleteBehavior.Restrict;
                }
            }
        }

    }
}

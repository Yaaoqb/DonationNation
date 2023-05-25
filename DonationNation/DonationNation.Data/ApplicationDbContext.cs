using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using DonationNation.Data.Models;

namespace DonationNation.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                          cancellationToken));
        }


        /// <summary>
        /// Before saving, add the createdon, creadtedby, updatedon to the entity
        /// </summary>
        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;
            string currentUserName = "System Generated";

            foreach (var entry in entries)
            {
                // for entities that inherit from BaseEntity,
                // set UpdatedOn / CreatedOn appropriately
                if (entry.Entity is BaseEntity trackable)
                {
                    if (entry.Entity is not ApplicationUser)
                    {
                        currentUserName = _httpContextAccessor.HttpContext.User.Identity.Name;
                    }

                    if (string.IsNullOrEmpty(currentUserName))
                    {
                        currentUserName = "Anonymous";
                    }
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // set the updated date to "now"
                            trackable.UpdatedOn = utcNow;

                            // mark property as "don't touch"
                            // we don't want to update on a Modify operation
                            entry.Property("CreatedOn").IsModified = false;

                            // set the updated by user if not passed on in entity
                            if (String.IsNullOrEmpty(trackable.UpdatedBy)) trackable.UpdatedBy = currentUserName;
                            break;

                        case EntityState.Added:
                            // set both updated and created date to "now"
                            // set createdby to CurrentUserId
                            trackable.CreatedOn = utcNow;
                            trackable.UpdatedOn = utcNow;

                            // set the created by user if not passed on in entity
                            if (String.IsNullOrEmpty(trackable.CreatedBy)) trackable.CreatedBy = currentUserName;
                            break;
                    }
                }
            }
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Badge> Badges { get; set; }

        public DbSet<Testimonial> Testimonials { get; set; }

        public DbSet<UserActivity> UserActivities { get; set; }

    }
}

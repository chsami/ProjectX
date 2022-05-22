using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebApi.Domain;
using WebApi.Domain.Contracts;
using WebApi.Domain.Entities;
using WebApi.Infrastructure.Models;

namespace WebApi.Infrastructure.Database
{
    public class ProjectDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Audit> AuditTrails { get; set; }

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Role>()
                .HasIndex(u => u.Name)
                .IsUnique();
            modelBuilder.Entity<Role>().HasData(
                new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = "User",
                    CreatedBy = "SEED",
                    CreatedOn = DateTime.UtcNow,
                },
                new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    CreatedBy = "SEED",
                    CreatedOn = DateTime.UtcNow,
                }
            );
            base.OnModelCreating(modelBuilder);
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
           // var httpContextAccessor = this.GetService<IHttpContextAccessor>();
           // var a = httpContextAccessor.HttpContext?.User;
            var auditEntries = OnBeforeSaveChanges(_currentUserService.UserId);
            var result = await base.SaveChangesAsync(cancellationToken);
            await OnAfterSaveChanges(auditEntries, cancellationToken);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                bool isAuditableEntity = typeof(IAuditableEntity).IsAssignableFrom(entry.Entity.GetType());

                if (isAuditableEntity)
                {
                    var auditableEntity = (IAuditableEntity)entry.Entity;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditableEntity.CreatedOn = DateTime.UtcNow;
                            auditableEntity.CreatedBy = userId ?? "";
                            break;

                        case EntityState.Modified:
                            auditableEntity.LastModifiedOn = DateTime.UtcNow;
                            auditableEntity.LastModifiedBy = userId ?? "";
                            break;
                    }
                }
                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId ?? ""
                };
                
                
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }

                if (auditEntry.AuditType == AuditType.None)
                {
                    auditEntries.Remove(auditEntry);
                }
            }
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                AuditTrails.Add(auditEntry.ToAudit());
            }
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries, CancellationToken cancellationToken = new())
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                AuditTrails.Add(auditEntry.ToAudit());
            }
            return SaveChangesAsync(cancellationToken);
        }
    }
}

using ExaminationSystem.Domin.Entities;
using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    /// <summary>
    /// EF Core configuration for the Role entity.
    /// Defines table structure, constraints, relationships, and seed data.
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.IsAvailable)
                .HasDefaultValue(true);

            // Unique constraint on role name to prevent duplicate roles
            builder.HasIndex(r => r.Name)
                .IsUnique()
                .HasDatabaseName("IX_Roles_Name_Unique");

            // Configure relationship to UserRoles with cascade delete prevention
            builder.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed default roles
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new Role
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000001"),
                    Name = "Student",
                    IsAvailable = true,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Role
                {
                    Id = new Guid("10000000-0000-0000-0000-000000000002"),
                    Name = "Admin",
                    IsAvailable = true,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );
        }
    }

    /// <summary>
    /// EF Core configuration for the UserRole entity (junction table).
    /// Defines the many-to-many relationship between User and Role.
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            // Composite primary key to prevent duplicate user-role assignments
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            // Foreign key to User with cascade delete
            builder.HasOne(ur => ur.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to Role with restrict delete (to preserve role data)
            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for efficient lookups
            builder.HasIndex(ur => ur.UserId)
                .HasDatabaseName("IX_UserRoles_UserId");

            builder.HasIndex(ur => ur.RoleId)
                .HasDatabaseName("IX_UserRoles_RoleId");
        }
    }
}
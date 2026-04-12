using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(ur => ur.Id);

            builder.Property(ur => ur.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(ur => ur.Description)
                .HasMaxLength(200);

            // Unique constraint on role name
            builder.HasIndex(ur => ur.Name)
                .IsUnique()
                .HasDatabaseName("IX_UserRoles_Name_Unique");

            // Seed default roles
            builder.HasData(
                new UserRole 
                { 
                    Id = new Guid("10000000-0000-0000-0000-000000000001"),
                    Name = "Student",
                    Description = "Student role - can take quizzes and view their progress",
                    CreatedAt = DateTime.UtcNow
                },
                new UserRole 
                { 
                    Id = new Guid("10000000-0000-0000-0000-000000000002"),
                    Name = "Admin",
                    Description = "Administrator role - full system access",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
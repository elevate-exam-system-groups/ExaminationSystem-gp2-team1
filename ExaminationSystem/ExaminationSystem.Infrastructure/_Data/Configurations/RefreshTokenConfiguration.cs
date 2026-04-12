using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.TokenHash)
                .IsRequired();

            builder.Property(r => r.IsRevoked)
                .HasDefaultValue(false);

            builder.Property(r => r.IpAddress)
                .HasMaxLength(45);

            // Index for quick lookup
            builder.HasIndex(r => new { r.UserId, r.IsRevoked })
                .HasDatabaseName("IX_RefreshTokens_UserId_IsRevoked");

            builder.HasIndex(r => r.ExpiresAt)
                .HasDatabaseName("IX_RefreshTokens_ExpiresAt");
        }
    }
}
using ExaminationSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure._Data.Configurations
{
    public class OtpRecordConfiguration : IEntityTypeConfiguration<OtpRecord>
    {
        public void Configure(EntityTypeBuilder<OtpRecord> builder)
        {
            builder.ToTable("OtpRecords");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.OtpHash)
                .IsRequired();

            builder.Property(o => o.IsUsed)
                .HasDefaultValue(false);

            builder.Property(o => o.AttemptCount)
                .HasDefaultValue(0);

            builder.Property(o => o.IsLocked)
                .HasDefaultValue(false);

            // Index for quick lookup
            builder.HasIndex(o => new { o.UserId, o.IsUsed })
                .HasDatabaseName("IX_OtpRecords_UserId_IsUsed");

            builder.HasIndex(o => o.ExpiresAt)
                .HasDatabaseName("IX_OtpRecords_ExpiresAt");
        }
    }
}
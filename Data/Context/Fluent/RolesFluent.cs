using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Auth;
using Shared.Entities;

namespace Data.Context.Fluent
{
    public class RolesFluent : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.HasOne<UserEntity>()
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            builder.HasOne<DbRole>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }
    }
}

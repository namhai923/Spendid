using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spendid.Domain.Users;

namespace Spendid.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.UserName)
            .HasMaxLength(200)
            .HasConversion(
                userName => userName.Value,
                value => new UserName(value)
            );

        builder.Property(u => u.IdentityId)
            .IsRequired()
            .HasMaxLength(255)
            .HasConversion(
                identityId => identityId.Value,
                value => new IdentityId(value)
            );

        builder.HasIndex(u => u.IdentityId).IsUnique();

        builder.Property(user => user.Email)
            .HasMaxLength(400)
            .HasConversion(
                email => email.Value,
                value => new Domain.Users.Email(value)
            );

        builder.HasIndex(user => user.Email).IsUnique();
    }
}

using Profile_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserID);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(255);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(255);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Country).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Gender).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Language).IsRequired().HasMaxLength(255);
        builder.Property(u => u.ProfilePicture).HasMaxLength(255);

        // unique attributes accross the entire app
        builder.HasIndex(u => u.UserID).IsUnique();
        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
    }

}
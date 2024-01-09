using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Database.Configuration;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.Property("Name").HasMaxLength(50);
        builder.Property("Surname").HasMaxLength(50);

        builder.HasData(new
            {
                Id = 1,
                Name = "Eric",
                Surname = "Clapton",
                BirthYear = 1945
            },
            new
            {
                Id = 2,
                Name = "Michael",
                Surname = "Jackson",
                BirthYear = 1958
            },
            new
            {
                Id = 3,
                Name = "David",
                Surname = "Gilmour",
                BirthYear = 1946
            }
        );
    }
}
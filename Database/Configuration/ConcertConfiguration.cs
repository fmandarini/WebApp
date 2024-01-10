using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using DateTime = System.DateTime;

namespace Database.Configuration;

public class ConcertConfiguration : IEntityTypeConfiguration<Concert>
{
    public void Configure(EntityTypeBuilder<Concert> builder)
    {
        builder.Property("Location").HasMaxLength(50);

        builder.HasData(
            new
            {
                Id = 1,
                Date = new DateTime(2021, 10, 10),
                Location = "Copenhagen",
                ArtistId = 1
            },
            new
            {
                Id = 2,
                Date = new DateTime(2021, 06, 11),
                Location = "London",
                ArtistId = 2
            },
            new
            {
                Id = 3,
                Date = new DateTime(2021, 07, 12),
                Location = "Rome",
                ArtistId = 3
            }
        );
    }
}
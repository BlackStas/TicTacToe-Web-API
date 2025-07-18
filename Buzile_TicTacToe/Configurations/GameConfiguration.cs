using Buzile_TicTacToe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Buzile_TicTacToe.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("games");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Player1Id).HasColumnName("player_1");
        builder.Property(x => x.Player2Id).HasColumnName("player_2");
        builder.Property(x => x.IsEnd).HasColumnName("is_end");

        builder.HasOne(x => x.Player1).WithMany().HasForeignKey(x => x.Player1Id);
        builder.HasOne(x => x.Player2).WithMany().HasForeignKey(x => x.Player2Id);
        builder.HasMany(x=>x.Steps).WithOne().HasForeignKey(x=>x.GameId);
    }
}
using Buzile_TicTacToe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Buzile_TicTacToe.Configurations;

public class GameStepConfiguration : IEntityTypeConfiguration<GameStep>
{
    public void Configure(EntityTypeBuilder<GameStep> builder)
    {
        builder.ToTable("game_steps");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.GameId).HasColumnName("game_id");
        builder.Property(x => x.PlayerId).HasColumnName("player_id");
        builder.Property(x => x.Column).HasColumnName("column");
        builder.Property(x => x.Row).HasColumnName("row");
        builder.Property(x => x.IsCross).HasColumnName("is_cross");
        builder.Property(x => x.StepTime).HasColumnName("step_time");
    }
}
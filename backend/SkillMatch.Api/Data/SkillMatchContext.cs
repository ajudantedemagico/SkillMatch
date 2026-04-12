using Microsoft.EntityFrameworkCore;
using SkillMatch.Api.Models;

namespace SkillMatch.Api.Data;

public class SkillMatchContext : DbContext
{
    public SkillMatchContext(DbContextOptions<SkillMatchContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario configuration
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(255);
            entity.Property(e => e.SenhaHash).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasMany(e => e.PasswordResetTokens)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PasswordResetToken configuration
        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VerificationCode).IsRequired();
            entity.Property(e => e.UsuarioId).IsRequired();
        });
    }
}

using Microsoft.EntityFrameworkCore;
using SkillMatch.Api.Models;

namespace SkillMatch.Api.Data;

public class SkillMatchContext : DbContext
{
    public SkillMatchContext(DbContextOptions<SkillMatchContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; } = null!;
    public DbSet<Formacao> Formacoes { get; set; } = null!;
    public DbSet<Experiencia> Experiencias { get; set; } = null!;
    public DbSet<Curriculo> Curriculos { get; set; } = null!;

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
            entity.HasMany(e => e.Formacoes)
                .WithOne(f => f.Usuario)
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Experiencias)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Curriculos)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PasswordResetToken configuration
        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VerificationCode).IsRequired();
            entity.Property(e => e.UsuarioId).IsRequired();
        });

        // Formacao configuration
        modelBuilder.Entity<Formacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Instituicao).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Curso).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Tipo).IsRequired().HasMaxLength(100);
        });

        // Experiencia configuration
        modelBuilder.Entity<Experiencia>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Empresa).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Cargo).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Atividades).HasMaxLength(2000);
            entity.Property(e => e.Tecnologias).HasMaxLength(1000);
            entity.Property(e => e.Resultados).HasMaxLength(2000);
        });

        // Curriculo configuration
        modelBuilder.Entity<Curriculo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DescricaoVaga).HasMaxLength(5000);
            entity.Property(e => e.SecoesJson).IsRequired(); // Store full CV as JSON
        });
    }
}


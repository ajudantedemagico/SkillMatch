namespace SkillMatch.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    public bool IsAtivo { get; set; } = true;

    // Relationships
    public List<PasswordResetToken> PasswordResetTokens { get; set; } = [];
}

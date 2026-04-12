namespace SkillMatch.Api.Models;

public class PasswordResetToken
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    public string VerificationCode { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

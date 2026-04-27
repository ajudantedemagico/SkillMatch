namespace SkillMatch.Api.Models;

public class Usuario
{
    // Auth
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    public bool IsAtivo { get; set; } = true;

    // Profile (Step 1)
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string LinkedIn { get; set; } = string.Empty;
    public string Portfolio { get; set; } = string.Empty;

    // Step 4: Technical Skills (JSON array)
    public string CompetenciasTecnicas { get; set; } = "[]"; // JSON array

    // Step 5: Professional Objective
    public string ObjetivosProfissionais { get; set; } = string.Empty;

    // Step 6: Soft Skills (JSON array)
    public string SoftSkills { get; set; } = "[]"; // JSON array

    // Relationships
    public List<PasswordResetToken> PasswordResetTokens { get; set; } = [];
    public List<Formacao> Formacoes { get; set; } = [];
    public List<Experiencia> Experiencias { get; set; } = [];
    public List<Curriculo> Curriculos { get; set; } = [];
}

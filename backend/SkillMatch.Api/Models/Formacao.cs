namespace SkillMatch.Api.Models;

public class Formacao
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public string Instituicao { get; set; } = string.Empty;
    public string Curso { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // "Ensino Médio", "Tecnólogo", "Graduação", "Mestrado", etc
    public DateTime DataInicio { get; set; }
    public DateTime? DataConclusao { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}

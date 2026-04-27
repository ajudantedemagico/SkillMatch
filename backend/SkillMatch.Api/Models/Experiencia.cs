namespace SkillMatch.Api.Models;

public class Experiencia
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public string Empresa { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public bool EmpregoAtual { get; set; } = false;
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    
    public string Atividades { get; set; } = string.Empty; // Descrição das atividades
    public string Tecnologias { get; set; } = string.Empty; // JSON array ou texto
    public string Resultados { get; set; } = string.Empty; // Resultados alcançados
    
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}

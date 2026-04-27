namespace SkillMatch.Api.Models;

public class Curriculo
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string DescricaoVaga { get; set; } = string.Empty; // Job description used for generation
    
    // CV Sections as JSON
    public string SecoesJson { get; set; } = string.Empty; // Complete CV as JSON with all sections
    
    // Edit tracking (RN-08)
    public bool CabecalhoEditado { get; set; } = false;
    public bool ResumoBioEditado { get; set; } = false;
    public bool ExperienciaEditada { get; set; } = false;
    public bool CompetenciasEditadas { get; set; } = false;
    public bool FormacaoEditada { get; set; } = false;

    public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    public int VersaoPerfilAncora { get; set; } = 1; // Version of profile used at generation time
}

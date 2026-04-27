using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SkillMatch.Api.Data;
using SkillMatch.Api.Dtos;
using SkillMatch.Api.Models;

namespace SkillMatch.Api.Services;

public interface ICurriculoService
{
    Task<GerarCurriculoResponseDto> GerarCurriculoAsync(int usuarioId, GerarCurriculoRequestDto dto);
    Task<CurriculoDetalheDto?> SalvarCurriculoAsync(int usuarioId, SalvarCurriculoRequestDto dto);
    Task<List<CurriculoListaDto>> ListarCurriculosAsync(int usuarioId);
    Task<CurriculoDetalheDto?> GetCurriculoAsync(int usuarioId, int curriculoId);
}

public class CurriculoService : ICurriculoService
{
    private readonly SkillMatchContext _context;
    private readonly IPerfilService _perfilService;

    public CurriculoService(SkillMatchContext context, IPerfilService perfilService)
    {
        _context = context;
        _perfilService = perfilService;
    }

    public async Task<GerarCurriculoResponseDto> GerarCurriculoAsync(int usuarioId, GerarCurriculoRequestDto dto)
    {
        if (!dto.ConsentimentoIA)
            throw new InvalidOperationException("Consentimento para IA é obrigatório");

        // Get user profile
        var perfil = await _perfilService.GetPerfilAsync(usuarioId);
        if (perfil == null)
            throw new InvalidOperationException("Perfil do usuário não encontrado");

        // TODO: Call AI service to generate optimized CV content
        // For now, we'll return a basic structure based on the profile

        var secoes = new CurriculoSecoesDto
        {
            Cabecalho = new CabecalhoDto
            {
                Nome = perfil.Nome,
                Email = perfil.Email,
                Telefone = perfil.Telefone,
                LinkedIn = perfil.LinkedIn,
                Portfolio = perfil.Portfolio,
                Localizacao = $"{perfil.Cidade}, {perfil.Estado}"
            },
            ResumoBio = new ResumoBioDto
            {
                Conteudo = perfil.ObjetivosProfissionais
            },
            Experiencias = perfil.Experiencias.Select(e => new ExperienciaGeradaDto
            {
                Empresa = e.Empresa,
                Cargo = e.Cargo,
                DataInicio = e.DataInicio,
                DataFim = e.DataFim,
                Descricao = e.Atividades,
                Tecnologias = e.Tecnologias
            }).ToList(),
            Competencias = new CompetenciasDto
            {
                Tecnicas = perfil.CompetenciasTecnicas,
                Comportamentais = perfil.SoftSkills
            },
            Formacoes = perfil.Formacoes.Select(f => new FormacaoGeradaDto
            {
                Instituicao = f.Instituicao,
                Curso = f.Curso,
                Tipo = f.Tipo,
                DataInicio = f.DataInicio,
                DataConclusao = f.DataConclusao
            }).ToList()
        };

        // TODO: Update cabecalho.Titulo based on job description + AI

        return new GerarCurriculoResponseDto
        {
            Titulo = $"CV - {perfil.Nome}",
            Secoes = secoes
        };
    }

    public async Task<CurriculoDetalheDto?> SalvarCurriculoAsync(int usuarioId, SalvarCurriculoRequestDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
        if (usuario == null)
            return null;

        var curriculo = new Curriculo
        {
            UsuarioId = usuarioId,
            Titulo = dto.Titulo,
            DescricaoVaga = dto.DescricaoVaga,
            SecoesJson = JsonSerializer.Serialize(dto.Secoes),
            CabecalhoEditado = dto.CabecalhoEditado,
            ResumoBioEditado = dto.ResumoBioEditado,
            ExperienciaEditada = dto.ExperienciaEditada,
            CompetenciasEditadas = dto.CompetenciasEditadas,
            FormacaoEditada = dto.FormacaoEditada,
            DataGeracao = DateTime.UtcNow
        };

        _context.Curriculos.Add(curriculo);
        await _context.SaveChangesAsync();

        return MapToDetalheDto(curriculo, dto.Secoes);
    }

    public async Task<List<CurriculoListaDto>> ListarCurriculosAsync(int usuarioId)
    {
        var curriculos = await _context.Curriculos
            .Where(c => c.UsuarioId == usuarioId)
            .OrderByDescending(c => c.DataGeracao)
            .ToListAsync();

        return curriculos.Select(c => new CurriculoListaDto
        {
            Id = c.Id,
            Titulo = c.Titulo,
            DataGeracao = c.DataGeracao,
            DescricaoVaga = TruncateText(c.DescricaoVaga, 100),
            FoiEditado = c.CabecalhoEditado || c.ResumoBioEditado || c.ExperienciaEditada || 
                        c.CompetenciasEditadas || c.FormacaoEditada
        }).ToList();
    }

    public async Task<CurriculoDetalheDto?> GetCurriculoAsync(int usuarioId, int curriculoId)
    {
        var curriculo = await _context.Curriculos
            .FirstOrDefaultAsync(c => c.Id == curriculoId && c.UsuarioId == usuarioId);

        if (curriculo == null)
            return null;

        try
        {
            var secoes = JsonSerializer.Deserialize<CurriculoSecoesDto>(curriculo.SecoesJson) ?? new CurriculoSecoesDto();
            return MapToDetalheDto(curriculo, secoes);
        }
        catch
        {
            return null;
        }
    }

    private CurriculoDetalheDto MapToDetalheDto(Curriculo curriculo, CurriculoSecoesDto secoes)
    {
        return new CurriculoDetalheDto
        {
            Id = curriculo.Id,
            Titulo = curriculo.Titulo,
            DescricaoVaga = curriculo.DescricaoVaga,
            Secoes = secoes,
            DataGeracao = curriculo.DataGeracao,
            DataAtualizacao = curriculo.DataAtualizacao,
            CabecalhoEditado = curriculo.CabecalhoEditado,
            ResumoBioEditado = curriculo.ResumoBioEditado,
            ExperienciaEditada = curriculo.ExperienciaEditada,
            CompetenciasEditadas = curriculo.CompetenciasEditadas,
            FormacaoEditada = curriculo.FormacaoEditada
        };
    }

    private string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
    }
}

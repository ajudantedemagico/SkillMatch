using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SkillMatch.Api.Data;
using SkillMatch.Api.Dtos;
using SkillMatch.Api.Models;

namespace SkillMatch.Api.Services;

public interface IPerfilService
{
    Task<PerfilDto?> GetPerfilAsync(int usuarioId);
    Task<bool> SalvarPerfilAsync(int usuarioId, PerfilDto dto);
}

public class PerfilService : IPerfilService
{
    private readonly SkillMatchContext _context;

    public PerfilService(SkillMatchContext context)
    {
        _context = context;
    }

    public async Task<PerfilDto?> GetPerfilAsync(int usuarioId)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Formacoes)
            .Include(u => u.Experiencias)
            .FirstOrDefaultAsync(u => u.Id == usuarioId);

        if (usuario == null)
            return null;

        var dto = new PerfilDto
        {
            Nome = usuario.Nome,
            Email = usuario.Email,
            Cidade = usuario.Cidade,
            Estado = usuario.Estado,
            Telefone = usuario.Telefone,
            LinkedIn = usuario.LinkedIn,
            Portfolio = usuario.Portfolio,
            ObjetivosProfissionais = usuario.ObjetivosProfissionais,
            Formacoes = usuario.Formacoes.Select(f => new FormacaoDto
            {
                Id = f.Id,
                Instituicao = f.Instituicao,
                Curso = f.Curso,
                Tipo = f.Tipo,
                DataInicio = f.DataInicio,
                DataConclusao = f.DataConclusao
            }).ToList(),
            Experiencias = usuario.Experiencias.Select(e => new ExperienciaDto
            {
                Id = e.Id,
                Empresa = e.Empresa,
                Cargo = e.Cargo,
                EmpregoAtual = e.EmpregoAtual,
                DataInicio = e.DataInicio,
                DataFim = e.DataFim,
                Atividades = e.Atividades,
                Tecnologias = ParseJsonArray(e.Tecnologias),
                Resultados = e.Resultados
            }).ToList()
        };

        // Parse JSON arrays
        try
        {
            dto.CompetenciasTecnicas = JsonSerializer.Deserialize<List<string>>(usuario.CompetenciasTecnicas) ?? [];
            dto.SoftSkills = JsonSerializer.Deserialize<List<string>>(usuario.SoftSkills) ?? [];
        }
        catch { }

        return dto;
    }

    public async Task<bool> SalvarPerfilAsync(int usuarioId, PerfilDto dto)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Formacoes)
            .Include(u => u.Experiencias)
            .FirstOrDefaultAsync(u => u.Id == usuarioId);

        if (usuario == null)
            return false;

        // Update user personal data
        usuario.Nome = dto.Nome;
        usuario.Cidade = dto.Cidade;
        usuario.Estado = dto.Estado;
        usuario.Telefone = dto.Telefone;
        usuario.LinkedIn = dto.LinkedIn;
        usuario.Portfolio = dto.Portfolio;
        usuario.ObjetivosProfissionais = dto.ObjetivosProfissionais;
        usuario.DataAtualizacao = DateTime.UtcNow;

        // Save skills as JSON
        usuario.CompetenciasTecnicas = JsonSerializer.Serialize(dto.CompetenciasTecnicas ?? []);
        usuario.SoftSkills = JsonSerializer.Serialize(dto.SoftSkills ?? []);

        // Update formações
        var formacaoIds = dto.Formacoes?.Where(f => f.Id.HasValue).Select(f => f.Id.Value).ToList() ?? [];
        var toRemove = usuario.Formacoes.Where(f => !formacaoIds.Contains(f.Id)).ToList();
        _context.Formacoes.RemoveRange(toRemove);

        // Add or update formações
        foreach (var formacaoDto in dto.Formacoes ?? [])
        {
            if (formacaoDto.Id.HasValue)
            {
                var formacao = usuario.Formacoes.FirstOrDefault(f => f.Id == formacaoDto.Id.Value);
                if (formacao != null)
                {
                    formacao.Instituicao = formacaoDto.Instituicao;
                    formacao.Curso = formacaoDto.Curso;
                    formacao.Tipo = formacaoDto.Tipo;
                    formacao.DataInicio = formacaoDto.DataInicio;
                    formacao.DataConclusao = formacaoDto.DataConclusao;
                }
            }
            else
            {
                usuario.Formacoes.Add(new Formacao
                {
                    UsuarioId = usuarioId,
                    Instituicao = formacaoDto.Instituicao,
                    Curso = formacaoDto.Curso,
                    Tipo = formacaoDto.Tipo,
                    DataInicio = formacaoDto.DataInicio,
                    DataConclusao = formacaoDto.DataConclusao,
                    DataCriacao = DateTime.UtcNow
                });
            }
        }

        // Update experiências
        var experienciaIds = dto.Experiencias?.Where(e => e.Id.HasValue).Select(e => e.Id.Value).ToList() ?? [];
        var experienciasToRemove = usuario.Experiencias.Where(e => !experienciaIds.Contains(e.Id)).ToList();
        _context.Experiencias.RemoveRange(experienciasToRemove);

        // Add or update experiências
        foreach (var expDto in dto.Experiencias ?? [])
        {
            if (expDto.Id.HasValue)
            {
                var experiencia = usuario.Experiencias.FirstOrDefault(e => e.Id == expDto.Id.Value);
                if (experiencia != null)
                {
                    experiencia.Empresa = expDto.Empresa;
                    experiencia.Cargo = expDto.Cargo;
                    experiencia.EmpregoAtual = expDto.EmpregoAtual;
                    experiencia.DataInicio = expDto.DataInicio;
                    experiencia.DataFim = expDto.DataFim;
                    experiencia.Atividades = expDto.Atividades;
                    experiencia.Tecnologias = JsonSerializer.Serialize(expDto.Tecnologias ?? []);
                    experiencia.Resultados = expDto.Resultados;
                }
            }
            else
            {
                usuario.Experiencias.Add(new Experiencia
                {
                    UsuarioId = usuarioId,
                    Empresa = expDto.Empresa,
                    Cargo = expDto.Cargo,
                    EmpregoAtual = expDto.EmpregoAtual,
                    DataInicio = expDto.DataInicio,
                    DataFim = expDto.DataFim,
                    Atividades = expDto.Atividades,
                    Tecnologias = JsonSerializer.Serialize(expDto.Tecnologias ?? []),
                    Resultados = expDto.Resultados,
                    DataCriacao = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private List<string> ParseJsonArray(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }
}

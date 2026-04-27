using Microsoft.EntityFrameworkCore;
using SkillMatch.Api.Data;
using SkillMatch.Api.Dtos;
using SkillMatch.Api.Models;

namespace SkillMatch.Api.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
    Task<Usuario?> GetUsuarioByEmailAsync(string email);
    Task<bool> ChangePasswordAsync(int usuarioId, string senhaAtual, string novaSenha);
    Task<bool> DeleteAccountAsync(int usuarioId, string senha);
}

public class AuthService : IAuthService
{
    private readonly SkillMatchContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(SkillMatchContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (usuario == null || !BcryptVerifyPassword(dto.Senha, usuario.SenhaHash))
            return null;

        if (!usuario.IsAtivo)
            return null;

        var token = _tokenService.GenerateToken(usuario);
        return new AuthResponseDto
        {
            Token = token,
            UsuarioId = usuario.Id,
            Nome = usuario.Nome
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        // Verificar se email já existe
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (usuarioExistente != null)
            return null;

        // Criar novo usuário
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BcryptHashPassword(dto.Senha),
            IsAtivo = true,
            DataCriacao = DateTime.UtcNow
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var token = _tokenService.GenerateToken(usuario);
        return new AuthResponseDto
        {
            Token = token,
            UsuarioId = usuario.Id,
            Nome = usuario.Nome
        };
    }

    public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ChangePasswordAsync(int usuarioId, string senhaAtual, string novaSenha)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario == null)
            return false;

        // Verificar se senha atual está correta
        if (!BcryptVerifyPassword(senhaAtual, usuario.SenhaHash))
            return false;

        // Atualizar para nova senha
        usuario.SenhaHash = BcryptHashPassword(novaSenha);
        usuario.DataAtualizacao = DateTime.UtcNow;

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAccountAsync(int usuarioId, string senha)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario == null)
            return false;

        // Verificar se senha está correta
        if (!BcryptVerifyPassword(senha, usuario.SenhaHash))
            return false;

        // Soft delete ou hard delete - vamos fazer soft delete marcando como inativo
        usuario.IsAtivo = false;
        usuario.DataAtualizacao = DateTime.UtcNow;

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        return true;
    }

    private static string BcryptHashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool BcryptVerifyPassword(string password, string hash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            return false;
        }
    }
}

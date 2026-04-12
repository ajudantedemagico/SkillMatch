using Microsoft.EntityFrameworkCore;
using SkillMatch.Api.Data;
using SkillMatch.Api.Models;

namespace SkillMatch.Api.Services;

public interface IPasswordRecoveryService
{
    Task<string?> RequestPasswordRecoveryAsync(string email);
    Task<bool> VerifyCodeAsync(string email, string code);
    Task<bool> ResetPasswordAsync(string email, string code, string novaSenha);
}

public class PasswordRecoveryService : IPasswordRecoveryService
{
    private readonly SkillMatchContext _context;
    private const int CodeExpirationMinutes = 15;

    public PasswordRecoveryService(SkillMatchContext context)
    {
        _context = context;
    }

    public async Task<string?> RequestPasswordRecoveryAsync(string email)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null || !usuario.IsAtivo)
            return null;

        // Gerar código de verificação
        var code = GenerateVerificationCode();
        
        // Limpar códigos antigos
        var oldTokens = await _context.PasswordResetTokens
            .Where(t => t.UsuarioId == usuario.Id && !t.IsUsed)
            .ToListAsync();
        _context.PasswordResetTokens.RemoveRange(oldTokens);

        // Criar novo token
        var token = new PasswordResetToken
        {
            UsuarioId = usuario.Id,
            VerificationCode = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(CodeExpirationMinutes),
            CreatedAt = DateTime.UtcNow,
            IsUsed = false
        };

        _context.PasswordResetTokens.Add(token);
        await _context.SaveChangesAsync();

        // TODO: Enviar código por email
        // Por enquanto, retornamos o código (apenas para testes)
        return code;
    }

    public async Task<bool> VerifyCodeAsync(string email, string code)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null)
            return false;

        var token = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(t => 
                t.UsuarioId == usuario.Id && 
                t.VerificationCode == code && 
                !t.IsUsed && 
                t.ExpiresAt > DateTime.UtcNow);

        return token != null;
    }

    public async Task<bool> ResetPasswordAsync(string email, string code, string novaSenha)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null)
            return false;

        var token = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(t => 
                t.UsuarioId == usuario.Id && 
                t.VerificationCode == code && 
                !t.IsUsed && 
                t.ExpiresAt > DateTime.UtcNow);

        if (token == null)
            return false;

        // Atualizar senha
        usuario.SenhaHash = BcryptHashPassword(novaSenha);
        usuario.DataAtualizacao = DateTime.UtcNow;

        // Marcar token como usado
        token.IsUsed = true;

        await _context.SaveChangesAsync();
        return true;
    }

    private static string GenerateVerificationCode()
    {
        // Gerar código de 4 dígitos
        return Random.Shared.Next(1000, 9999).ToString();
    }

    private static string BcryptHashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillMatch.Api.Dtos;
using SkillMatch.Api.Services;
using System.Security.Claims;

namespace SkillMatch.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPasswordRecoveryService _passwordRecoveryService;

    public AuthController(IAuthService authService, IPasswordRecoveryService passwordRecoveryService)
    {
        _authService = authService;
        _passwordRecoveryService = passwordRecoveryService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
            return BadRequest(new { message = "Email e senha são obrigatórios" });

        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(new { message = "Email ou senha inválidos" });

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
            return BadRequest(new { message = "Nome, email e senha são obrigatórios" });

        if (dto.Senha.Length < 6)
            return BadRequest(new { message = "Senha deve ter no mínimo 6 caracteres" });

        var result = await _authService.RegisterAsync(dto);
        if (result == null)
            return BadRequest(new { message = "Email já está em uso" });

        return Ok(result);
    }

    [HttpPost("password-recovery/request-code")]
    public async Task<ActionResult<PasswordRecoveryResponseDto>> RequestPasswordRecovery([FromBody] RequestPasswordRecoveryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest(new { message = "Email é obrigatório" });

        var code = await _passwordRecoveryService.RequestPasswordRecoveryAsync(dto.Email);
        if (code == null)
            return NotFound(new { message = "Usuário não encontrado" });

        // TODO: Enviar código por email em produção
        // Por enquanto, retornamos o código para testes
        return Ok(new 
        { 
            message = "Código de verificação enviado",
            code = code // REMOVER EM PRODUÇÃO
        });
    }

    [HttpPost("password-recovery/verify-code")]
    public async Task<ActionResult<PasswordRecoveryResponseDto>> VerifyCode([FromBody] VerifyCodeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.CodigoVerificacao))
            return BadRequest(new { message = "Email e código são obrigatórios" });

        var isValid = await _passwordRecoveryService.VerifyCodeAsync(dto.Email, dto.CodigoVerificacao);
        if (!isValid)
            return BadRequest(new { message = "Código inválido ou expirado" });

        return Ok(new 
        { 
            success = true,
            message = "Código verificado com sucesso"
        });
    }

    [HttpPost("password-recovery/reset")]
    public async Task<ActionResult<PasswordRecoveryResponseDto>> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.CodigoVerificacao) || string.IsNullOrWhiteSpace(dto.NovaSenha))
            return BadRequest(new { message = "Email, código e nova senha são obrigatórios" });

        if (dto.NovaSenha.Length < 6)
            return BadRequest(new { message = "Nova senha deve ter no mínimo 6 caracteres" });

        var success = await _passwordRecoveryService.ResetPasswordAsync(dto.Email, dto.CodigoVerificacao, dto.NovaSenha);
        if (!success)
            return BadRequest(new { message = "Falha ao resetar senha. Código pode estar inválido ou expirado" });

        return Ok(new 
        { 
            success = true,
            message = "Senha alterada com sucesso"
        });
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.SenhaAtual) || string.IsNullOrWhiteSpace(dto.NovaSenha))
            return BadRequest(new { message = "Senha atual e nova senha são obrigatórias" });

        if (dto.NovaSenha.Length < 6)
            return BadRequest(new { message = "Nova senha deve ter no mínimo 6 caracteres" });

        var usuarioId = GetUsuarioId();
        if (usuarioId == 0)
            return Unauthorized();

        var success = await _authService.ChangePasswordAsync(usuarioId, dto.SenhaAtual, dto.NovaSenha);
        if (!success)
            return BadRequest(new { message = "Senha atual inválida ou falha ao alterar senha" });

        return Ok(new { message = "Senha alterada com sucesso" });
    }

    [Authorize]
    [HttpDelete("account")]
    public async Task<ActionResult> DeleteAccount([FromBody] DeleteAccountDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Senha))
            return BadRequest(new { message = "Senha é obrigatória para deletar a conta" });

        var usuarioId = GetUsuarioId();
        if (usuarioId == 0)
            return Unauthorized();

        var success = await _authService.DeleteAccountAsync(usuarioId, dto.Senha);
        if (!success)
            return BadRequest(new { message = "Senha inválida ou falha ao deletar conta" });

        return Ok(new { message = "Conta deletada com sucesso" });
    }

    private int GetUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null && int.TryParse(claim.Value, out var id) ? id : 0;
    }
}

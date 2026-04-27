namespace SkillMatch.Api.Dtos;

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class RegisterDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class RequestPasswordRecoveryDto
{
    public string Email { get; set; } = string.Empty;
}

public class VerifyCodeDto
{
    public string Email { get; set; } = string.Empty;
    public string CodigoVerificacao { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    public string Email { get; set; } = string.Empty;
    public string CodigoVerificacao { get; set; } = string.Empty;
    public string NovaSenha { get; set; } = string.Empty;
}

public class PasswordRecoveryResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ChangePasswordDto
{
    public string SenhaAtual { get; set; } = string.Empty;
    public string NovaSenha { get; set; } = string.Empty;
}

public class DeleteAccountDto
{
    public string Senha { get; set; } = string.Empty;
}

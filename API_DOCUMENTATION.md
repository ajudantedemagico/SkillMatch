# SkillMatch Backend - Documentação da API

## Status da Implementação 

O backend está **totalmente funcional** para login, registro e recuperação de senha.

## Configuração

- **URL Base**: `http://localhost:5000/api`
- **Banco de Dados**: SQLite (`skillmatch.db`)
- **Autenticação**: JWT (7 dias de validade)

## Endpoints Implementados

### 1. **Login**
```
POST /auth/login
Content-Type: application/json

{
  "email": "usuario@example.com",
  "senha": "minhasenha123"
}
```

**Resposta (200 OK)**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuarioId": 1,
  "nome": "João Silva"
}
```

---

### 2. **Registro**
```
POST /auth/register
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "minhasenha123"
}
```

**Resposta (200 OK)**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuarioId": 1,
  "nome": "João Silva"
}
```

**Validações**:
- Senha mínima: 6 caracteres
- Email: obrigatório e único
- Nome: obrigatório

---

### 3. **Solicitar Recuperação de Senha**
```
POST /auth/password-recovery/request-code
Content-Type: application/json

{
  "email": "joao@example.com"
}
```

**Resposta (200 OK)**:
```json
{
  "message": "Código de verificação enviado",
  "code": "1234"
}
```

 **IMPORTANTE**: Atualmente, o código é retornado na resposta (apenas para testes). Em produção, será enviado por email.

---

### 4. **Verificar Código de Recuperação**
```
POST /auth/password-recovery/verify-code
Content-Type: application/json

{
  "email": "joao@example.com",
  "codigoVerificacao": "1234"
}
```

**Resposta (200 OK)**:
```json
{
  "success": true,
  "message": "Código verificado com sucesso"
}
```

---

### 5. **Resetar Senha**
```
POST /auth/password-recovery/reset
Content-Type: application/json

{
  "email": "joao@example.com",
  "codigoVerificacao": "1234",
  "novaSenha": "novaSenha123"
}
```

**Resposta (200 OK)**:
```json
{
  "success": true,
  "message": "Senha alterada com sucesso"
}
```

---

## Fluxo de Recuperação de Senha (Frontend)

A implementação no backend suporta o fluxo de 4 etapas do seu frontend:

1. **Step 1**: Email → `POST /password-recovery/request-code`
2. **Step 2**: Código 4-dígitos → `POST /password-recovery/verify-code`
4. **Step 4**: Sucesso (redirecionamento no frontend)

---

## Recursos de Segurança

- **Senhas com Hash**: BCrypt com salt
- **Tokens JWT**: Válidos por 7 dias
- **Códigos de Verificação**: Expiram em 15 minutos
- **CORS Habilitado**: Aceita requisições do frontend
- **Email Único**: Index de banco de dados garante unicidade

---

## Variáveis de Configuração

**`appsettings.json`**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=skillmatch.db"
  },
  "Jwt": {
    "Key": "your-secret-key-change-this-in-production-minimum-32-characters-long",
    "Issuer": "SkillMatch",
    "Audience": "SkillMatchApp"
  }
}
```

**TODO em Produção**: 
- Mudar a chave JWT para uma chave segura e única
- Implementar envio real de emails

---

## Testando a API

### Usando cURL

**Login**:
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","senha":"password123"}'
```

**Registro**:
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"nome":"Test User","email":"test@example.com","senha":"password123"}'
```

### Usando Postman/Insomnia
Importe este arquivo ou crie coleções com os endpoints acima.

---

## Próximos Passos

1. **Email Service**: Integrar envio de emails (SendGrid, SMTP, etc.)
2. **Perfil do Usuário**: Endpoints GET/PUT `/api/perfil`
3. **Currículo**: Endpoints para gerenciar CVs
4. **Autenticação em Endpoints Protegidos**: Usar header `Authorization: Bearer <token>`

---

## Troubleshooting

**Erro: "Email já está em uso"**
- O email já foi registrado. Use outro email ou reset a senha.

**Erro: "Código inválido ou expirado"**
- O código expirou (15 minutos) ou está incorreto.
- Solicite um novo código.

**Erro: CORS bloqueado**
- Verifique se o frontend está enviando as requisições para `http://localhost:5000/api`

---

**Desenvolvido em**: 12 de Abril de 2026

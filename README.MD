# SkillMatch

Aplicação web para otimização de currículos usando IA. Gere CVs personalizados que combinam perfeitamente com a vaga de seus sonhos!

## Stack

### Frontend
- **HTML** - Estrutura semântica
- **CSS** - Styled responsivo
- **JavaScript Vanilla** - Sem dependências

### Backend
- **C#** - Linguagem
- **ASP.NET Core 10.0** - Framework web
- **Entity Framework Core** - ORM para banco de dados

### Banco de Dados
- **SQLite** (Desenvolvimento)
- **SQL Server** (Produção - futuro)

## Arquitetura

```
Frontend (HTML/CSS/JS) ↔ API REST ↔ Banco de Dados
```

## Como Rodar

### Backend
```bash
cd backend/SkillMatch.Api
dotnet run --launch-profile http
```
O backend estará disponível em: `http://localhost:5000/api`

### Frontend
Opção 1: VS Code Live Server
```
1. Instale a extensão "Live Server"
2. Clique com direito em index.html
3. Selecione "Open with Live Server"
```

Opção 2: Abrir no navegador
```
Simplesmente abra o arquivo index.html no navegador
```

##  Funcionalidades Implementadas

### Autenticação
- [x] Registro de usuário
- [x] Login com email e senha
- [x] JWT tokens (7 dias de validade)
- [x] Recuperação de senha com código de verificação

### Recuperação de Senha (4 Etapas)
- [x] Solicitar código de recuperação
- [x] Verificar código (válido por 15 minutos)
- [x] Resetar senha

### Segurança
- [x] Senhas com hash BCrypt
- [x] Tokens JWT seguros
- [x] CORS habilitado
- [x] Email único no banco de dados

## API Endpoints

### Autenticação
```
POST /api/auth/register              - Criar nova conta
POST /api/auth/login                 - Fazer login
POST /api/auth/password-recovery/request-code  - Solicitar código
POST /api/auth/password-recovery/verify-code   - Verificar código
POST /api/auth/password-recovery/reset         - Resetar senha
```

Documentação completa: [API_DOCUMENTATION.md](API_DOCUMENTATION.md)

## Testando

### Opção 1: No Arquivo `.http` (VS Code)
Abra `backend/SkillMatch.Api/SkillMatch.Api.http` e clique em "Send Request"

### Opção 2: cURL
```bash
# Registrar
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"nome":"João","email":"joao@test.com","senha":"123456"}'

# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@test.com","senha":"123456"}'
```

### Opção 3: Frontend (index.html)
Complete os formulários de login/cadastro na página inicial e teste!

## Estrutura do Projeto

```
SkillMatch/
├── backend/SkillMatch.Api/
│   ├── Controllers/         # Endpoints da API
│   ├── Models/              # Entidades do banco (Usuario, PasswordResetToken)
│   ├── Services/            # Lógica de negócio (Auth, TokenService, etc)
│   ├── Data/                # DbContext e configurações
│   ├── Dtos/                # Data Transfer Objects
│   ├── Migrations/          # Migrações Entity Framework
│   └── Program.cs           # Configuração da aplicação
├── frontend/
│   ├── index.html           # Página de login/cadastro
│   ├── app.html             # App principal (TODO)
│   ├── pages/
│   │   ├── recuperar-senha.html
│   │   ├── perfil.html
│   │   ├── cv-editar.html
│   │   └── historico.html
│   ├── js/
│   │   ├── api.js           # Cliente HTTP para API
│   │   ├── auth.js          # Handlers de autenticação
│   │   └── app.js           # Lógica principal
│   └── css/
│       └── style.css        # Estilos
├── docs/
│   └── api-endpoints.md     # Documentação técnica
├── API_DOCUMENTATION.md     
└── README.md               
```

## Fluxo de Autenticação

```
1. Usuário se registra (POST /auth/register)
   ↓
2. Backend cria conta + gera JWT token
   ↓
3. Token armazenado em localStorage
   ↓
4. Usuário redirecionado para app.html
   ↓
5. Token enviado em cada requisição (Authorization: Bearer <token>)
```

## Configurações Importantes

#### JWT Secret
**Localização**: `backend/SkillMatch.Api/appsettings.json`

```json
"Jwt": {
  "Key": "seu-secret-key-aqui",
  "Issuer": "SkillMatch",
  "Audience": "SkillMatchApp"
}
```

**Em produção**, mude essa chave para algo seguro e único!

#### Banco de Dados
**Desenvolvimento**: SQLite (`skillmatch.db`)
**Produção**: Será migrado para SQL Server (veja seção Futuro)

## Próximos Passos

- [ ] Implementar endpoints de perfil (GET/PUT)
- [ ] Implementar endpoints de currículo
- [ ] Integração com IA (geração de CV)
- [ ] Email service (SendGrid/SMTP)
- [ ] Validação de email
- [ ] Testes automatizados
- [ ] Dashboard de usuário


## Troubleshooting

**Erro: CORS bloqueado**
- Verifique se `http://localhost:5000/api` está na lista de origens permitidas

**Erro: Banco de dados não encontrado**
- O SQLite é criado automaticamente na primeira execução

**Erro: JWT inválido**
- Token expirou (7 dias) ou chave JWT mudou
- Faça login novamente

##  Suporte

Para dúvidas ou problemas:
1. Verifique a [API_DOCUMENTATION.md](API_DOCUMENTATION.md)
2. Observe os logs do terminal do backend
3. Abra a aba Network no DevTools (F12) do navegador

---

**Status**: ✅ Backend funcional  
**Última atualização**: 12 de Abril de 2026

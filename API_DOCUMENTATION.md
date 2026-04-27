# SkillMatch Backend - Documentação da API

## Status da Implementação 

✅ **Autenticação**: Login, Registro, Recuperação de Senha, Alterar Senha, Deletar Conta
✅ **Perfil**: Visualizar, GET/PUT completo com Formações e Experiências
✅ **Currículo**: Geração com IA (em progresso), Salvar, Listar, Obter detalhes

## Configuração

- **URL Base**: `http://localhost:5000/api`
- **Banco de Dados**: SQLite (`skillmatch.db`)
- **Autenticação**: JWT (7 dias de validade)
- **Endpoints Protegidos**: Requerem header `Authorization: Bearer <token>`

---

## 📋 Endpoints Implementados (13 Total)

### **AUTENTICAÇÃO** (7 endpoints)

#### 1. Login
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

#### 2. Registro
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

---

#### 3. Solicitar Recuperação de Senha
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

---

#### 4. Verificar Código de Recuperação
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

#### 5. Resetar Senha
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

#### 6. Alterar Senha (Logado)
```
PUT /api/auth/change-password
Authorization: Bearer <jwt_token>
Content-Type: application/json

{
  "senhaAtual": "minhasenha123",
  "novaSenha": "novaSenha456"
}
```

**Resposta (200 OK)**:
```json
{
  "message": "Senha alterada com sucesso"
}
```

**Erro (400)**:
- `"Senha atual inválida..."` - Senha atual está incorreta
- `"Nova senha deve ter no mínimo 6 caracteres"` - Validação de comprimento

---

#### 7. Deletar Conta
```
DELETE /api/auth/account
Authorization: Bearer <jwt_token>
Content-Type: application/json

{
  "senha": "minhasenha123"
}
```

**Resposta (200 OK)**:
```json
{
  "message": "Conta deletada com sucesso"
}
```

**Nota**: A conta é marcada como inativa (soft delete). Todos os dados são preservados no banco de dados.

---

### **PERFIL** (2 endpoints) ⭐ NOVO

#### 8. Obter Perfil
```
GET /api/perfil
Authorization: Bearer <jwt_token>
```

**Resposta (200 OK)**:
```json
{
  "nome": "João Silva",
  "email": "joao@example.com",
  "cidade": "São Paulo",
  "estado": "SP",
  "telefone": "11999999999",
  "linkedin": "linkedin.com/in/joao",
  "portfolio": "joao.dev",
  "formacoes": [
    {
      "id": 1,
      "instituicao": "USP",
      "curso": "Ciência da Computação",
      "tipo": "Graduação",
      "dataInicio": "2020-01-01",
      "dataConclusao": "2024-12-15"
    }
  ],
  "experiencias": [
    {
      "id": 1,
      "empresa": "TechCorp",
      "cargo": "Dev Senior",
      "empregoAtual": true,
      "dataInicio": "2022-06-01",
      "dataFim": null,
      "atividades": "Desenvolvimento de APIs",
      "tecnologias": ["C#", "ASP.NET", "SQL Server"],
      "resultados": "Reduziu tempo de resposta em 40%"
    }
  ],
  "competenciasTecnicas": ["C#", "ASP.NET", "React", "SQL Server"],
  "objetivosProfissionais": "Ser arquiteto de software",
  "softSkills": ["Liderança", "Comunicação", "Trabalho em Equipe"]
}
```

---

#### 9. Salvar Perfil
```
PUT /api/perfil
Authorization: Bearer <jwt_token>
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@example.com",
  "cidade": "São Paulo",
  "estado": "SP",
  "telefone": "11999999999",
  "linkedin": "linkedin.com/in/joao",
  "portfolio": "joao.dev",
  "formacoes": [
    {
      "instituicao": "USP",
      "curso": "Ciência da Computação",
      "tipo": "Graduação",
      "dataInicio": "2020-01-01",
      "dataConclusao": "2024-12-15"
    }
  ],
  "experiencias": [
    {
      "empresa": "TechCorp",
      "cargo": "Dev Senior",
      "empregoAtual": true,
      "dataInicio": "2022-06-01",
      "dataFim": null,
      "atividades": "Desenvolvimento de APIs",
      "tecnologias": ["C#", "ASP.NET"],
      "resultados": "Reduziu tempo de resposta"
    }
  ],
  "competenciasTecnicas": ["C#", "ASP.NET", "React"],
  "objetivosProfissionais": "Ser arquiteto de software",
  "softSkills": ["Liderança", "Comunicação"]
}
```

**Resposta (200 OK)**: Retorna o perfil atualizado

---

### **CURRÍCULO** (4 endpoints) ⭐ NOVO

#### 10. Gerar Currículo com IA
```
POST /api/curriculos/gerar
Authorization: Bearer <jwt_token>
Content-Type: application/json

{
  "descricaoVaga": "Procuramos um Dev Senior com C#, ASP.NET e React...",
  "consentimentoIA": true
}
```

**Resposta (200 OK)**:
```json
{
  "id": 0,
  "titulo": "CV - João Silva",
  "secoes": {
    "cabecalho": {
      "nome": "João Silva",
      "titulo": "Senior Developer",
      "email": "joao@example.com",
      "telefone": "11999999999",
      "linkedin": "linkedin.com/in/joao",
      "portfolio": "joao.dev",
      "localizacao": "São Paulo, SP"
    },
    "resumoBio": {
      "conteudo": "Profissional com experiência em desenvolvimento..."
    },
    "experiencias": [
      {
        "empresa": "TechCorp",
        "cargo": "Dev Senior",
        "dataInicio": "2022-06-01",
        "dataFim": null,
        "descricao": "Descrição otimizada para a vaga...",
        "tecnologias": ["C#", "ASP.NET", "SQL"]
      }
    ],
    "competencias": {
      "tecnicas": ["C#", "ASP.NET", "React", "SQL Server"],
      "comportamentais": ["Liderança", "Comunicação"]
    },
    "formacoes": [
      {
        "instituicao": "USP",
        "curso": "Ciência da Computação",
        "tipo": "Graduação",
        "dataInicio": "2020-01-01",
        "dataConclusao": "2024-12-15"
      }
    ],
    "certificacoes": []
  }
}
```

---

#### 11. Salvar Currículo
```
POST /api/curriculos/salvar
Authorization: Bearer <jwt_token>
Content-Type: application/json

{
  "titulo": "CV - TechCorp Senior Dev",
  "descricaoVaga": "Procuramos um Dev Senior com C#...",
  "secoes": { /* mesma estrutura do endpoint gerar */ },
  "cabecalhoEditado": false,
  "resumoBioEditado": true,
  "experienciaEditada": false,
  "competenciasEditadas": false,
  "formacaoEditada": false
}
```

**Resposta (200 OK)**:
```json
{
  "id": 1,
  "titulo": "CV - TechCorp Senior Dev",
  "secoes": { },
  "dataGeracao": "2026-04-27T10:45:00Z",
  "dataAtualizacao": null,
  "cabecalhoEditado": false,
  "resumoBioEditado": true
}
```

---

#### 12. Listar Currículos
```
GET /api/curriculos
Authorization: Bearer <jwt_token>
```

**Resposta (200 OK)**:
```json
[
  {
    "id": 1,
    "titulo": "CV - TechCorp Senior Dev",
    "dataGeracao": "2026-04-27T10:45:00Z",
    "descricaoVaga": "Procuramos um Dev Senior...",
    "visualizacoes": 0,
    "foiEditado": true
  },
  {
    "id": 2,
    "titulo": "CV - Startup Frontend",
    "dataGeracao": "2026-04-26T15:30:00Z",
    "descricaoVaga": "Procuramos um Dev Frontend React...",
    "visualizacoes": 5,
    "foiEditado": false
  }
]
```

---

#### 13. Obter Detalhes do Currículo
```
GET /api/curriculos/{id}
Authorization: Bearer <jwt_token>
```

**Resposta (200 OK)**:
```json
{
  "id": 1,
  "titulo": "CV - TechCorp Senior Dev",
  "descricaoVaga": "Procuramos um Dev Senior...",
  "secoes": { /* estrutura completa */ },
  "dataGeracao": "2026-04-27T10:45:00Z",
  "dataAtualizacao": "2026-04-27T11:20:00Z",
  "cabecalhoEditado": false,
  "resumoBioEditado": true,
  "experienciaEditada": false,
  "competenciasEditadas": false,
  "formacaoEditada": false
}
```

---

## 🔐 Recursos de Segurança

- ✅ **Senhas com Hash**: BCrypt com salt automático
- ✅ **Tokens JWT**: Válidos por 7 dias, assinados com chave secreta
- ✅ **Códigos de Verificação**: Expiram em 15 minutos
- ✅ **CORS Habilitado**: Aceita requisições do frontend
- ✅ **Email Único**: Index de banco de dados garante unicidade
- ✅ **Endpoints Protegidos**: Requerem token JWT válido
- ✅ **Validação de Entrada**: Todos os dados são validados

---

## ⚙️ Variáveis de Configuração

**`appsettings.json`**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=skillmatch.db"
  },
  "Jwt": {
    "Key": "your-secret-key-change-this-in-production",
    "Issuer": "SkillMatch",
    "Audience": "SkillMatchApp"
  }
}
```

---

## 🧪 Testando a API

### Opção 1: Arquivo HTTP (VS Code)
Abra `backend/SkillMatch.Api/SkillMatch.Api.http` e clique em "Send Request"

### Opção 2: Frontend
Abra `index.html` com Live Server e use os formulários

### Opção 3: cURL
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"nome":"João","email":"joao@test.com","senha":"123456"}'
```

---

## 📊 Modelo de Dados

**Tabelas**: Usuarios, PasswordResetTokens, Formacoes, Experiencias, Curriculos

**Relacionamentos**:
- Usuario (1) ──→ (N) Formacao
- Usuario (1) ──→ (N) Experiencia
- Usuario (1) ──→ (N) Curriculo
- Usuario (1) ──→ (N) PasswordResetToken

---

## 🚀 Próximos Passos

- [ ] Email Service (SendGrid/SMTP)
- [ ] IA Integration (OpenAI)
- [ ] PDF Export
- [ ] Share CV Link
- [ ] Tests
- [ ] Rate Limiting
- [ ] Audit Logging

---

**Última atualização**: 27 de Abril de 2026 (13 endpoints, 2 novos)
**Status**: ✅ Pronto para Testes

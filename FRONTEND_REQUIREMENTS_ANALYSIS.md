# SkillMatch Frontend - API Requirements & Data Models

**Date:** April 27, 2026  
**Status:** Frontend Analysis Complete

---

## Executive Summary

The SkillMatch frontend is a single-page application with 3 main workflows:

1. **Profile Management** - Multi-step form to create/edit the "Anchor Profile"
2. **CV Generation** - AI-powered CV tailoring based on job description
3. **CV History** - Browse and view previously saved CVs

This document specifies all API endpoints required, request/response structures, data models, and form field definitions.

---

## 1. PROFILE PAGE - Perfil Âncora (`pages/perfil.html`)

### Overview
Multi-step form with 6 sequential steps. Users complete once, then load/edit via the same page.

**Progress Tracking:** Visual step dots (1-6) at top of page

---

### Step 1: Personal Data (Dados Pessoais)

#### Fields
| Field | Type | Required | Validation | Notes |
|-------|------|----------|-----------|-------|
| `nome` | Text | Yes | - | Full name |
| `cidade` | Text | Yes | - | City |
| `estado` | Text | Yes | Max 2 chars | State/UF code (SP, RJ, etc) |
| `email` | Email | Yes | Valid email | Auto-populated from auth |
| `telefone` | Tel | Yes | - | Phone number, placeholder: "(11) 91234-5678" |
| `linkedin` | URL | No | Valid URL or empty | LinkedIn profile link |
| `portfolio` | URL | No | Valid URL or empty | GitHub/Portfolio link |

---

### Step 2: Education (Escolaridade)

#### Multiple Entries
Each entry is a form card with remove button.

| Field | Type | Required | Values | Notes |
|-------|------|----------|--------|-------|
| `instituicao` | Text | Yes | - | School/University name |
| `curso` | Text | Yes | - | Degree/Course name |
| `tipo` | Dropdown | Yes | `graduacao`, `pos-graduacao`, `tecnico`, `livre` | Type of education |
| `inicio` | Month | Yes | YYYY-MM | Start date |
| `conclusao` | Month | No | YYYY-MM | End date (nullable) |

#### Data Structure (Array)
```json
{
  "formacoes": [
    {
      "instituicao": "Universidade São Paulo",
      "curso": "Sistemas de Informação",
      "tipo": "graduacao",
      "dataInicio": "2018-03-01T00:00:00Z",
      "dataConclusao": "2022-12-01T00:00:00Z"
    }
  ]
}
```

---

### Step 3: Work Experience (Histórico Profissional)

#### Multiple Entries
Each entry is a form card with remove button.

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| `empresa` | Text | Yes | Company name |
| `cargo` | Text | Yes | Job title |
| `empregoAtual` | Checkbox | No | Current job? (hides "Fim" field if checked) |
| `inicio` | Month | Yes | YYYY-MM start date |
| `fim` | Month | No* | YYYY-MM end date (*hidden if "empregoAtual" checked) |
| `atividades` | Textarea | Yes | Main responsibilities/activities |
| `tecnologias` | Text | No | Comma-separated tech stack |
| `resultados` | Textarea | No | Results/Achievements |

#### Data Structure (Array)
```json
{
  "experiencias": [
    {
      "empresa": "Tech Corp",
      "cargo": "Senior Backend Developer",
      "empregoAtual": false,
      "dataInicio": "2020-01-01T00:00:00Z",
      "dataFim": "2022-12-31T00:00:00Z",
      "principaisAtividades": "Developed REST APIs using C# and .NET Core...",
      "tecnologiasUtilizadas": ["C#", "NET", "SQL Server", "Docker"],
      "resultadosConquistas": "Improved API performance by 40%..."
    }
  ]
}
```

---

### Step 4: Technical Skills (Competências Técnicas)

#### Input
- Text input with "Add" button
- Press Enter or click button to add tag
- Remove tag by clicking × on tag

#### Data Structure (Array)
```json
{
  "competencias": ["JavaScript", "C#", "React", "SQL", "Docker"]
}
```

---

### Step 5: Professional Objective (Objetivo Profissional)

#### Field
| Field | Type | Required | Guidelines |
|-------|------|----------|------------|
| `resumo` | Textarea | No | Professional summary (150-300 words recommended) |

#### Notes
- Self-written career objective or summary
- Used as professional profile description
- May be AI-enhanced later

#### Data Structure
```json
{
  "resumoProfissional": "Backend developer with 5 years experience..."
}
```

---

### Step 6: Soft Skills (Soft Skills)

#### Input
- Text input with "Add" button
- Quick suggestions provided: Communication, Teamwork, Proactivity, Adaptability, Responsibility, Analytical Thinking
- Remove tag by clicking × on tag

#### Data Structure (Array)
```json
{
  "softSkills": ["Communication", "Leadership", "Teamwork"]
}
```

---

### API Endpoints

#### GET /api/perfil
**Purpose:** Load existing user profile for editing

**Headers:**
```
Authorization: Bearer {jwt_token}
```

**Response (200 OK):**
```json
{
  "nomeCompleto": "João Silva",
  "cidade": "São Paulo",
  "estado": "SP",
  "email": "joao@example.com",
  "telefone": "(11) 98765-4321",
  "linkedin": "linkedin.com/in/joaosilva",
  "portfolio": "github.com/joaosilva",
  "resumoProfissional": "Backend developer with 5+ years experience...",
  "competencias": ["C#", "NET", "SQL", "Docker"],
  "softSkills": ["Communication", "Leadership"],
  "formacoes": [
    {
      "instituicao": "Universidade São Paulo",
      "curso": "Sistemas de Informação",
      "tipo": "graduacao",
      "dataInicio": "2018-03-01T00:00:00Z",
      "dataConclusao": "2022-12-01T00:00:00Z"
    }
  ],
  "experiencias": [
    {
      "empresa": "Tech Corp",
      "cargo": "Senior Backend Developer",
      "empregoAtual": false,
      "dataInicio": "2020-01-01T00:00:00Z",
      "dataFim": "2022-12-31T00:00:00Z",
      "principaisAtividades": "Developed REST APIs...",
      "tecnologiasUtilizadas": ["C#", "NET", "SQL Server"],
      "resultadosConquistas": "Improved API performance by 40%"
    }
  ]
}
```

**Response (204 No Content):** If user has no profile (first time)

---

#### PUT /api/perfil
**Purpose:** Save/update user profile (complete replacement)

**Headers:**
```
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "nomeCompleto": "João Silva",
  "cidade": "São Paulo",
  "estado": "SP",
  "email": "joao@example.com",
  "telefone": "(11) 98765-4321",
  "linkedin": "linkedin.com/in/joaosilva",
  "portfolio": "github.com/joaosilva",
  "resumoProfissional": "Backend developer with 5+ years...",
  "competencias": ["C#", "NET", "SQL", "Docker"],
  "softSkills": ["Communication", "Leadership"],
  "formacoes": [
    {
      "instituicao": "Universidade São Paulo",
      "curso": "Sistemas de Informação",
      "tipo": "graduacao",
      "dataInicio": "2018-03-01T00:00:00Z",
      "dataConclusao": "2022-12-01T00:00:00Z"
    }
  ],
  "experiencias": [
    {
      "empresa": "Tech Corp",
      "cargo": "Senior Backend Developer",
      "empregoAtual": false,
      "dataInicio": "2020-01-01T00:00:00Z",
      "dataFim": "2022-12-31T00:00:00Z",
      "principaisAtividades": "Developed REST APIs...",
      "tecnologiasUtilizadas": ["C#", "NET", "SQL Server"],
      "resultadosConquistas": "Improved performance by 40%"
    }
  ]
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Perfil salvo com sucesso"
}
```

---

## 2. CV GENERATION FLOW

### Workflow Overview
```
User → Chat Page → (with job description & consent)
                   → POST /curriculos/gerar
                   → Save to sessionStorage
                   → Navigate to CV Edit Page
                   → Review/edit AI-generated content
                   → POST /curriculos/salvar
                   → Redirect to History
```

---

### Chat Page (`pages/chat.html`)

#### Fields
| Field | Type | Required | Notes |
|-------|------|----------|-------|
| `descricaoVaga` | Textarea | Yes | Full job description (12 rows) |
| `consentimento` | Checkbox | Yes | "I agree to use my data for AI processing" |

#### UI Elements
- Warning box about data sent to OpenAI
- "Gerar Currículo" button disabled until both fields filled

---

### API Endpoint: POST /api/curriculos/gerar

**Purpose:** Generate CV sections using AI based on job description and user's profile

**Headers:**
```
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "descricaoVaga": "Buscamos Desenvolvedor(a) Back-end C# para integrar nosso time...",
  "consentimentoIA": true
}
```

**Response (200 OK):**
```json
{
  "resumoProfissional": "Desenvolvedor Back-end C# com experiência em .NET Core...",
  "experienciaProfissional": [
    {
      "empresa": "Tech Corp",
      "cargo": "Senior Backend Developer",
      "periodo": "Jan 2020 - Dez 2022",
      "atividades": [
        "Developed REST APIs using C# and .NET",
        "Optimized database queries improving performance by 40%",
        "Mentored junior developers"
      ],
      "resultados": "Improved system performance significantly",
      "tecnologias": ["C#", "NET", "SQL Server", "Docker"]
    }
  ],
  "formacaoAcademica": [
    {
      "curso": "Sistemas de Informação",
      "instituicao": "Universidade São Paulo",
      "tipo": "graduacao",
      "periodo": "2018 - 2022"
    }
  ],
  "competenciasTecnicas": ["C#", "NET", "SQL Server", "Docker", "Entity Framework"],
  "softSkills": ["Communication", "Leadership", "Problem-solving"]
}
```

**Flow After Success:**
1. Save response to `sessionStorage.setItem('cv_secoes', JSON.stringify(response))`
2. Save job description to `sessionStorage.setItem('cv_vaga', descricaoVaga)`
3. Navigate to `cv-editar.html`

---

## 3. CV EDIT PAGE (`pages/cv-editar.html`)

### Purpose
Review AI-generated CV, make edits, and save to history. Track which sections were manually edited.

---

### Fields & Sections

#### 1. CV Title (Required)
```html
<input type="text" id="cv-titulo" placeholder="CV Dev Backend - Tech Corp" />
```

---

#### 2. Professional Summary (📝)
- Editable textarea
- **Edit Flag:** `resumoProfissionalEditado`

```json
{
  "resumoProfissional": "AI-generated or manually edited professional summary..."
}
```

---

#### 3. Work Experience (💼)
- Read-only cards from sessionStorage
- Editable activities textarea
- **Edit Flag:** `experienciaEditada`

**Editable Field:**
```
Atividades (textarea) - each line is an activity bullet
```

**Data Structure:**
```json
{
  "experienciaProfissional": [
    {
      "empresa": "Tech Corp",
      "cargo": "Senior Backend Developer",
      "periodo": "Jan 2020 - Dez 2022",
      "atividades": ["Activity 1", "Activity 2"],
      "resultados": "Results text",
      "tecnologias": ["C#", "NET"]
    }
  ]
}
```

---

#### 4. Education (🎓)
- Display-only (read-only)
- **Edit Flag:** `formacaoEditada` (always false, no UI for editing)

---

#### 5. Technical Skills (⚙️)
- Editable tags
- Add/remove functionality
- **Edit Flag:** `competenciasEditadas`

```json
{
  "competenciasTecnicas": ["C#", "NET", "SQL"]
}
```

---

#### 6. Soft Skills (🤝)
- Editable tags
- Add/remove functionality
- **Edit Flag:** `softSkillsEditadas`

```json
{
  "softSkills": ["Communication", "Leadership"]
}
```

---

### Edit Tracking System (RN-08)

Frontend tracks which sections were manually modified by user:

```json
{
  "resumoProfissionalEditado": false,
  "experienciaEditada": false,
  "formacaoEditada": false,
  "competenciasEditadas": false,
  "softSkillsEditadas": false
}
```

Each flag set to `true` when user modifies that section. Backend can use this to:
- Track edit patterns
- Provide UI badges (✏️ "Edited manually")
- Support future version control features

---

### API Endpoint: POST /api/curriculos/salvar

**Purpose:** Save the CV to user's history

**Headers:**
```
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

**Request Body:**
```json
{
  "titulo": "CV Dev Backend - Tech Corp",
  "descricaoVaga": "Buscamos Desenvolvedor(a) Back-end C# para integrar...",
  "secoes": {
    "resumoProfissional": "Desenvolvedor Back-end C# com experiência...",
    "experienciaProfissional": [
      {
        "empresa": "Tech Corp",
        "cargo": "Senior Backend Developer",
        "periodo": "Jan 2020 - Dez 2022",
        "atividades": ["Activity 1", "Activity 2"],
        "resultados": "Results text",
        "tecnologias": ["C#", "NET", "SQL Server"]
      }
    ],
    "formacaoAcademica": [
      {
        "curso": "Sistemas de Informação",
        "instituicao": "Universidade São Paulo",
        "tipo": "graduacao",
        "periodo": "2018 - 2022"
      }
    ],
    "competenciasTecnicas": ["C#", "NET", "SQL Server", "Docker"],
    "softSkills": ["Communication", "Leadership"]
  },
  "resumoProfissionalEditado": false,
  "experienciaEditada": true,
  "formacaoEditada": false,
  "competenciasEditadas": true,
  "softSkillsEditadas": false
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "id": 123,
  "message": "CV salvo no histórico"
}
```

**Post-Success Flow:**
1. Clear sessionStorage: `cv_secoes`, `cv_vaga`
2. Show success toast
3. Redirect to `historico.html` after 1.5 seconds

---

## 4. HISTORY PAGE (`pages/historico.html`)

### Overview
Displays all CVs created by the user. Shows summary cards clickable to view full CV details in a modal.

---

### Card Layout
Each CV appears as a clickable card with:

```
[Title]
Generated on [date] · Profile v[number]
[Job description first 80 chars]...
[✏️ Edited manually (if applicable)]
```

---

### API Endpoint: GET /api/curriculos

**Purpose:** List all CVs for the authenticated user

**Headers:**
```
Authorization: Bearer {jwt_token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "titulo": "CV Dev Backend - Tech Corp",
    "geradoEm": "2024-04-25T10:30:00Z",
    "versaoPerfilAncora": 1,
    "descricaoVaga": "Buscamos Desenvolvedor(a) Back-end C# para integrar...",
    "editadoManualmente": true
  },
  {
    "id": 2,
    "titulo": "CV Dev Full Stack - StartUp XYZ",
    "geradoEm": "2024-04-20T14:15:00Z",
    "versaoPerfilAncora": 1,
    "descricaoVaga": "Procuramos Full Stack Developer com React e Node.js...",
    "editadoManualmente": false
  }
]
```

**Empty State (200 OK):**
```json
[]
```

---

### API Endpoint: GET /api/curriculos/{id}

**Purpose:** Get full CV details for modal display

**Headers:**
```
Authorization: Bearer {jwt_token}
```

**URL Parameters:**
- `id` - CV ID (number)

**Response (200 OK):**
```json
{
  "id": 1,
  "titulo": "CV Dev Backend - Tech Corp",
  "geradoEm": "2024-04-25T10:30:00Z",
  "versaoPerfilAncora": 1,
  "descricaoVaga": "Buscamos Desenvolvedor(a) Back-end...",
  "editadoManualmente": true,
  "secoes": {
    "resumoProfissional": "Desenvolvedor Back-end C# com experiência em .NET Core...",
    "experienciaProfissional": [
      {
        "empresa": "Tech Corp",
        "cargo": "Senior Backend Developer",
        "periodo": "Jan 2020 - Dez 2022",
        "atividades": ["Developed REST APIs", "Optimized DB queries"],
        "resultados": "Improved performance by 40%",
        "tecnologias": ["C#", "NET", "SQL Server"]
      }
    ],
    "formacaoAcademica": [
      {
        "curso": "Sistemas de Informação",
        "instituicao": "Universidade São Paulo",
        "tipo": "graduacao",
        "periodo": "2018 - 2022"
      }
    ],
    "competenciasTecnicas": ["C#", "NET", "SQL Server", "Docker"],
    "softSkills": ["Communication", "Leadership"]
  }
}
```

---

### Modal Sections

#### 1. Header
- Back button
- CV title
- Generated date, profile version, edit status

#### 2. Resume Section
```
📝 Resumo Profissional
[Full text displayed]
```

#### 3. Experience Section
```
💼 Experiência Profissional
[Company] — [Job Title]
[Period]
• Activity 1
• Activity 2
[Tech: C# · NET · SQL]
```

#### 4. Education Section
```
🎓 Formação Acadêmica
[Course] — [Institution]
[Type] · [Period]
```

#### 5. Technical Skills
```
⚙️ Competências Técnicas
[Tag] [Tag] [Tag]
```

#### 6. Soft Skills
```
🤝 Soft Skills
[Tag] [Tag] [Tag]
```

---

## 5. AUTHENTICATION & SECURITY

### Token Management

**Storage:**
```javascript
localStorage.getItem('sm_token')      // JWT Bearer token
localStorage.getItem('sm_user')       // {id, nome} JSON
```

**Authorization Header:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**On 401 Unauthorized:**
- Clear token and user from localStorage
- Redirect to login page (`/index.html`)

---

### CORS & Headers

**Required Headers:**
```
Content-Type: application/json
Authorization: Bearer {token}  (for authenticated endpoints)
```

**Expected CORS Configuration:**
- Allow origin: `http://localhost:5173` or `*` for development
- Allow methods: GET, POST, PUT, DELETE
- Allow credentials: true (for cookies if needed)

---

## 6. ERROR HANDLING

### API Response Codes

| Status | Meaning | Frontend Behavior |
|--------|---------|-------------------|
| 200 | Success | Process response data |
| 201 | Created | Treat as 200 |
| 204 | No Content | Return `null` |
| 400 | Bad Request | Show error toast: `response.message` |
| 401 | Unauthorized | Clear auth, redirect to login |
| 404 | Not Found | Show error toast |
| 500 | Server Error | Show generic error toast |

### Error Toast
```javascript
showToast(error.message, 'error')
// Displays for 3.5 seconds in top-right
```

---

## 7. DATA MODELS FOR BACKEND IMPLEMENTATION

### Usuario (User Profile)
```csharp
class Usuario
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string? LinkedIn { get; set; }
    public string? Portfolio { get; set; }
    public string? ResumoProfissional { get; set; }
    public List<string> Competencias { get; set; } = new();
    public List<string> SoftSkills { get; set; } = new();
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
    
    // Navigation
    public ICollection<Formacao> Formacoes { get; set; } = new List<Formacao>();
    public ICollection<Experiencia> Experiencias { get; set; } = new List<Experiencia>();
    public ICollection<Curriculo> Curriculos { get; set; } = new List<Curriculo>();
}
```

### Formacao (Education)
```csharp
class Formacao
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Instituicao { get; set; }
    public string Curso { get; set; }
    public string Tipo { get; set; } // "graduacao", "pos-graduacao", "tecnico", "livre"
    public DateTime DataInicio { get; set; }
    public DateTime? DataConclusao { get; set; }
    
    // Navigation
    public Usuario Usuario { get; set; }
}
```

### Experiencia (Work Experience)
```csharp
class Experiencia
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Empresa { get; set; }
    public string Cargo { get; set; }
    public bool EmpregoAtual { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string PrincipaisAtividades { get; set; }
    public List<string> TecnologiasUtilizadas { get; set; } = new();
    public string? ResultadosConquistas { get; set; }
    
    // Navigation
    public Usuario Usuario { get; set; }
}
```

### Curriculo (CV)
```csharp
class Curriculo
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Titulo { get; set; }
    public string DescricaoVaga { get; set; }
    public DateTime GeradoEm { get; set; }
    public int VersaoPerfilAncora { get; set; } // Profile version at generation time
    public bool EditadoManualmente { get; set; } // Computed from edit flags
    
    // Edit tracking
    public bool ResumoProfissionalEditado { get; set; }
    public bool ExperienciaEditada { get; set; }
    public bool FormacaoEditada { get; set; }
    public bool CompetenciasEditadas { get; set; }
    public bool SoftSkillsEditadas { get; set; }
    
    // JSON blob containing full CV content
    public string SecoesJson { get; set; } // Serialized CVSecoes object
    
    // Navigation
    public Usuario Usuario { get; set; }
}

// Nested class (serialized as JSON)
class CVSecoes
{
    public string ResumoProfissional { get; set; }
    public List<CVExperiencia> ExperienciaProfissional { get; set; }
    public List<CVFormacao> FormacaoAcademica { get; set; }
    public List<string> CompetenciasTecnicas { get; set; }
    public List<string> SoftSkills { get; set; }
}

class CVExperiencia
{
    public string Empresa { get; set; }
    public string Cargo { get; set; }
    public string Periodo { get; set; }
    public List<string> Atividades { get; set; }
    public string? Resultados { get; set; }
    public List<string> Tecnologias { get; set; }
}

class CVFormacao
{
    public string Curso { get; set; }
    public string Instituicao { get; set; }
    public string Tipo { get; set; }
    public string Periodo { get; set; }
}
```

---

## 8. DTOs (Data Transfer Objects)

### Request DTOs

#### SalvarPerfilDto
```csharp
public class SalvarPerfilDto
{
    public string NomeCompleto { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string? LinkedIn { get; set; }
    public string? Portfolio { get; set; }
    public string? ResumoProfissional { get; set; }
    public List<string> Competencias { get; set; } = new();
    public List<string> SoftSkills { get; set; } = new();
    public List<FormacaoDto> Formacoes { get; set; } = new();
    public List<ExperienciaDto> Experiencias { get; set; } = new();
}

public class FormacaoDto
{
    public string Instituicao { get; set; }
    public string Curso { get; set; }
    public string Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataConclusao { get; set; }
}

public class ExperienciaDto
{
    public string Empresa { get; set; }
    public string Cargo { get; set; }
    public bool EmpregoAtual { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string PrincipaisAtividades { get; set; }
    public List<string> TecnologiasUtilizadas { get; set; } = new();
    public string? ResultadosConquistas { get; set; }
}
```

#### GerarCVDto
```csharp
public class GerarCVDto
{
    public string DescricaoVaga { get; set; }
    public bool ConsentimentoIA { get; set; }
}
```

#### SalvarCVDto
```csharp
public class SalvarCVDto
{
    public string Titulo { get; set; }
    public string DescricaoVaga { get; set; }
    public CVSecoesDto Secoes { get; set; }
    public bool ResumoProfissionalEditado { get; set; }
    public bool ExperienciaEditada { get; set; }
    public bool FormacaoEditada { get; set; }
    public bool CompetenciasEditadas { get; set; }
    public bool SoftSkillsEditadas { get; set; }
}

public class CVSecoesDto
{
    public string ResumoProfissional { get; set; }
    public List<CVExperienciaDto> ExperienciaProfissional { get; set; }
    public List<CVFormacaoDto> FormacaoAcademica { get; set; }
    public List<string> CompetenciasTecnicas { get; set; }
    public List<string> SoftSkills { get; set; }
}

public class CVExperienciaDto
{
    public string Empresa { get; set; }
    public string Cargo { get; set; }
    public string Periodo { get; set; }
    public List<string> Atividades { get; set; }
    public string? Resultados { get; set; }
    public List<string> Tecnologias { get; set; }
}

public class CVFormacaoDto
{
    public string Curso { get; set; }
    public string Instituicao { get; set; }
    public string Tipo { get; set; }
    public string Periodo { get; set; }
}
```

---

## 9. API ENDPOINT SUMMARY

### Required Endpoints

| Endpoint | Method | Auth | Purpose |
|----------|--------|------|---------|
| `/api/perfil` | GET | Yes | Load user profile |
| `/api/perfil` | PUT | Yes | Save user profile |
| `/api/curriculos/gerar` | POST | Yes | Generate CV with AI |
| `/api/curriculos/salvar` | POST | Yes | Save CV to history |
| `/api/curriculos` | GET | Yes | List user's CVs |
| `/api/curriculos/{id}` | GET | Yes | Get CV details |

**Base URL:** `http://localhost:5000/api`

---

## 10. VALIDATION RULES

### Profile Validation

| Field | Rule |
|-------|------|
| `nomeCompleto` | Required, min 3 chars |
| `cidade` | Required |
| `estado` | Required, exactly 2 chars |
| `email` | Required, valid email, unique |
| `telefone` | Required, valid phone format |
| `linkedin` | Optional, valid URL if provided |
| `portfolio` | Optional, valid URL if provided |
| `resumo` | Optional, max 1000 chars |

### CV Title Validation
- Required
- Min 3 characters
- Max 100 characters

### Job Description Validation
- Required
- Min 20 characters
- Accepted by AI service

---

## 11. RESPONSE PATTERNS

### Success Response
```json
{
  "success": true,
  "message": "Operation completed",
  "data": {}
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "errors": {
    "fieldName": ["Validation error message"]
  }
}
```

### List Response
```json
[
  { "id": 1, ... },
  { "id": 2, ... }
]
```

---

## Summary

### Total Endpoints: 6
- Profile: 2 (GET, PUT)
- CV Generation: 1 (POST)
- CV Management: 3 (POST, GET list, GET detail)

### Key Models: 4
- Usuario (Profile)
- Formacao (Education)
- Experiencia (Work)
- Curriculo (CV)

### Total Form Fields: ~30+
- Profile Steps: 6
- CV Generation: 2
- CV Editing: 6 sections

### Data Volumes
- Each profile: ~2KB (excluding blobs)
- Each CV: ~5-10KB (depends on content length)
- User typical history: 3-10 CVs per person

---

**End of Document**

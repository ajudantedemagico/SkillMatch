# SkillMatch Frontend - Quick Reference Guide

## 🎯 Three Main Workflows

### 1️⃣ Profile Management
**Pages:** `perfil.html`  
**Steps:** 6 sequential steps with progress bar
**Endpoints:**
- `GET /api/perfil` → Load existing profile
- `PUT /api/perfil` → Save complete profile

---

### 2️⃣ CV Generation
**Pages:** `chat.html` → `cv-editar.html`  
**Flow:** 
1. User enters job description + consent
2. API calls OpenAI with user's profile
3. Returns AI-generated CV sections
4. User reviews/edits sections
5. Saves to history

**Endpoints:**
- `POST /api/curriculos/gerar` → Generate (requires consent)
- `POST /api/curriculos/salvar` → Save edited CV

---

### 3️⃣ CV History
**Pages:** `historico.html`  
**Features:** List all CVs, view full details in modal, track edit history

**Endpoints:**
- `GET /api/curriculos` → List user's CVs
- `GET /api/curriculos/{id}` → Get CV details

---

## 📝 Form Fields by Page

### Perfil.html (6-Step Form)

#### Step 1: Personal Data
```
nome (required)
cidade (required)
estado (required, 2 chars max)
email (required, auto-filled from auth)
telefone (required)
linkedin (optional)
portfolio (optional)
```

#### Step 2: Education
```
Multiple entries:
  - instituicao
  - curso
  - tipo (dropdown: graduacao|pos-graduacao|tecnico|livre)
  - dataInicio (month-year)
  - dataConclusao (month-year, optional)
```

#### Step 3: Work Experience
```
Multiple entries:
  - empresa
  - cargo
  - empregoAtual (checkbox, hides "fim" if true)
  - dataInicio (month-year)
  - dataFim (month-year, optional if current)
  - principaisAtividades (textarea)
  - tecnologiasUtilizadas (comma-separated list)
  - resultadosConquistas (textarea, optional)
```

#### Step 4: Technical Skills
```
Array of tags:
  competencias = ["JavaScript", "C#", "SQL", ...]
```

#### Step 5: Professional Objective
```
resumoProfissional (textarea, optional)
```

#### Step 6: Soft Skills
```
Array of tags:
  softSkills = ["Communication", "Leadership", ...]
Quick suggestions provided
```

---

### Chat.html (CV Generation)

```
descricaoVaga (textarea, required)
consentimentoIA (checkbox, required)
```

---

### CV-Editar.html (CV Review & Edit)

```
cv-titulo (text, required)

Editable sections:
  - resumoProfissional (textarea)
  - competenciasTecnicas (tags)
  - softSkills (tags)
  - experienciaProfissional.atividades (textareas)

Read-only sections:
  - formacaoAcademica
```

Edit flags tracked:
```
resumoProfissionalEditado
experienciaEditada
formacaoEditada
competenciasEditadas
softSkillsEditadas
```

---

## 📡 API Endpoint Checklist

### Authentication
All endpoints require: `Authorization: Bearer {jwt_token}`

### Profile
- [ ] `GET /api/perfil` → Returns Usuario object or 204
- [ ] `PUT /api/perfil` → Saves complete profile

### CV Generation
- [ ] `POST /api/curriculos/gerar` → Returns CVSecoes object
  - Input: `descricaoVaga`, `consentimentoIA`
  - Uses OpenAI API internally

### CV Management
- [ ] `POST /api/curriculos/salvar` → Saves CV to history
  - Input: titulo, descricaoVaga, secoes, edit flags
  - Response: `{success, id}`

- [ ] `GET /api/curriculos` → Returns array of CV summaries
  - Fields: id, titulo, geradoEm, versaoPerfilAncora, descricaoVaga, editadoManualmente

- [ ] `GET /api/curriculos/{id}` → Returns full CV object
  - Includes all sections and metadata

---

## 🗄️ Database Models

### Usuario
```
id (PK)
nomeCompleto
email (unique, index)
telefone
cidade
estado
linkedin (nullable)
portfolio (nullable)
resumoProfissional (nullable)
competencias (JSON array)
softSkills (JSON array)
criadoEm
atualizadoEm
```

Foreign Keys:
- Formacao (1:N)
- Experiencia (1:N)
- Curriculo (1:N)

---

### Formacao (Education)
```
id (PK)
usuarioId (FK)
instituicao
curso
tipo
dataInicio
dataConclusao (nullable)
```

---

### Experiencia (Work)
```
id (PK)
usuarioId (FK)
empresa
cargo
empregoAtual
dataInicio
dataFim (nullable)
principaisAtividades
tecnologiasUtilizadas (JSON array)
resultadosConquistas (nullable)
```

---

### Curriculo (CV)
```
id (PK)
usuarioId (FK)
titulo
descricaoVaga
geradoEm
versaoPerfilAncora (profile version snapshot)
editadoManualmente (computed boolean)
resumoProfissionalEditado
experienciaEditada
formacaoEditada
competenciasEditadas
softSkillsEditadas
secoesJson (large text - serialized CVSecoes)
```

**secoesJson structure:**
```json
{
  "resumoProfissional": "string",
  "experienciaProfissional": [
    {
      "empresa": "string",
      "cargo": "string",
      "periodo": "string",
      "atividades": ["string"],
      "resultados": "string?",
      "tecnologias": ["string"]
    }
  ],
  "formacaoAcademica": [
    {
      "curso": "string",
      "instituicao": "string",
      "tipo": "string",
      "periodo": "string"
    }
  ],
  "competenciasTecnicas": ["string"],
  "softSkills": ["string"]
}
```

---

## 🔐 Security & Auth

### Headers Required
```
Authorization: Bearer {jwt_token}
Content-Type: application/json
```

### Token Storage (Frontend)
```javascript
localStorage.getItem('sm_token')      // JWT
localStorage.getItem('sm_user')       // {id, nome}
```

### Error Handling
- `401` → Clear token, redirect to login
- `400/422` → Show error message from response
- `500` → Show generic error
- `204` → Return null

---

## 🔄 Data Flow Examples

### Profile Save Flow
```
User fills 6-step form
  → Click "Salvar Perfil"
  → Collect all data into DTO
  → PUT /api/perfil
  → Show success toast
  → Redirect to app.html
```

### CV Generation Flow
```
Chat page:
  User enters job description + consent
  → POST /api/curriculos/gerar
  → Save response to sessionStorage
  → Navigate to cv-editar.html

Edit page:
  Load from sessionStorage
  User edits sections + sets flags
  → Click "Salvar no histórico"
  → POST /api/curriculos/salvar
  → Clear sessionStorage
  → Redirect to historico.html
```

### CV Viewing Flow
```
History page:
  → GET /api/curriculos
  → Display list
  User clicks CV
  → GET /api/curriculos/{id}
  → Display full CV in modal
```

---

## 📊 Key Statistics

| Item | Count |
|------|-------|
| Total API endpoints | 6 |
| Database tables | 4 |
| Form steps | 6 |
| HTML pages | 4 |
| Edit-tracked flags | 5 |
| CV sections | 5 |
| Profile sections | 6 |

---

## ✅ Implementation Checklist

### Models & Migrations
- [ ] Create Usuario model
- [ ] Create Formacao model
- [ ] Create Experiencia model
- [ ] Create Curriculo model
- [ ] Add DbSets to SkillMatchContext
- [ ] Create and run migrations

### Controllers
- [ ] Create PerfilController
  - [ ] GET /api/perfil
  - [ ] PUT /api/perfil

### Services
- [ ] Create PerfilService
  - [ ] GetPerfil()
  - [ ] SalvarPerfil()
- [ ] Create CVService
  - [ ] GerarCV() - integrate with OpenAI
  - [ ] SalvarCV()
  - [ ] ListarCVs()
  - [ ] ObterCV(id)

### Controllers (continued)
- [ ] Create CurriculoController
  - [ ] POST /api/curriculos/gerar
  - [ ] POST /api/curriculos/salvar
  - [ ] GET /api/curriculos
  - [ ] GET /api/curriculos/{id}

### Validation
- [ ] Profile field validation
- [ ] CV title validation
- [ ] Job description validation
- [ ] User authorization checks

### Testing
- [ ] Unit tests for services
- [ ] Integration tests for endpoints
- [ ] Manual testing with frontend

---

## 🎨 Frontend Features to Support

### UI Patterns
- Multi-step form with progress bar
- Editable tag lists
- Card-based list displays
- Modal dialogs
- Toast notifications
- Loading overlays
- Edit tracking badges

### Keyboard Shortcuts
- `Enter` key to add tags
- `Escape` to close modals (implicit)

### Responsive Design
- Mobile-first (forms stack vertically)
- Fixed bottom navigation
- Scrollable content area with bottom nav padding

---

## 📞 Support for Future Features

The current design supports:
- ✅ Multiple CVs per user (history)
- ✅ Profile versioning (tracked per CV)
- ✅ Edit tracking (manual vs AI)
- ✅ AI content generation
- ✅ Future: PDF export
- ✅ Future: Email CV
- ✅ Future: Job matching

---

**Document created:** April 27, 2026  
**Last updated:** Latest frontend analysis complete

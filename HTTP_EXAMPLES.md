# SkillMatch - HTTP Request/Response Examples

This document shows real examples of all HTTP requests and responses for the 6 required endpoints.

---

## 1. GET /api/perfil

### Purpose
Load existing user profile for editing

### Request
```http
GET /api/perfil HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```

### Success Response (200 OK)
```json
{
  "nomeCompleto": "João Silva",
  "cidade": "São Paulo",
  "estado": "SP",
  "email": "joao@example.com",
  "telefone": "(11) 98765-4321",
  "linkedin": "linkedin.com/in/joaosilva",
  "portfolio": "github.com/joaosilva",
  "resumoProfissional": "Backend developer with 5+ years of experience in C# and .NET. Specialized in REST APIs, microservices, and cloud architectures. Passionate about clean code and mentoring junior developers.",
  "competencias": [
    "C#",
    "NET",
    "SQL Server",
    "Docker",
    "Entity Framework",
    "REST APIs",
    "Microservices"
  ],
  "softSkills": [
    "Communication",
    "Leadership",
    "Problem-solving",
    "Team collaboration"
  ],
  "formacoes": [
    {
      "instituicao": "Universidade São Paulo",
      "curso": "Sistemas de Informação",
      "tipo": "graduacao",
      "dataInicio": "2018-03-01T00:00:00Z",
      "dataConclusao": "2022-12-01T00:00:00Z"
    },
    {
      "instituicao": "Coursera",
      "curso": "Cloud Architecture with AWS",
      "tipo": "livre",
      "dataInicio": "2023-01-01T00:00:00Z",
      "dataConclusao": null
    }
  ],
  "experiencias": [
    {
      "empresa": "Tech Corp",
      "cargo": "Senior Backend Developer",
      "empregoAtual": false,
      "dataInicio": "2020-01-01T00:00:00Z",
      "dataFim": "2022-12-31T00:00:00Z",
      "principaisAtividades": "Developed and maintained REST APIs using C# and .NET Core. Designed and implemented database schemas using Entity Framework. Optimized database queries improving system performance by 40%. Mentored 3 junior developers.",
      "tecnologiasUtilizadas": [
        "C#",
        "NET Core",
        "SQL Server",
        "Entity Framework",
        "Docker"
      ],
      "resultadosConquistas": "Reduced API response time from 500ms to 100ms. Led migration of monolith to microservices."
    },
    {
      "empresa": "Digital Solutions Inc",
      "cargo": "Backend Developer",
      "empregoAtual": true,
      "dataInicio": "2023-01-01T00:00:00Z",
      "dataFim": null,
      "principaisAtividades": "Build and maintain microservices. Implement CI/CD pipelines. Collaborate with frontend team. Code reviews and testing.",
      "tecnologiasUtilizadas": [
        "C#",
        "NET",
        "PostgreSQL",
        "Docker",
        "Kubernetes"
      ],
      "resultadosConquistas": null
    }
  ]
}
```

### No Profile (204 No Content)
```
HTTP/1.1 204 No Content
```

### Error Response (401 Unauthorized)
```http
HTTP/1.1 401 Unauthorized
Content-Type: application/json

{
  "message": "Invalid or expired token"
}
```

---

## 2. PUT /api/perfil

### Purpose
Save or update user's complete profile

### Request
```http
PUT /api/perfil HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "nomeCompleto": "João Silva",
  "cidade": "São Paulo",
  "estado": "SP",
  "email": "joao@example.com",
  "telefone": "(11) 98765-4321",
  "linkedin": "linkedin.com/in/joaosilva",
  "portfolio": "github.com/joaosilva",
  "resumoProfissional": "Backend developer with 5+ years of experience...",
  "competencias": [
    "C#",
    "NET",
    "SQL Server",
    "Docker",
    "Entity Framework"
  ],
  "softSkills": [
    "Communication",
    "Leadership",
    "Problem-solving"
  ],
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
      "tecnologiasUtilizadas": [
        "C#",
        "NET Core",
        "SQL Server"
      ],
      "resultadosConquistas": "Improved API performance by 40%"
    },
    {
      "empresa": "Digital Solutions",
      "cargo": "Backend Developer",
      "empregoAtual": true,
      "dataInicio": "2023-01-01T00:00:00Z",
      "dataFim": null,
      "principaisAtividades": "Build microservices...",
      "tecnologiasUtilizadas": [
        "C#",
        "NET",
        "PostgreSQL"
      ],
      "resultadosConquistas": null
    }
  ]
}
```

### Success Response (200 OK)
```json
{
  "success": true,
  "message": "Perfil salvo com sucesso"
}
```

### Error Response (400 Bad Request)
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "nomeCompleto": ["Nome é obrigatório"],
    "email": ["Email deve ser único"]
  }
}
```

---

## 3. POST /api/curriculos/gerar

### Purpose
Generate CV sections using AI based on job description

### Request
```http
POST /api/curriculos/gerar HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "descricaoVaga": "Buscamos Desenvolvedor(a) Back-end C# para integrar nosso time de 8 pessoas. A posição é permanente em regime híbrido (2 dias presencial SP). Requisitos: Experiência com .NET 6+, Entity Framework, REST APIs, SQL Server. Conhecimento em Docker é diferencial. Salário: R$ 12k-15k.",
  "consentimentoIA": true
}
```

### Success Response (200 OK)
```json
{
  "resumoProfissional": "Desenvolvedor Back-end C# com 5+ anos de experiência em desenvolvimento de aplicações .NET. Especializado em criação de APIs REST robustas, design de arquitetura de microserviços e otimização de performance. Profundo conhecimento em Entity Framework, SQL Server e Docker. Histórico comprovado de entregar soluções de alta qualidade que melhoram a performance do sistema.",
  "experienciaProfissional": [
    {
      "empresa": "Tech Corp",
      "cargo": "Senior Backend Developer",
      "periodo": "Jan 2020 - Dez 2022",
      "atividades": [
        "Desenvolveu e manteve múltiplas REST APIs em C# e .NET Core utilizadas por mais de 100k usuários",
        "Projetou e implementou schemas de banco de dados complexos usando Entity Framework com otimizações avançadas",
        "Reduziu tempo de resposta das APIs de 500ms para 100ms através de otimizações de query e caching",
        "Mentorizou 3 desenvolvedores juniores em boas práticas de código e arquitetura",
        "Implementou CI/CD pipelines usando Docker e GitHub Actions"
      ],
      "resultados": "Liderou migração bem-sucedida de monolito para arquitetura de microserviços. Sistema melhorou performance em 40% e reduzir tempo de deploy de 2 horas para 15 minutos.",
      "tecnologias": [
        "C#",
        "NET Core",
        "SQL Server",
        "Entity Framework",
        "Docker",
        "REST APIs"
      ]
    },
    {
      "empresa": "Digital Solutions Inc",
      "cargo": "Backend Developer",
      "periodo": "Jan 2023 - Atualmente",
      "atividades": [
        "Desenvolve microserviços em .NET que processam 1M+ requisições diárias",
        "Implementa e mantém bancos de dados SQL Server e PostgreSQL com otimizações de índice",
        "Contribui para pipelines de CI/CD com Docker e Kubernetes",
        "Realiza code reviews e testes automatizados para garantir qualidade"
      ],
      "resultados": null,
      "tecnologias": [
        "C#",
        "NET",
        "SQL Server",
        "PostgreSQL",
        "Docker",
        "Kubernetes"
      ]
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
  "competenciasTecnicas": [
    "C#",
    "NET",
    "NET Core",
    "Entity Framework",
    "SQL Server",
    "REST APIs",
    "Docker",
    "Microservices",
    "PostgreSQL"
  ],
  "softSkills": [
    "Communication",
    "Leadership",
    "Problem-solving",
    "Team collaboration",
    "Mentoring"
  ]
}
```

### Error Response (400 Bad Request)
```json
{
  "success": false,
  "message": "Job description is too short (minimum 20 characters)"
}
```

### Error Response (401 Unauthorized)
```json
{
  "message": "Invalid or expired token"
}
```

### Error Response (404 Not Found)
```json
{
  "message": "User profile not found. Please complete your profile first."
}
```

---

## 4. POST /api/curriculos/salvar

### Purpose
Save the AI-generated (or edited) CV to user's history

### Request
```http
POST /api/curriculos/salvar HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "titulo": "CV Dev Backend - Tech Corp",
  "descricaoVaga": "Buscamos Desenvolvedor(a) Back-end C# para integrar nosso time...",
  "secoes": {
    "resumoProfissional": "Desenvolvedor Back-end C# com 5+ anos de experiência...",
    "experienciaProfissional": [
      {
        "empresa": "Tech Corp",
        "cargo": "Senior Backend Developer",
        "periodo": "Jan 2020 - Dez 2022",
        "atividades": [
          "Desenvolveu REST APIs em C# e .NET Core",
          "Otimizou queries de banco de dados",
          "Mentorizou desenvolvedores juniores"
        ],
        "resultados": "Melhorou performance em 40%",
        "tecnologias": [
          "C#",
          "NET Core",
          "SQL Server",
          "Docker"
        ]
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
    "competenciasTecnicas": [
      "C#",
      "NET",
      "SQL Server",
      "Docker",
      "Entity Framework"
    ],
    "softSkills": [
      "Communication",
      "Leadership",
      "Problem-solving"
    ]
  },
  "resumoProfissionalEditado": false,
  "experienciaEditada": true,
  "formacaoEditada": false,
  "competenciasEditadas": false,
  "softSkillsEditadas": false
}
```

### Success Response (200 OK)
```json
{
  "success": true,
  "id": 42,
  "message": "CV salvo no histórico"
}
```

### Error Response (400 Bad Request)
```json
{
  "success": false,
  "message": "CV title is required and must be between 3 and 100 characters"
}
```

---

## 5. GET /api/curriculos

### Purpose
List all CVs created by the authenticated user

### Request
```http
GET /api/curriculos HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```

### Success Response (200 OK) - With CVs
```json
[
  {
    "id": 1,
    "titulo": "CV Dev Backend - Tech Corp",
    "geradoEm": "2024-04-25T10:30:00Z",
    "versaoPerfilAncora": 1,
    "descricaoVaga": "Buscamos Desenvolvedor(a) Back-end C# para integrar nosso time...",
    "editadoManualmente": true
  },
  {
    "id": 2,
    "titulo": "CV Dev Full Stack - StartUp XYZ",
    "geradoEm": "2024-04-20T14:15:00Z",
    "versaoPerfilAncora": 1,
    "descricaoVaga": "Procuramos Full Stack Developer com React, Node.js, PostgreSQL...",
    "editadoManualmente": false
  },
  {
    "id": 3,
    "titulo": "CV Tech Lead - Enterprise Corp",
    "geradoEm": "2024-04-10T09:45:00Z",
    "versaoPerfilAncora": 1,
    "descricaoVaga": "Buscamos Tech Lead com experiência em arquitetura de sistemas...",
    "editadoManualmente": true
  }
]
```

### Success Response (200 OK) - Empty List
```json
[]
```

### Error Response (401 Unauthorized)
```json
{
  "message": "Invalid or expired token"
}
```

---

## 6. GET /api/curriculos/{id}

### Purpose
Get full CV details for viewing in modal

### Request
```http
GET /api/curriculos/1 HTTP/1.1
Host: localhost:5000
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```

### Success Response (200 OK)
```json
{
  "id": 1,
  "titulo": "CV Dev Backend - Tech Corp",
  "geradoEm": "2024-04-25T10:30:00Z",
  "versaoPerfilAncora": 1,
  "descricaoVaga": "Buscamos Desenvolvedor(a) Back-end C# para integrar nosso time...",
  "editadoManualmente": true,
  "secoes": {
    "resumoProfissional": "Desenvolvedor Back-end C# com 5+ anos de experiência em desenvolvimento de aplicações .NET. Especializado em criação de APIs REST robustas, design de arquitetura de microserviços e otimização de performance.",
    "experienciaProfissional": [
      {
        "empresa": "Tech Corp",
        "cargo": "Senior Backend Developer",
        "periodo": "Jan 2020 - Dez 2022",
        "atividades": [
          "Desenvolveu e manteve múltiplas REST APIs em C# e .NET Core",
          "Projetou e implementou schemas de banco de dados complexos",
          "Reduziu tempo de resposta das APIs de 500ms para 100ms",
          "Mentorizou 3 desenvolvedores juniores"
        ],
        "resultados": "Liderou migração bem-sucedida de monolito para microserviços",
        "tecnologias": [
          "C#",
          "NET Core",
          "SQL Server",
          "Entity Framework",
          "Docker"
        ]
      },
      {
        "empresa": "Digital Solutions Inc",
        "cargo": "Backend Developer",
        "periodo": "Jan 2023 - Atualmente",
        "atividades": [
          "Desenvolve microserviços em .NET",
          "Implementa e mantém bancos de dados",
          "Contribui para pipelines de CI/CD",
          "Realiza code reviews e testes automatizados"
        ],
        "resultados": null,
        "tecnologias": [
          "C#",
          "NET",
          "SQL Server",
          "PostgreSQL",
          "Docker",
          "Kubernetes"
        ]
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
    "competenciasTecnicas": [
      "C#",
      "NET",
      "SQL Server",
      "Docker",
      "Entity Framework",
      "REST APIs",
      "Microservices"
    ],
    "softSkills": [
      "Communication",
      "Leadership",
      "Problem-solving",
      "Team collaboration",
      "Mentoring"
    ]
  }
}
```

### Error Response (404 Not Found)
```json
{
  "message": "CV not found"
}
```

### Error Response (403 Forbidden)
```json
{
  "message": "You do not have permission to view this CV"
}
```

---

## Common HTTP Headers

### All Requests Should Include
```
Content-Type: application/json
Authorization: Bearer {jwt_token}
User-Agent: SkillMatch/1.0
```

### All Responses Include
```
Content-Type: application/json
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
```

---

## Status Codes Summary

| Status | Meaning | Endpoint(s) |
|--------|---------|-------------|
| 200 | OK - Success | All GET/PUT/POST |
| 201 | Created | (Could use for POST save CV) |
| 204 | No Content | GET /perfil (first time) |
| 400 | Bad Request | PUT/POST with invalid data |
| 401 | Unauthorized | No/invalid token |
| 403 | Forbidden | CV belongs to other user |
| 404 | Not Found | CV/Profile doesn't exist |
| 422 | Unprocessable Entity | Validation errors |
| 500 | Server Error | Unhandled exception |

---

## Tips for Implementation

1. **Token Validation:** Check JWT token on every request (except auth endpoints)
2. **User Context:** Extract `userId` from JWT claims to filter data
3. **Timestamps:** Always use ISO 8601 format for dates
4. **Nulls:** Use `null` for optional fields, not empty strings
5. **Arrays:** Use actual arrays `[]`, not comma-separated strings
6. **Errors:** Include `message` field for all error responses
7. **Sorting:** List endpoints should sort by most recent first (`geradoEm DESC`)

---

**Document created:** April 27, 2026

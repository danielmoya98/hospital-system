# 🏥 HospitalSystem

Sistema de gestión hospitalaria desarrollado con una arquitectura moderna basada en **ASP.NET Core** y **Angular**, siguiendo buenas prácticas de arquitectura limpia (Clean Architecture), separación por capas y una interfaz moderna.

## ✨ Características

- 🔐 Autenticación mediante JWT
- 👥 Gestión de usuarios y roles
- 🩺 Gestión de pacientes
- 👨‍⚕️ Gestión de médicos
- 📅 Gestión de citas médicas
- 🏥 Gestión de especialidades
- 📋 Historial clínico
- 📊 Dashboard administrativo
- 🌐 API REST
- 📱 Frontend responsive

---

# 🏗 Arquitectura

```
HospitalSystem
│
├── backend
│   ├── HospitalSystem.Api
│   ├── HospitalSystem.Application
│   ├── HospitalSystem.Domain
│   ├── HospitalSystem.Infrastructure
│   └── HospitalSystem.sln
│
├── frontend
│   └── hospital-ui
│
├── docker
│
└── infrastructure
```

---

# 🚀 Tecnologías

## Backend

- ASP.NET Core 10
- Entity Framework Core
- SQL Server / PostgreSQL
- JWT Authentication
- Clean Architecture
- Dependency Injection
- Swagger / OpenAPI

## Frontend

- Angular 22
- TypeScript
- SCSS
- RxJS
- Angular Router
- Angular Signals
- HttpClient
- Tailwind CSS

---

# 📁 Estructura del proyecto

## Backend

```
backend/
│
├── HospitalSystem.Api
├── HospitalSystem.Application
├── HospitalSystem.Domain
├── HospitalSystem.Infrastructure
└── HospitalSystem.sln
```

## Frontend

```
frontend/
│
└── hospital-ui
```

---

# ⚙ Requisitos

- .NET SDK 10
- Node.js 22+
- npm
- Angular CLI
- Git

---

# ▶ Ejecutar el Backend

```bash
cd backend
dotnet restore
dotnet run --project HospitalSystem.Api
```

La API estará disponible en:

```
https://localhost:5001
```

o

```
http://localhost:5000
```

(según la configuración del proyecto)

---

# ▶ Ejecutar el Frontend

```bash
cd frontend
npm install
npm start
```

Angular estará disponible en:

```
http://localhost:4200
```

---

# 📦 Compilar

## Backend

```bash
cd backend
dotnet publish -c Release
```

## Frontend

```bash
cd frontend
npm run build
```

---

# 🚀 Despliegue

## Backend

- Render

## Frontend

- Cloudflare Pages

## Base de datos

- PostgreSQL

---

# 🔒 Variables de entorno

El proyecto utiliza archivos de configuración para los distintos entornos.

Backend

```
appsettings.json
appsettings.Development.json
```

Frontend

```
environment.ts
environment.development.ts
```

---

# 📌 Estado del proyecto

🚧 En desarrollo.

---

# 📄 Licencia

Este proyecto se distribuye únicamente con fines educativos y de portafolio.

---

# 👨‍💻 Autor

**Daniel Moya**

Bolivia 🇧🇴

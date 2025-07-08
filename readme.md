# 🏦 BancaAPI

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Build](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/TU_USUARIO/BancaAPI/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**BancaAPI** es una API RESTful desarrollada con ASP.NET Core 8 y Clean Architecture para la gestión profesional de cuentas bancarias, clientes, transacciones (depósitos/retiros) y consultas de saldo. El proyecto sigue principios SOLID y buenas prácticas de ingeniería de software.

---

## 📌 Características principales

- Creación y gestión de cuentas bancarias
- Consulta de saldos en tiempo real
- Depósitos y retiros con validaciones
- Registro y consulta de historial de transacciones
- Arquitectura limpia (Clean Architecture)
- Pruebas unitarias con xUnit, Moq y FluentAssertions
- Documentación interactiva con Swagger

---

## ⚙️ Requisitos

| Requisito                | Versión recomendada         |
|--------------------------|----------------------------|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0) | 8.0.411 o superior           |
| Visual Studio 2022+ o VS Code | Última disponible              |

> El archivo `global.json` asegura la compatibilidad con .NET 8.

---

## 🚀 Instalación y ejecución

1. **Clona el repositorio:**
   ```bash
   git clone https://github.com/cvargas0503/BancaAPI.git
   cd BancaAPI
   ```
2. **Restaura los paquetes:**
   ```bash
   dotnet restore
   ```
3. **Ejecuta la API:**
   ```bash
   cd src/BancaAPI.API
   dotnet run
   ```
4. **Accede a la documentación Swagger:**
   [http://localhost:5155/swagger](http://localhost:5155/swagger)

---

## 🧪 Pruebas unitarias

1. Desde la raíz del proyecto:
   ```bash
   dotnet test
   ```
2. Para mayor detalle en consola:
   ```bash
   dotnet test --logger "console;verbosity=detailed"
   ```
3. Las pruebas están ubicadas en: `tests/BancaAPI.Tests/Unit/CuentaServiceTests.cs`

   Incluyen:
   - Creación de cuenta
   - Retiros con saldo suficiente e insuficiente (esperando excepción)
   - Depósitos válidos e inválidos
   - Consulta de saldo
   - Historial de transacciones
   - Otros escenarios relevantes

---

## 📁 Estructura del Proyecto

| Carpeta/Archivo              | Descripción                                 |
|------------------------------|---------------------------------------------|
| `global.json`                | Fuerza uso de .NET 8                        |
| `BancaAPI-Test.sln`          | Solución principal                          |
| `src/BancaAPI.API`           | API REST principal                          |
| `src/BancaAPI.Application`   | Servicios, DTOs, validaciones               |
| `src/BancaAPI.Domain`        | Entidades y enums del dominio               |
| `src/BancaAPI.Infrastructure`| EF Core, DBContext, repositorios            |
| `tests/BancaAPI.Tests`       | Pruebas unitarias                           |

---

## 📚 Endpoints principales

La documentación completa y pruebas de los endpoints están disponibles en Swagger. Algunos endpoints destacados:

- `POST /api/Cuenta` — Crear nueva cuenta bancaria
- `GET /api/Cuenta/{numeroCuenta}/saldo` — Consultar saldo de cuenta
- `POST /api/Cuenta/deposito` — Realizar depósito
- `POST /api/Cuenta/retiro` — Realizar retiro
- `GET /api/Cuenta/{numeroCuenta}/transacciones` — Ver historial de transacciones

---

## 🧑‍💻 Autor

Carlos Vargas  
Desarrollado con ❤️ en .NET 8


## 📄 Licencia

MIT © 2025 — Puedes usar, modificar y distribuir libremente este código.



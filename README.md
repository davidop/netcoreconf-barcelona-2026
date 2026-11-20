# 🪄 El aprendiz de agente — NetCoreConf Barcelona 2026

**Magia, telemetría y gobernanza bajo control** · Érika Cepeda & David Oliva Paredes · 45 minutos

> **Estado:** implementación incremental en curso. Los flujos deterministas `SIMULATED` y `REPLAY` permiten ensayar sin Azure. Las integraciones `LIVE` con Microsoft Foundry, Copilot Studio, Entra ID, Teams y Azure **no están validadas ni habilitadas**.

[![CI](https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/ci.yml/badge.svg)](https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/ci.yml)

## La historia

Dos maestros, dos visiones: **Érika** defiende seguridad, gobernanza y calidad; **David** persigue automatización, rendimiento y optimización. **MerlinOps**, un aprendiz de mago tecnológico 3D, representa al supervisor multiagente. Los **Guardianes de Azure** analizan telemetría, costes, calidad y seguridad; ninguna propuesta de cambio sortea los Quality Gates.

| Escena | Modo escénico previsto | Qué demuestra |
| --- | --- | --- |
| 1. La optimización | LIVE (con Replay preparado) | Análisis multiagente de modelos y costes Microsoft Foundry, calidad y aprobación |
| 2. El escudo del guardián | LIVE (con Replay preparado) | Propuesta insegura bloqueada por Policy-as-Code |
| 3. El hechizo que salió mal | REPLAY | Degradación posterior, Quality Gates y auto-rollback preautorizado |

Los modos de ejecución **LIVE**, **REPLAY** y **SIMULATED** deben aparecer siempre identificados. No se representará una simulación como prueba real de Azure ni ahorro facturado.

## Diseño técnico

- **Runtime:** .NET 10 + Microsoft Agent Framework (SDK/compatibilidad revisados y fijados).
- **Desarrollo:** monorepo .NET Aspire, SQLite; persistencia cloud futura Azure Table Storage + Blob Storage mediante contratos.
- **Presentación:** Slidev + Vue; Mission Control integrado, SSE de eventos de solo lectura, REST protegido para operaciones, Replay determinista con Event Sourcing + trazas OpenTelemetry correlacionadas.
- **Agentes:** supervisor MerlinOps; especialistas de observabilidad, FinOps, calidad y gobernanza. Delegación paralela, evaluaciones, aprobaciones humanas y rollback.
- **Seguridad:** default-deny, mínimo privilegio, identidad Entra y RBAC antes de LIVE, políticas YAML versionadas y ruta futura a OPA/Rego. Revisión humana antes de publicar trazas anonimizadas.
- **Infraestructura:** Terraform y GitHub Actions preparados pero **sin auto-apply**. Objetivo de gasto estimado ≤ 115 €/mes; no supone un límite absoluto de facturación.
- **Calidad:** tests deterministas, Quality Gates, accesibilidad, seguridad, Replay offline, Playwright y Conference Readiness Gates.
- **Open source:** MIT para el código original; las marcas y logotipos de terceros conservan sus derechos.

## Primer arranque (sin Azure)

```bash
# API .NET 10 — una vez instalado el SDK 10
dotnet run --project src/MerlinOps.Api

# Presentación — terminal independiente
cd slides
npm install
npm run dev
```

Una vez construido el proyecto, las rutas públicas de ejemplo serán `GET /api/scenarios`, `POST /api/runs/simulate`, `GET /api/runs/{id}`, `GET /api/runs/{id}/events` y `GET /api/runs/{id}/stream`. El backend no habilitará escrituras Azure por defecto.

## Organización

- `src/`: API y motor de simulación, eventos y políticas.
- `slides/`: Slidev, Mission Control y narrativa.
- `tests/`: tests de contratos, políticas y escenarios.
- `infra/`: Terraform para entornos opcionales y protegidos.
- `docs/`: arquitectura, ADR, decisiones, demo y evidencias.
- `policies/`, `replay/`, `skills/`: controles, datasets y agentes de desarrollo.
- `.github/workflows/`: CI y publicación segura.

Consulta [docs/architecture.md](docs/architecture.md), [docs/decisions.md](docs/decisions.md) y [docs/demo-runbook.md](docs/demo-runbook.md) para preparar la demo. Los vínculos se incorporarán conforme avance la implementación.

## Fuente de verdad

Este repositorio documenta **78 decisiones de diseño** acordadas; una decisión no equivale a funcionalidad implementada. Se diferencia explícitamente entre **implementado**, **probado**, **simulado**, **pendiente de integración** y **no autorizado para desplegar**.

**No se crearán ni modificarán recursos Azure sin autorización explícita posterior.**

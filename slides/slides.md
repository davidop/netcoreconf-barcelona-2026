---
theme: default
title: "El aprendiz de agente · NetCoreConf Barcelona 2026"
info: |
  Magia, telemetría y gobernanza bajo control · Érika Cepeda y David Oliva Paredes
layout: cover
class: title-screen
---

<div class="eyebrow">NETCORECONF BARCELONA 2026 · 45 MINUTOS</div>

# El aprendiz<br>de agente <span class="wand">🪄</span>

### Magia, telemetría y gobernanza bajo control

<p class="lead">Microsoft Agent Framework · Microsoft Foundry · Copilot Studio · Azure · Slidev</p>

<div class="speakers">Érika Cepeda <span>×</span> David Oliva Paredes</div>

<!-- Érika abre con el conflicto de seguridad; David contrapone la autonomía. -->

---
layout: default
---

# Dos maestros. Dos visiones.

<div class="duel">
  <div class="duel-pane">
    <div class="role">ÉRIKA · LA GUARDIANA</div>
    <h2>¿Y quién aprueba ese cambio?</h2>
    <p>Seguridad, calidad, cumplimiento, auditoría y control humano.</p>
  </div>
  <div class="duel-pane">
    <div class="role">DAVID · EL OPTIMIZADOR</div>
    <h2>¿Y si dejamos que los agentes colaboren?</h2>
    <p>Automatización, rendimiento, reducción del coste y velocidad.</p>
  </div>
</div>

<div class="slide-caption">El reto: automatizar sin perder el control.</div>

<!-- Diálogo alternado. No público interactuando con la infraestructura. -->

---
layout: default
---

# Presentamos a MerlinOps

<div class="mascot-layout">
  <MerlinOps3D status="planning" />
  <div>
    <h2>Un aprendiz. Cuatro guardianes.</h2>
    <p>El supervisor coordina especialistas en telemetría, FinOps, calidad y gobernanza.</p>
    <p><strong>No decide por encima de las políticas.</strong> Propone, mide, explica y pide aprobación.</p>
    <div class="tag-line">EL PERSONAJE REPRESENTA EVENTOS TÉCNICOS, NO UNA IA INFALIBLE</div>
  </div>
</div>

---
layout: default
---

# La arquitectura detrás del hechizo

<div class="architecture">
  <div class="arch-node high">Slidev · Mission Control<br><small>LIVE / REPLAY / SIMULATED</small></div>
  <div class="arch-arrow">↓ SSE / REST</div>
  <div class="arch-node">ASP.NET Core .NET 10<br><small>API, Event Store, auditoría, políticas</small></div>
  <div class="arch-arrow">↓</div>
  <div class="arch-node accent">MerlinOps · Microsoft Agent Framework<br><small>Planificación y delegación especializada</small></div>
  <div class="arch-grid">
    <div>📈 Telemetry</div><div>💶 FinOps</div><div>🧪 Quality</div><div>🛡️ Governance</div>
  </div>
  <div class="arch-arrow">↓ gates deterministas + aprobación humana</div>
  <div class="arch-node">Foundry · Copilot Studio · Teams · Terraform<br><small>Conectores LIVE sujetos a autorización y validación</small></div>
</div>

<!-- Las rutas LIVE no se han comprobado. La demo local utiliza adaptadores deterministas. -->

---
layout: default
---

# Los tres hechizos

<div class="triple">
  <div class="scenario-card"><span>01</span><h2>El ahorro</h2><p>Optimización de modelos con límites de costes, latencia y calidad.</p><strong>LIVE previsto</strong></div>
  <div class="scenario-card"><span>02</span><h2>El escudo</h2><p>Una propuesta arriesgada choca contra las políticas.</p><strong>LIVE previsto</strong></div>
  <div class="scenario-card"><span>03</span><h2>La reversión</h2><p>Regresión de calidad, detección y auto-rollback.</p><strong>REPLAY</strong></div>
</div>

<div class="slide-caption">No confundiremos las métricas sintéticas con facturación o telemetría real.</div>

---
layout: default
---

# Mission Control

<div class="demo-context">Seleccionad un escenario y recorred la secuencia. El modo inicial es REPLAY de un dataset sintético versionado.</div>

<MissionControl />

<!-- Escena principal. Alternar la explicación de eventos y Quality Gates. En dev se puede ejecutar SIMULATED vía SSE. -->

---
layout: default
---

# El escudo de la gobernanza

<div class="split-flow">
  <div class="big-word denied">DENY</div>
  <div>
    <h2>Fail closed</h2>
    <p>Riesgo crítico detectado → bloqueo inmediato. Sin despliegue y sin excepción implícita.</p>
    <p>GitHub Policy-as-Code → evaluación determinista en backend → auditoría → alternativas seguras.</p>
  </div>
</div>

<!-- Érika bloquea el "hechizo" de David. Mostrar policy-block y el evento gate.evaluated. -->

---
layout: default
---

# El hechizo que salió mal

<div class="split-flow">
  <div class="big-word rollback">↩</div>
  <div>
    <h2>Progressive Delivery + Auto-Rollback</h2>
    <p>Despliegue canary → degradación sintética de calidad → Quality Gate → rollback preautorizado → recuperación.</p>
    <p><strong>REPLAY:</strong> secuencia reproducible; no se afirma haber modificado Azure.</p>
  </div>
</div>

---
layout: default
---

# ¿Cuánto cuesta una decisión?

<div class="metrics-line">
  <div><strong>0,42 USD</strong><small>Estimación ficticia por flujo</small></div>
  <div><strong>0,94</strong><small>Calidad sintética</small></div>
  <div><strong>1.850 ms</strong><small>Latencia sintética p95</small></div>
</div>

<p>Budget objetivo de plataforma: <b>115 €/mes</b>. No equivale a un tope garantizado en facturación.</p>
<p>El sistema distinguirá <b>estimación</b>, <b>medición</b> y <b>ahorro efectivamente observado</b>.</p>

---
layout: default
---

# Qué probar antes de darlo por bueno

<div class="check-grid">
  <div>✓ Tests de políticas y escenarios</div>
  <div>✓ Contratos de eventos</div>
  <div>✓ Replay sin conexión</div>
  <div>✓ Accesibilidad + movimiento reducido</div>
  <div>✓ Conectores protegidos por identidad</div>
  <div>✓ Ensayo general con contingencia</div>
</div>

<p class="footnote">Las capacidades marcadas como LIVE solo se considerarán verificadas tras un ensayo real autorizado.</p>

---
layout: center
class: final-screen
---

# El poder no está en el hechizo.

## Está en saber cuándo usarlo, cuándo detenerlo y cómo revertirlo.

<p>Érika Cepeda · David Oliva Paredes</p>

<div class="repo-link">github.com/davidop/netcoreconf-barcelona-2026</div>

<!-- Final: la automatización empresarial necesita límites, evidencia y supervisión. -->

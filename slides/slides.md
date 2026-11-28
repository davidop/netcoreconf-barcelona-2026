---
theme: seriph
title: "El aprendiz de agente · NetCoreConf Barcelona 2026"
info: |
  Magia, telemetría y gobernanza bajo control · Érika Cepeda y David Oliva Paredes
layout: default
class: nc-official-slide
---

<ConferenceCover />

<!-- Portada oficial. Érika es la aprendiz y David es Merlín. Textos, retratos, badges y QR editables. -->

---
layout: default
class: nc-official-slide
---

<ConferenceSponsors />

<!-- Diapositiva 2: logos originales de los patrocinadores del evento. -->


---
layout: default
---

# La aprendiz y Merlín: dos perspectivas.

<div class="duel">
  <div class="duel-pane">
    <div class="role">ÉRIKA · LA APRENDIZ</div>
    <h2>¿Y si mejoramos el workflow del agente?</h2>
    <p>Experimentación guiada en Copilot Studio: hipótesis, mejora v2 y demostración.</p>
  </div>
  <div class="duel-pane">
    <div class="role">DAVID · MERLÍN</div>
    <h2>¿Qué dicen los Guardianes?</h2>
    <p>Arquitectura MAF + Foundry, evidencias, Policy-as-Code y revisión humana.</p>
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
  <div class="arch-node high">Copilot Studio · Entrada<br><small>A2A preferido / REST protegido (PENDIENTE)</small></div>
  <div class="arch-arrow">↓ misión de incidencias</div>
  <div class="arch-node accent">MerlinOps · Microsoft Agent Framework · .NET 10<br><small>Orquestación real local, workflows v1/v2</small></div>
  <div class="arch-grid">
    <div>📈 Telemetry</div><div>💶 FinOps</div><div>🧪 Quality</div><div>🛡️ Governance</div>
  </div>
  <div class="arch-node">Microsoft Foundry · Modelos principal + respaldo<br><small>Inferencia real PENDIENTE DE VALIDAR</small></div>
  <div class="arch-arrow">↓ Policy-as-Code y Quality Gates en .NET</div>
  <div class="arch-node high">Slidev · Mission Control<br><small>Comparación, auditoría, gobernanza · SIMULATED / REPLAY</small></div>
</div>

<!-- Las rutas LIVE no se han comprobado. La demo local utiliza adaptadores deterministas. -->

---
layout: default
---

# Los tres hechizos

<div class="triple">
  <div class="scenario-card"><span>01</span><h2>El aprendiz mejora</h2><p>v1 repite consultas. v2 reduce llamadas sin perder evidencias.</p><strong>MAF LOCAL</strong></div>
  <div class="scenario-card"><span>02</span><h2>El escudo de Merlín</h2><p>Una propuesta aumenta los reintentos por encima del límite.</p><strong>DENY EN .NET</strong></div>
  <div class="scenario-card"><span>03</span><h2>La recuperación</h2><p>Regresión simulada y restauración verificada de v1 local.</p><strong>ROLLBACK LOCAL</strong></div>
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

# Merlín: ¿de verdad ha aprendido MerlinOps?

<div class="demo-context">MerlinOps mejora su PROPIO workflow de resolución de incidencias. Cuatro Guardianes en MAF, evidencias iguales para v1/v2, telemetría de la ejecución local y políticas fuera del LLM.</div>

<IncidentImprovement />

<!-- ÉRIKA (aprendiz): propone y ejecuta la mejora v1/v2. DAVID (Merlín): presenta 9 vs 4 llamadas medidas sobre fixtures sintéticos, valida Guardianes, gobernanza y rollback local. MAF real; Foundry y Copilot Studio NO CONECTADOS. -->

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

<!-- David, en el rol de Merlín, explica el DENY de backend. Érika, la aprendiz, reformula la mejora. -->

---
layout: default
---

# El hechizo que salió mal

<div class="split-flow">
  <div class="big-word rollback">↩</div>
  <div>
    <h2>Quality Gate + Restauración local verificable</h2>
    <p>Se activa v2 en un manifiesto de pruebas local → degradación sintética → Quality Gate → restauración de v1 → comprobación SHA-256.</p>
    <p><strong>SIMULATED:</strong> el incidente es ficticio. La restauración del archivo es real y local; nunca un rollback de Azure.</p>
  </div>
</div>

---
layout: default
---

# ¿Cómo probamos que ha mejorado?

<div class="metrics-line">
  <div><strong>9 → 4</strong><small>Llamadas locales contadas (fixture)</small></div>
  <div><strong>55,6%</strong><small>Reducción de llamadas en el caso sintético</small></div>
  <div><strong>4/4</strong><small>Guardianes MAF completados</small></div>
</div>

<p><b>Tiempo de proceso</b>: medido localmente en cada ejecución; no representa latencia cloud.</p>
<p><b>Foundry</b>: tokens, coste y latencia solo cuando la inferencia real esté conectada y verificada. Presupuesto objetivo: 115 €/mes.</p>

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
layout: default
class: nc-official-slide
---

<ConferenceThanks />

<!-- Cierre original del organizador. La aprobación humana y Policy-as-Code siguen independientes del relato. -->

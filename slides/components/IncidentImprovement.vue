<script setup lang="ts">
import { computed, ref } from 'vue'

type Version = {
  version: string
  toolCalls: number
  observedLocalDurationMs: number
  durationProvenance: string
  qualityScore: number
  mafGuardiansCompleted: number
  toolTrace: string[]
}
type Audit = { sequence: number, kind: string, message: string, gate: string, provenance: string }
type Report = {
  id: string
  caseId: string
  incidentId: string
  service: string
  mode: string
  evidenceSha256: string
  v1: Version
  v2: Version
  toolCallReductionPercent: number
  technicalDecision: string
  decisionReason: string
  proposedChange: string
  outcome: string
  humanApproval: string
  foundry: { provider: string, plannedProject: string, status: string, inputTokens: number|null, outputTokens: number|null }
  rollback?: { verified:boolean, restoredVersion:string } | null
  audit: Audit[]
}
const scenario = ref('incident-improvement')
const comparison = ref<Report | null>(null)
const processing = ref(false)
const error = ref('')
const localhost = computed(() => typeof location !== 'undefined'
  && ['localhost', '127.0.0.1'].includes(location.hostname))

function clear() { comparison.value = null; error.value = '' }
async function execute() {
  if (!localhost.value || processing.value) return
  processing.value = true
  clear()
  try {
    const response = await fetch('http://localhost:5080/api/incident/compare', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({caseId:scenario.value}),
    })
    if (!response.ok) throw new Error('API de MerlinOps no disponible o escenario inválido.')
    const result = await response.json() as Report
    if (result.mode !== 'SIMULATED' || result.foundry.status !== 'NOT_CONNECTED')
      throw new Error('Origen de ejecución no reconocido. No se mostrarán resultados.')
    comparison.value = result
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Error de comunicación'
  } finally {
    processing.value = false
  }
}
</script>

<template>
  <section class="incident" aria-label="MerlinOps: mejora supervisada de un workflow de incidencias">
    <header class="heading">
      <div><strong>EL APRENDIZ SE MEJORA · INC-2042</strong><small>Incidencia en order-processing · Guardianes y Quality Gates</small></div>
      <span class="provenance">LOCAL · SIMULATED</span>
    </header>
    <div class="pipeline" aria-label="Estado de los tres productos">
      <span>Copilot Studio <b>PENDIENTE</b></span>
      <span>MAF <b>REAL · LOCAL</b></span>
      <span>Foundry <b>NO CONECTADO</b></span>
    </div>
    <div class="controls">
      <label for="incident-scenario">MISIÓN</label>
      <select id="incident-scenario" v-model="scenario" @change="clear">
        <option value="incident-improvement">01 · Optimización supervisada</option>
        <option value="incident-policy-block">02 · Érika bloquea riesgo</option>
        <option value="incident-regression">03 · Regresión y rollback local</option>
        <option value="incident-missing-evidence">04 · Evidencia ausente</option>
      </select>
      <button type="button" :disabled="!localhost || processing" @click="execute">
        {{processing ? 'Ejecutando MAF…' : '▶ Ejecutar v1 vs v2'}}
      </button>
    </div>

    <div v-if="comparison" class="results">
      <div class="stats">
        <div><small>v1 · LLAMADAS LOCALES</small><strong>{{comparison.v1.toolCalls}}</strong></div>
        <div class="highlight"><small>LLAMADAS EVITADAS</small><strong>{{comparison.toolCallReductionPercent}}%</strong></div>
        <div><small>v2 · LLAMADAS LOCALES</small><strong>{{comparison.v2.toolCalls}}</strong></div>
      </div>
      <div class="smallstats">
        <span>v1 {{comparison.v1.observedLocalDurationMs}} ms · v2 {{comparison.v2.observedLocalDurationMs}} ms</span>
        <span>4/4 Guardianes MAF · calidad {{Math.round(comparison.v2.qualityScore*100)}}%</span>
      </div>
      <div class="decision" :class="{ blocked: comparison.technicalDecision === 'DENY' }">
        <strong>{{comparison.technicalDecision}} · {{comparison.outcome}}</strong>
        <p>{{comparison.decisionReason}} · {{comparison.proposedChange}}</p>
        <p v-if="comparison.rollback">
          Rollback LOCAL {{comparison.rollback.verified ? 'verificado: v1 restaurada' : 'NO VERIFICADO'}}.
        </p>
        <p v-else-if="comparison.technicalDecision==='PASS'">
          La evaluación técnica NO constituye autorización. Estado humano: {{comparison.humanApproval}}.
        </p>
      </div>
      <div class="audit" aria-label="Trazabilidad y eventos del workflow">
        <span v-for="e in comparison.audit" :key="e.sequence">{{e.sequence}} · {{e.kind}}</span>
      </div>
      <div class="footnote">Tiempo observado en proceso local con herramientas sintéticas · tokens Foundry no disponibles · no hay cambios cloud</div>
    </div>

    <div v-else class="intro">
      <strong>¿Puede MerlinOps mejorar su propia forma de resolver incidencias?</strong>
      <p>v1 repite diagnósticos. v2 reutiliza evidencias y ejecuta un workflow explícito. Érika comprueba calidad, políticas y trazabilidad antes de permitir siquiera una revisión humana.</p>
      <div class="baseline">v1 · 9 herramientas <span>→</span> v2 · 4 herramientas</div>
      <p class="hint">{{localhost ? 'Ejecuta el workflow local .NET + MAF para obtener evidencias.' : 'Vista pública de respaldo: la ejecución requiere Slidev y API locales.'}}</p>
    </div>
    <p v-if="error" role="alert" class="error">{{error}}. Disponible el Replay sintético de Mission Control.</p>
    <footer>La IA propone · .NET evalúa · Érika supervisa · Slidev muestra evidencias</footer>
  </section>
</template>

<style scoped>
.incident{height:447px;display:flex;flex-direction:column;gap:8px;padding:12px 16px;border-radius:18px;background:linear-gradient(130deg,#071528,#123452);border:1px solid #4b7598;color:#e6f4ff;font-size:12px;box-shadow:0 15px 40px #07192b6e;overflow:hidden}
.heading{display:flex;justify-content:space-between;align-items:center;gap:12px}.heading strong{display:block;color:#8be7ed;font-size:14px;letter-spacing:.04em}.heading small{display:block;color:#b3cee3}.provenance{font-size:10px;font-weight:700;border:1px solid #8a6d44;color:#ffe0b3;border-radius:18px;padding:5px 10px;white-space:nowrap}
.pipeline{display:grid;grid-template-columns:repeat(3,1fr);gap:7px}.pipeline span{border-radius:7px;background:#142c4c;padding:7px;text-align:center;font-size:11px}.pipeline b{display:block;font-size:9px;margin-top:2px;color:#ffd69b}.pipeline span:nth-child(2) b{color:#91f4d2}
.controls{display:flex;align-items:center;gap:8px;flex-wrap:wrap}.controls label{font-size:10px;color:#a9c1d9}.controls select,.controls button{font:inherit;background:#194163;border:1px solid #628bac;color:#fff;border-radius:7px;padding:7px 9px}.controls select{max-width:240px}.controls button{font-weight:700;cursor:pointer}.controls button:disabled{opacity:.5;cursor:not-allowed}.controls button:focus-visible,.controls select:focus-visible{outline:2px solid white;outline-offset:2px}
.results{display:flex;flex-direction:column;gap:8px;min-height:0;flex:1}.stats{display:grid;grid-template-columns:1fr 1.1fr 1fr;gap:8px}.stats>div{background:#163452;border:1px solid #396280;border-radius:9px;text-align:center;padding:7px}.stats small{display:block;font-size:10px;line-height:1.2;color:#b7d0df}.stats strong{display:block;font-size:30px;line-height:1.05}.stats .highlight{background:#104c4e;border-color:#37918a}.stats .highlight strong{color:#9af4d5}.smallstats{display:flex;justify-content:space-between;gap:5px;font-size:10px;color:#bde3f3}.decision{background:#134a44;border-left:3px solid #42bda5;padding:8px;max-height:105px;overflow:auto}.decision.blocked{background:#542636;border-color:#f67f89}.decision strong{font-size:13px}.decision p{font-size:10px;margin:4px 0}.audit{display:flex;gap:5px;overflow:auto;white-space:nowrap}.audit span{padding:5px 7px;border:1px solid #3e6888;border-radius:6px;background:#183856;font-size:10px}.footnote{font-size:9px;color:#bdcfe2}
.intro{flex:1;display:flex;flex-direction:column;justify-content:center;gap:10px;text-align:center;padding:14px;background:#183854;border-radius:12px}.intro strong{font-size:20px}.intro p{max-width:560px;margin:0 auto;font-size:12px}.baseline{font-size:17px;font-weight:700;color:#a1f4df}.baseline span{padding:0 12px;color:#fff}.intro .hint{font-size:10px;color:#f5d4a7}
.error{color:#ffb8bd;margin:0}.incident footer{border-top:1px solid #3f6180;padding-top:5px;text-align:center;color:#b7d4e2;font-size:10px}
@media (prefers-reduced-motion:reduce){*{animation:none!important;transition:none!important}}
</style>

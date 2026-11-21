<script setup lang="ts">
import { computed, ref } from 'vue'

type Version = { version:string, toolCalls:number, syntheticDurationMs:number, durationProvenance:string, qualityScore:number, toolTrace:string[] }
type TraceEvent = { sequence:number, kind:string, message:string, gate:string }
type Comparison = {
  id:string, caseId:string, provenance:string, evidenceSource:string, targetResource:string,
  userRequest:string, evidenceSha256:string, v1:Version, v2:Version,
  callReductionPercent:number, syntheticDurationReductionPercent:number,
  proposedChange:string, technicalDecision:string, decisionReason:string,
  outcome:string, humanApproval:string, cloudWritesEnabled:boolean,
  rollback?: { restoredVersion:string, verified:boolean, actualSha256:string } | null,
  events:TraceEvent[]
}

const caseId = ref('finops-success')
const report = ref<Comparison | null>(null)
const loading = ref(false)
const error = ref('')
const localApi = computed(() => typeof location !== 'undefined'
  && (location.hostname === 'localhost' || location.hostname === '127.0.0.1'))

async function compare() {
  if (!localApi.value || loading.value) return
  loading.value = true
  report.value = null
  error.value = ''
  try {
    const response = await fetch('http://localhost:5080/api/erika/compare', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ caseId: caseId.value }),
    })
    if (!response.ok) throw new Error('El backend local no ha aceptado la comparación.')
    const result = await response.json() as Comparison
    if (result.provenance !== 'SIMULATED' || result.cloudWritesEnabled)
      throw new Error('Procedencia o permisos de escritura inesperados: ejecución rechazada.')
    report.value = result
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'No se ha podido completar la comparación.'
  } finally {
    loading.value = false
  }
}
function choose() { report.value = null; error.value = '' }
</script>

<template>
  <section class="compare" aria-label="Comparación supervisada v1 y v2 de MerlinOps">
    <header class="top">
      <div><strong>CASO ÉRIKA · AZURE CONTAINER APPS</strong><small>Una petición · las mismas evidencias · dos workflows</small></div>
      <span class="tag">SIMULATED · FIXTURES</span>
    </header>
    <div class="action">
      <label for="case-selector">CASO</label>
      <select id="case-selector" v-model="caseId" @change="choose">
        <option value="finops-success">01 · Mejorar sin perder calidad</option>
        <option value="policy-block">02 · Bloqueo de política</option>
        <option value="quality-rollback">03 · Regresión y rollback local</option>
        <option value="missing-metrics">Extra · Métricas incompletas</option>
      </select>
      <button type="button" :disabled="!localApi || loading" @click="compare">
        {{ loading ? 'Evaluando…' : 'Ejecutar comparación local' }}
      </button>
    </div>

    <div v-if="report" class="results">
      <div class="measurement">
        <div><small>v1 · comprobaciones repetidas</small><strong>{{report.v1.toolCalls}}</strong><span>llamadas a herramienta local</span></div>
        <div class="saving"><small>REDUCCIÓN DE LLAMADAS</small><strong>{{report.callReductionPercent}}%</strong><span>medición del simulador local</span></div>
        <div><small>v2 · workflow explícito</small><strong>{{report.v2.toolCalls}}</strong><span>llamadas a herramienta local</span></div>
      </div>
      <div class="quality">
        <span>RÚBRICA DETERMINISTA: {{Math.round(report.v2.qualityScore*100)}}%</span>
        <span>DURACIÓN: {{report.syntheticDurationReductionPercent}}% menos · MODELO SINTÉTICO</span>
      </div>
      <div class="decision">
        <strong :class="report.technicalDecision==='DENY' ? 'deny' : 'pass'">
          {{report.technicalDecision}} · {{report.outcome}}
        </strong>
        <p>{{report.decisionReason}}</p>
        <p>{{report.proposedChange}}</p>
        <p v-if="report.rollback">Rollback local real: {{report.rollback.verified ? 'v1 restaurada y hash verificado' : 'VERIFICACIÓN FALLIDA'}}</p>
        <p v-if="report.technicalDecision==='PASS' && !report.rollback">Los gates técnicos pasan; aún no existe aprobación humana.</p>
      </div>
      <div class="trail" aria-label="Registro de decisiones">
        <span v-for="event in report.events" :key="event.sequence">{{event.sequence}} · {{event.kind}}</span>
      </div>
      <p class="small">Snapshot SHA-256: {{report.evidenceSha256.slice(0,16)}}… · {{report.evidenceSource}} · NO AZURE WRITES</p>
    </div>

    <div v-else class="placeholder">
      <div class="headline">El aprendiz detecta trabajo repetido.</div>
      <p>v1 consulta varias veces configuración, tráfico y latencia. v2 agrupa las comprobaciones y mantiene las mismas reglas de calidad y gobernanza.</p>
      <div class="sample">
        <span><b>v1</b> · 9 consultas</span>
        <span>vs.</span>
        <span><b>v2</b> · 4 consultas</span>
      </div>
      <p v-if="!localApi" class="notice">GitHub Pages: vista pública sin comandos. Para ejecutar la comparación, abre Slidev y la API .NET 10 en localhost.</p>
      <p v-else class="notice">Ejecuta el caso para obtener evidencias del motor de pruebas local.</p>
    </div>
    <p v-if="error" role="alert" class="error">{{error}} Replay anterior disponible en Mission Control.</p>
    <footer>No self-learning nativo · Sin aprobación o despliegue real · Calidad y políticas en backend</footer>
  </section>
</template>

<style scoped>
.compare{height:448px;background:linear-gradient(135deg,#061528,#0e294b);border:1px solid #416b91;border-radius:18px;box-shadow:0 18px 35px #00132677;color:#e7f4ff;overflow:hidden;padding:12px 16px;display:flex;flex-direction:column;gap:8px;font-size:12px}
.top{display:flex;justify-content:space-between;align-items:center;gap:12px}.top strong{display:block;font-size:13px;letter-spacing:.06em;color:#8eeaf0}.top small{color:#b8c9db}.tag{border:1px solid #8e6e4d;color:#ffdda9;border-radius:14px;padding:4px 9px;font-size:10px;white-space:nowrap}
.action{display:flex;align-items:center;gap:8px;flex-wrap:wrap}.action label{font-size:10px;color:#b8c9db}.action select,.action button{background:#183b61;color:#fff;border:1px solid #5b7fa4;border-radius:7px;padding:7px 10px;font:inherit}.action select{max-width:250px}.action button{cursor:pointer;font-weight:700}.action button:disabled{opacity:.5;cursor:not-allowed}.action button:focus-visible,.action select:focus-visible{outline:2px solid #fff;outline-offset:2px}
.results{display:flex;flex-direction:column;gap:7px;flex:1;min-height:0}.measurement{display:grid;grid-template-columns:1fr 1.1fr 1fr;gap:8px}.measurement>div{border:1px solid #37617e;border-radius:10px;background:#163251;padding:9px;text-align:center}.measurement small{display:block;min-height:23px;color:#c0d2e3}.measurement strong{font-size:32px;display:block;color:#fff;line-height:1}.measurement span{font-size:10px;color:#b6d4db}.measurement .saving{background:#0d4244;border-color:#238d88}.saving strong{color:#81f1c8}.quality{display:flex;justify-content:space-between;gap:9px;font-weight:700;font-size:10px;color:#c9e3eb}
.decision{background:#16375b;border-left:3px solid #5ba6df;padding:9px;max-height:109px;overflow:auto}.decision strong{font-size:12px}.decision strong.pass{color:#80f1c5}.decision strong.deny{color:#ff9ca4}.decision p{margin:3px 0;font-size:10px}.trail{display:flex;gap:5px;overflow:auto;white-space:nowrap}.trail span{font-size:10px;padding:5px 7px;border:1px solid #355a78;border-radius:6px;background:#173552}.small{font-size:9px;color:#9eb8cc;margin:0}
.placeholder{flex:1;background:#142d4b;border-radius:12px;padding:20px;display:flex;flex-direction:column;justify-content:center;gap:7px;text-align:center}.headline{font-size:20px;font-weight:700}.placeholder p{font-size:12px;max-width:570px;margin:4px auto}.sample{display:flex;align-items:center;justify-content:center;gap:12px;font-size:19px}.sample span:first-child,.sample span:last-child{background:#194260;padding:8px 14px;border-radius:10px}.sample b{color:#94ebee}.notice{color:#f5d4a5}
.error{color:#ffacb6;margin:0}.compare footer{font-size:10px;color:#aac9da;border-top:1px solid #3a5471;padding-top:5px;text-align:center}
@media (prefers-reduced-motion:reduce){*{animation:none!important;transition:none!important}}
</style>

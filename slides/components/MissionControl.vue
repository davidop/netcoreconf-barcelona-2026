<script setup lang="ts">
import { computed, onBeforeUnmount, ref } from 'vue'
import replay from '../data/replay.json'
import MerlinOps3D from './MerlinOps3D.vue'

type Event = { sequence:number, kind:string, agent:string, message:string, gate:string, metric?:number, traceId:string }
type Scenario = { id:string, name:string, status:string, events:Event[] }
const scenarios = replay.scenarios as Scenario[]
const selected = ref(0)
const position = ref(0)
const running = ref(false)
const provenance = ref<'REPLAY'|'SIMULATED'>('REPLAY')
const currentEvents = ref<Event[]>(scenarios[0].events)
const error = ref('')
let ticker:ReturnType<typeof setInterval>|undefined
let stream:EventSource|undefined

const visible = computed(() => currentEvents.value.slice(0,position.value))
const last = computed(() => visible.value.at(-1))
const gate = computed(() => [...visible.value].reverse().find(x => x.gate !== 'NONE'))
const percent = computed(() => currentEvents.value.length ? Math.round(position.value/currentEvents.value.length*100) : 0)
const visualState = computed(() => last.value?.kind.includes('rollback') ? 'rollback'
  : last.value?.gate === 'DENY' ? 'blocked'
  : last.value?.kind.includes('approval') ? 'approval'
  : last.value?.kind.includes('completed') ? 'success' : 'planning')
const localApiEnabled = computed(() => typeof location !== 'undefined' && ['localhost','127.0.0.1'].includes(location.hostname))

function stop() {
  if (ticker) clearInterval(ticker)
  ticker = undefined
  running.value = false
  stream?.close()
  stream = undefined
}
function changeScenario(index:number) {
  stop()
  selected.value = index
  provenance.value = 'REPLAY'
  currentEvents.value = scenarios[index].events
  position.value = 0
  error.value = ''
}
function next() {
  if(position.value < currentEvents.value.length) position.value++
  else stop()
}
function previous() { position.value = Math.max(0,position.value-1) }
function play() {
  stop()
  if(position.value >= currentEvents.value.length) position.value = 0
  running.value = true
  ticker = setInterval(next, 900)
}
function reset() { stop(); position.value = 0 }

async function simulateViaSse() {
  if(!localApiEnabled.value) return
  stop()
  error.value = ''
  currentEvents.value = []
  position.value = 0
  provenance.value = 'SIMULATED'
  try {
    const response = await fetch('http://localhost:5080/api/runs/maf-simulate', {
      method:'POST',headers:{'Content-Type':'application/json'},
      body:JSON.stringify({scenario:scenarios[selected.value].id}),
    })
    if(!response.ok) throw new Error('API offline or scenario rejected')
    const run = await response.json()
    stream = new EventSource('http://localhost:5080/api/runs/'+encodeURIComponent(run.id)+'/stream')
    stream.addEventListener('mission',(evt:MessageEvent) => {
      const message = JSON.parse(evt.data) as { mode:string, missionEvent:Event }
      if(message.mode !== 'SIMULATED') { error.value='Invalid provenance';stop();return }
      currentEvents.value = [...currentEvents.value,message.missionEvent]
      position.value = currentEvents.value.length
    })
    stream.addEventListener('complete',()=>{stream?.close();stream=undefined})
    stream.onerror = () => {
      stream?.close();stream=undefined
      error.value='SSE interrumpido. Replay sintético disponible.'
    }
  } catch(e) {
    error.value = e instanceof Error ? e.message : 'Error en API'
    changeScenario(selected.value)
    error.value = 'API no disponible. Se ha recuperado REPLAY sintético.'
  }
}
onBeforeUnmount(stop)
</script>

<template>
  <section class="mission" aria-label="Mission Control MerlinOps">
    <header class="mission-header">
      <div class="mission-title"><span class="signal"></span><strong>MERLINOPS</strong><span>MISSION CONTROL</span></div>
      <div class="mode" :aria-label="'Origen de datos: '+provenance">{{ provenance }} · SYNTHETIC</div>
    </header>

    <div class="mission-commands">
      <label for="mission-scenario">ESCENARIO</label>
      <select id="mission-scenario" :value="selected" @change="changeScenario(Number(($event.target as HTMLSelectElement).value))">
        <option v-for="(scene,index) in scenarios" :key="scene.id" :value="index">{{scene.name}}</option>
      </select>
      <button type="button" @click="play" :disabled="provenance!=='REPLAY'">▶ Reproducir</button>
      <button type="button" @click="stop" :disabled="!running">Ⅱ Pausar</button>
      <button type="button" @click="previous" :disabled="provenance!=='REPLAY' || position===0">‹</button>
      <button type="button" @click="next" :disabled="provenance!=='REPLAY' || position>=currentEvents.length">›</button>
      <button type="button" @click="reset" :disabled="provenance!=='REPLAY'">↺</button>
      <button type="button" class="simulate" @click="simulateViaSse" :disabled="!localApiEnabled">SIMULATED SSE</button>
    </div>

    <div class="mission-body">
      <div class="agent-focus">
        <MerlinOps3D :status="visualState" />
        <div class="agent-label"><strong>MerlinOps</strong><small>Supervisor multiagente</small></div>
        <div class="guardian-chips"><span>Telemetry</span><span>FinOps</span><span>Quality</span><span>Governance</span></div>
      </div>
      <div class="telemetry">
        <div class="metric-row">
          <div><small>EVENTOS</small><strong>{{position}} / {{currentEvents.length}}</strong></div>
          <div><small>QUALITY GATE</small><strong :class="{denied:gate?.gate==='DENY'}">{{gate?.gate || 'WAITING'}}</strong></div>
          <div><small>PROGRESO</small><strong>{{percent}}%</strong></div>
        </div>
        <div class="track" role="progressbar" :aria-valuenow="percent" aria-valuemin="0" aria-valuemax="100"><span :style="{width:percent+'%'}"></span></div>
        <div class="timeline" aria-live="polite" aria-relevant="additions">
          <div v-for="event in [...visible].reverse()" :key="event.sequence" class="event-row">
            <span class="sequence">{{String(event.sequence).padStart(2,'0')}}</span>
            <div><b>{{event.agent}}</b><p>{{event.message}}</p></div>
            <span v-if="event.gate!=='NONE'" :class="['gate',event.gate.toLowerCase()]">{{event.gate}}</span>
          </div>
          <p v-if="!visible.length" class="empty">Pulsa Reproducir para seguir las decisiones de los Guardianes.</p>
        </div>
      </div>
    </div>
    <footer class="mission-footer">
      <span>{{last?.kind ?? 'READY'}}</span>
      <span>DATOS FICTICIOS · SIN CAMBIOS AZURE · LIVE DESHABILITADO</span>
    </footer>
    <p v-if="error" role="alert" class="mission-error">{{error}}</p>
  </section>
</template>

<style scoped>
.mission{height:465px;background:linear-gradient(135deg,#09192f,#102644);border:1px solid #416b91;border-radius:18px;overflow:hidden;box-shadow:0 25px 50px #020b16ab;color:#e6f3ff;font-size:13px}
.mission-header{display:flex;justify-content:space-between;align-items:center;padding:12px 18px;background:#16345a;border-bottom:1px solid #42628a}
.mission-title{display:flex;gap:12px;align-items:center;letter-spacing:.07em}.mission-title>strong{color:#78e7f7;letter-spacing:.14em}.mission-title>span:last-child{font-size:10px;color:#a6beda}
.signal{width:8px;height:8px;border-radius:50%;background:#6fe6cd;box-shadow:0 0 10px #50ddb8}
.mode{font-size:10px;color:#ffe0ab;font-weight:700;border:1px solid #b48354;padding:5px 9px;border-radius:18px}
.mission-commands{display:flex;gap:7px;align-items:center;padding:8px 15px;flex-wrap:wrap}
.mission-commands label{color:#a0b7d3;font-size:10px}
.mission-commands select,.mission-commands button{font:inherit;color:#e7f4ff;border:1px solid #4c709a;background:#15395f;border-radius:7px;padding:5px 9px;cursor:pointer}
.mission-commands select{max-width:172px}.mission-commands button:disabled{opacity:.43;cursor:not-allowed}.mission-commands .simulate{border-color:#698de4;color:#cbd4ff}
.mission-commands button:focus-visible,.mission-commands select:focus-visible{outline:2px solid #9bf6ff;outline-offset:2px}
.mission-body{display:grid;grid-template-columns:36% 64%;height:332px}
.agent-focus{border-right:1px solid #32567a;display:flex;flex-direction:column;justify-content:center;align-items:center;padding:7px}.agent-label{text-align:center}.agent-label strong{display:block;font-size:18px}.agent-label small{color:#a3bfd9}.guardian-chips{display:flex;justify-content:center;flex-wrap:wrap;gap:5px;margin-top:8px}.guardian-chips span{font-size:9px;border:1px solid #2f7390;border-radius:12px;padding:3px 5px;color:#a8dbe9}
.telemetry{padding:10px 15px;min-height:0;display:flex;flex-direction:column}.metric-row{display:grid;grid-template-columns:repeat(3,1fr);gap:8px}.metric-row>div{background:#173858;border:1px solid #2c5a7b;padding:6px 8px;border-radius:8px}.metric-row small{display:block;color:#92b4c9;font-size:9px}.metric-row strong{font-size:15px;color:#b0f3ed}.metric-row strong.denied{color:#ff8e98}.track{height:5px;margin:8px 0;border-radius:10px;background:#284563;overflow:hidden}.track span{display:block;background:#71decf;height:100%;transition:width .2s}
.timeline{flex:1;overflow:auto;min-height:0;scrollbar-color:#497fa4 transparent}.event-row{display:flex;gap:9px;align-items:start;border-bottom:1px solid #294462;padding:5px 0;font-size:11px}.sequence{color:#68bce7;font-weight:700;font-family:monospace}.event-row>div{flex:1}.event-row b{color:#e2f2ff}.event-row p{margin:1px 0;color:#b7cce1;font-size:10px}.gate{font-size:9px;font-weight:700;padding:2px 5px;border-radius:5px;color:#d6dce5;background:#304a6c}.gate.pass{background:#13493f;color:#7cebcf}.gate.deny{background:#5e2534;color:#ffb1bb}.gate.pending{background:#675424;color:#ffe4a8}.empty{color:#8fa8c9;margin:35px 0;text-align:center}
.mission-footer{display:flex;justify-content:space-between;background:#102846;border-top:1px solid #365478;padding:9px 14px;font-size:9px;letter-spacing:.04em;color:#afcce1}.mission-error{color:#ffb4c1;margin:0;padding:3px 14px;font-size:11px}
@media (prefers-reduced-motion:reduce){.track span{transition:none}}
</style>

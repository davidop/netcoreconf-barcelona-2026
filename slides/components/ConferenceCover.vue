<script setup>
import { onMounted, ref } from 'vue'
import QRCode from 'qrcode'

const repo = 'https://github.com/davidop/netcoreconf-barcelona-2026'
const asset = (filename) => import.meta.env.BASE_URL + 'conference/' + filename
const qrData = ref('')
const missing = ref({ erika: false, david: false, mvp: false })
onMounted(async () => {
  try { qrData.value = await QRCode.toDataURL(repo, { margin: 2, width: 256, errorCorrectionLevel: 'Q' }) }
  catch { qrData.value = '' }
})
</script>

<template>
  <section class="nc-cover" aria-label="Portada oficial de NetCoreConf Barcelona 2026">
    <div class="nc-cover__art" :style="{ backgroundImage: 'url(' + asset('official-cover.webp') + ')' }" aria-hidden="true" />
    <div class="nc-cover__brand-fallback" aria-hidden="true">&lt;NetCoreConf/&gt; · Barcelona ’26</div>
    <div class="nc-cover__content">
      <p class="nc-cover__eyebrow">BARCELONA · 12 DICIEMBRE 2026</p>
      <h1>El aprendiz de agente:<br>magia, telemetría y<br>gobernanza bajo control <span aria-hidden="true">🪄</span></h1>
      <p class="nc-cover__strap">Copilot Studio · Microsoft Agent Framework · Foundry</p>

      <div class="nc-cover__speakers" aria-label="Ponentes">
        <a class="nc-speaker" href="https://www.linkedin.com/in/erikacepeda/" target="_blank" rel="noopener noreferrer" aria-label="Perfil de LinkedIn de Érika Cepeda">
          <img v-if="!missing.erika" class="nc-speaker__portrait" :src="asset('erika.webp')" alt="Érika Cepeda" @error="missing.erika = true" />
          <div v-else class="nc-speaker__portrait nc-speaker__placeholder">ÉC</div>
          <strong>Érika Cepeda</strong>
          <span class="nc-speaker__title">M365 Copilot Specialist</span>
          <img v-if="!missing.mvp" class="nc-speaker__badge" :src="asset('mvp.webp')" alt="Microsoft MVP" @error="missing.mvp = true" />
          <span v-else class="nc-speaker__badge-fallback">◇ MICROSOFT MVP</span>
        </a>
        <a class="nc-speaker" href="https://www.linkedin.com/in/davidolivaparedes/" target="_blank" rel="noopener noreferrer" aria-label="Perfil de LinkedIn de David Oliva">
          <img v-if="!missing.david" class="nc-speaker__portrait" :src="asset('david.webp')" alt="David Oliva" @error="missing.david = true" />
          <div v-else class="nc-speaker__portrait nc-speaker__placeholder">DO</div>
          <strong>David Oliva</strong>
          <span class="nc-speaker__title">Microsoft Specialist</span>
          <img v-if="!missing.mvp" class="nc-speaker__badge" :src="asset('mvp.webp')" alt="Microsoft MVP" @error="missing.mvp = true" />
          <span v-else class="nc-speaker__badge-fallback">◇ MICROSOFT MVP</span>
        </a>
      </div>
      <div class="nc-cover__footer">
        <div class="nc-cover__role"><b>Érika</b> · La aprendiz <span aria-hidden="true">✦</span> <b>David</b> · Merlín</div>
        <a class="nc-cover__repo" :href="repo" target="_blank" rel="noopener noreferrer" aria-label="Abrir repositorio público MerlinOps en GitHub">
          <span class="nc-cover__repo-label"><b>GitHub</b><small>Escanea para explorar la demo</small></span>
          <img v-if="qrData" :src="qrData" alt="Código QR al repositorio de MerlinOps" />
          <span v-else class="nc-cover__qr-fallback">github.com/davidop/netcoreconf-barcelona-2026</span>
        </a>
      </div>
    </div>
  </section>
</template>

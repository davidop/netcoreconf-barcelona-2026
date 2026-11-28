<script setup>
import { ref } from 'vue'
const missing = ref(false)
const src = import.meta.env.BASE_URL + 'conference/official-sponsors.webp'
</script>
<template>
  <section class="nc-sponsors" aria-label="Patrocinadores oficiales de NetCoreConf Barcelona 2026">
    <img v-if="!missing" :src="src" alt="Diapositiva de patrocinadores originales proporcionada por NetCoreConf Barcelona 2026: Platinum, Gold y Colabora" @error="missing = true" />
    <div v-else class="nc-sponsors__fallback">
      <h1>Patrocinadores oficiales · NetCoreConf 2026</h1>
      <p>Platinum: Inkoova · Intelequia · NTT DATA · Tokiota · V-Valley</p>
      <p>Gold: Axazure · Plain Concepts · SCRM Lidl International Hub</p>
      <p>Colabora: Microsoft · Sessionize</p>
      <small>Recurso de la plantilla oficial pendiente: slides/public/conference/official-sponsors.webp</small>
    </div>
  </section>
</template>

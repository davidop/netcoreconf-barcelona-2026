<script setup>
const repo = 'https://github.com/davidop/netcoreconf-barcelona-2026'
const image = import.meta.env.BASE_URL + 'conference/official-thanks.webp'
</script>
<template>
  <section class="nc-thanks" aria-label="Diapositiva de agradecimiento de NetCoreConf Barcelona 2026">
    <div class="nc-thanks__art" :style="{ backgroundImage: 'url(' + image + ')' }" aria-hidden="true" />
    <div class="nc-thanks__links">
      <span>Érika Cepeda · David Oliva</span>
      <a :href="repo" target="_blank" rel="noopener noreferrer">github.com/davidop/netcoreconf-barcelona-2026</a>
    </div>
  </section>
</template>

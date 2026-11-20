import { defineConfig } from 'vite'

// GitHub Pages project site. Local dev and local preview retain / as base.
export default defineConfig({
  base: process.env.GITHUB_PAGES === 'true' ? '/netcoreconf-barcelona-2026/' : '/',
})

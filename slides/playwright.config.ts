import { defineConfig } from '@playwright/test'

export default defineConfig({
  testDir: './e2e',
  timeout: 30000,
  use: { baseURL: 'http://127.0.0.1:3030', trace: 'on-first-retry', screenshot: 'on' },
  webServer: {
    command: 'npm run build && npx vite preview --host 127.0.0.1 --port 3030 --strictPort',
    url: 'http://127.0.0.1:3030',
    timeout: 120000,
    stdout: 'pipe',
    stderr: 'pipe',
    reuseExistingServer: !process.env.CI,
  },
})

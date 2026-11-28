import { expect, test } from '@playwright/test'

test('Slidev loads the conference cover', async ({ page }) => {
  await page.goto('/')
  await expect(page.getByText('El aprendiz', { exact: false }).first()).toBeVisible()
  await page.screenshot({ path: 'test-results/cover.png', fullPage: true })
})

test('Mission Control retains truthful synthetic provenance', async ({ page }) => {
  await page.goto('/7')
  const mission = page.getByRole('region', { name: 'Mission Control MerlinOps' })
  await expect(mission).toBeVisible()
  await expect(mission.getByText('REPLAY · SYNTHETIC')).toBeVisible()
  await expect(mission.getByText('LIVE DESHABILITADO', { exact: false })).toBeVisible()
  await mission.getByRole('button', { name: '▶ Reproducir' }).click()
  await page.screenshot({ path: 'test-results/mission-control.png', fullPage: true })
})


test('Erika incident improvement stage states actual and pending integrations', async ({ page }) => {
  await page.goto('/8')
  const panel = page.getByRole('region', { name: 'MerlinOps: mejora supervisada de un workflow de incidencias' })
  await expect(panel).toBeVisible()
  await expect(panel.getByText('LOCAL · SIMULATED')).toBeVisible()
  await expect(panel.getByText('REAL · LOCAL')).toBeVisible()
  await expect(panel.getByText('NO CONECTADO')).toBeVisible()
  await expect(panel.getByText('¿Puede MerlinOps mejorar su propia forma de resolver incidencias?')).toBeVisible()
  await expect(panel.getByText('v1').first()).toBeVisible()
  await expect(panel.getByText('v2').first()).toBeVisible()
  await panel.getByLabel('MISIÓN').selectOption('incident-policy-block')
  await expect(panel.getByRole('button', { name: '▶ Ejecutar v1 vs v2' })).toBeVisible()
  await page.screenshot({ path: 'test-results/erika-incident-v1-v2.png', fullPage: true })
})

test('Editable conference cover shows both MVPs, titles, GitHub link and sponsor stage', async ({ page }) => {
  await page.goto('/1')
  const cover = page.getByRole('region', { name: 'Portada oficial de NetCoreConf Barcelona 2026' })
  await expect(cover.getByText('Érika Cepeda')).toBeVisible()
  await expect(cover.getByText('David Oliva')).toBeVisible()
  await expect(cover.getByText('M365 Copilot Specialist')).toBeVisible()
  await expect(cover.getByText('Microsoft Specialist', { exact: true })).toBeVisible()
  await expect(cover.getByRole('link', { name: 'Abrir repositorio público MerlinOps en GitHub' })).toHaveAttribute('href','https://github.com/davidop/netcoreconf-barcelona-2026')
  await expect(cover.getByAltText('Código QR al repositorio de MerlinOps')).toBeVisible()
  await page.goto('/2')
  await expect(page.getByRole('region', { name: 'Patrocinadores oficiales de NetCoreConf Barcelona 2026' })).toBeVisible()
})

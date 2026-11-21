import { expect, test } from '@playwright/test'

test('Slidev loads the conference cover', async ({ page }) => {
  await page.goto('/')
  await expect(page.getByText('El aprendiz', { exact: false }).first()).toBeVisible()
  await page.screenshot({ path: 'test-results/cover.png', fullPage: true })
})

test('Mission Control retains truthful synthetic provenance', async ({ page }) => {
  await page.goto('/6')
  const mission = page.getByRole('region', { name: 'Mission Control MerlinOps' })
  await expect(mission).toBeVisible()
  await expect(mission.getByText('REPLAY · SYNTHETIC')).toBeVisible()
  await expect(mission.getByText('LIVE DESHABILITADO', { exact: false })).toBeVisible()
  await mission.getByRole('button', { name: '▶ Reproducir' }).click()
  await page.screenshot({ path: 'test-results/mission-control.png', fullPage: true })
})


test('Erika comparison explains synthetic evidence and backend-only gates', async ({ page }) => {
  await page.goto('/7')
  const panel = page.getByRole('region', { name: 'Comparación supervisada v1 y v2 de MerlinOps' })
  await expect(panel).toBeVisible()
  await expect(panel.getByText('SIMULATED · FIXTURES')).toBeVisible()
  await expect(panel.getByText('El aprendiz detecta trabajo repetido.')).toBeVisible()
  await expect(panel.getByText('v1').first()).toBeVisible()
  await expect(panel.getByText('v2').first()).toBeVisible()
  await panel.getByLabel('CASO').selectOption('policy-block')
  await expect(panel.getByRole('button', { name: 'Ejecutar comparación local' })).toBeVisible()
  await page.screenshot({ path: 'test-results/erika-v1-v2.png', fullPage: true })
})

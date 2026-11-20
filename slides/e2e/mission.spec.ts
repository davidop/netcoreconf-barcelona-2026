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

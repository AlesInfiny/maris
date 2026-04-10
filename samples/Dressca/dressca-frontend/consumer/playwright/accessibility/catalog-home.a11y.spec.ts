import { mkdir, writeFile } from 'node:fs/promises'
import path from 'node:path'
import AxeBuilder from '@axe-core/playwright'
import { test } from '@playwright/test'

const wcagTags = ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa']
const outputPath = path.resolve(
  process.cwd(),
  'playwright-report',
  'accessibility',
  'catalog-home.json',
)

test('catalog home page accessibility scan', async ({ page }, testInfo) => {
  await page.goto('/')
  await page.locator('#category-select').waitFor()
  await page.locator('#brand-select').waitFor()
  await page.getByRole('button', { name: '買い物かごに入れる' }).first().waitFor()

  const accessibilityScanResults = await new AxeBuilder({ page }).withTags(wcagTags).analyze()

  await testInfo.attach('accessibility-scan-results', {
    body: JSON.stringify(accessibilityScanResults, null, 2),
    contentType: 'application/json',
  })

  const report = {
    name: 'Catalog home',
    path: '/',
    scannedAt: new Date().toISOString(),
    violationCount: accessibilityScanResults.violations.length,
    violations: accessibilityScanResults.violations.map((violation) => ({
      id: violation.id,
      impact: violation.impact ?? 'unknown',
      description: violation.description,
      help: violation.help,
      helpUrl: violation.helpUrl,
      nodeCount: violation.nodes.length,
      targets: violation.nodes.flatMap((node) => node.target),
    })),
  }

  await mkdir(path.dirname(outputPath), { recursive: true })
  await writeFile(outputPath, JSON.stringify(report, null, 2), 'utf8')
})

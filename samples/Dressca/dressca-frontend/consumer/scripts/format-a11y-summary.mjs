import { readdir, readFile, writeFile } from 'node:fs/promises'
import path from 'node:path'

const inputDirectory = path.resolve(process.cwd(), 'playwright-report', 'accessibility')
const outputFile = path.resolve(process.cwd(), 'playwright-report', 'accessibility-summary.md')
const impactOrder = ['critical', 'serious', 'moderate', 'minor', 'unknown']

const escapeTableCell = (value) => String(value).replaceAll('|', '\\|').replaceAll('\n', '<br>')

const collectJsonFiles = async (directory) => {
  const entries = await readdir(directory, { withFileTypes: true })
  const files = await Promise.all(
    entries.map(async (entry) => {
      const fullPath = path.join(directory, entry.name)
      if (entry.isDirectory()) {
        return collectJsonFiles(fullPath)
      }

      return entry.name.endsWith('.json') ? [fullPath] : []
    }),
  )

  return files.flat()
}

const jsonFiles = await collectJsonFiles(inputDirectory)

if (jsonFiles.length === 0) {
  throw new Error(`No accessibility result files were found under ${inputDirectory}.`)
}

const scanResults = await Promise.all(
  jsonFiles.map(async (filePath) => JSON.parse(await readFile(filePath, 'utf8'))),
)

const violations = scanResults.flatMap((result) =>
  result.violations.map((violation) => ({
    ...violation,
    pageName: result.name,
    pagePath: result.path,
  })),
)

const countsByImpact = impactOrder.reduce((counts, impact) => {
  counts[impact] = violations.filter((violation) => violation.impact === impact).length
  return counts
}, {})

const lines = []

if (violations.length === 0) {
  lines.push(
    `Scanned ${scanResults.length} page(s). No automatically detectable WCAG A/AA violations were found.`,
  )
} else {
  lines.push(
    `Scanned ${scanResults.length} page(s). Detected ${violations.length} automatically detectable WCAG A/AA violation(s).`,
  )
  lines.push('')
  lines.push('| Impact | Count |')
  lines.push('| --- | ---: |')

  for (const impact of impactOrder) {
    lines.push(`| ${impact} | ${countsByImpact[impact]} |`)
  }

  lines.push('')
  lines.push('| Page | Rule | Impact | Nodes | Help |')
  lines.push('| --- | --- | --- | ---: | --- |')

  for (const violation of violations) {
    lines.push(
      `| ${escapeTableCell(violation.pageName)} (${escapeTableCell(violation.pagePath)}) | ${escapeTableCell(violation.id)} | ${escapeTableCell(violation.impact)} | ${violation.nodeCount} | [${escapeTableCell(violation.help)}](${violation.helpUrl}) |`,
    )
  }
}

lines.push('')
lines.push(
  `Source files: ${jsonFiles.map((filePath) => path.relative(process.cwd(), filePath)).join(', ')}`,
)

await writeFile(outputFile, `${lines.join('\n')}\n`, 'utf8')

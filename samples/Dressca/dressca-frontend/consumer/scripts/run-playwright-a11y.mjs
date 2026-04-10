import { spawn } from 'node:child_process'

const child = spawn('npm exec -- playwright test --config ./playwright.config.ts', {
  shell: true,
  stdio: 'inherit',
  env: {
    ...process.env,
    PLAYWRIGHT_A11Y: '1',
  },
})

child.on('error', (error) => {
  console.error(error)
  process.exit(1)
})

child.on('exit', (code) => {
  process.exit(code ?? 1)
})

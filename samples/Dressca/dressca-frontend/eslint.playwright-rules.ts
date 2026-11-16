import type { Linter } from 'eslint'

/**
 * Playwright のページオブジェクトに適用するルールです。
 */
export const playwrightCodingConventionRules: Linter.Config[] = [
  {
    name: 'e2e/pages/additional-rules',
    files: ['**/e2e/pages/**/*.ts'],
    rules: {
      'no-restricted-syntax': [
        'error',
        {
          selector:
            'CallExpression[callee.property.name=/^(innerText|textContent|allInnerTexts|allTextContents|isVisible|isEnabled)$/]',
          message:
            'Page Object 内で innerText(), isVisible() 等の値を取得するメソッドは使用しないでください。Locator を返し、テストコード側で expect(locator).toHaveXxx() / toBeXxx() を使ってください。',
        },
      ],
    },
  },
]

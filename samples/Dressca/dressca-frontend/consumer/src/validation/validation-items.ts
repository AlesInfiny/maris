import { z } from 'zod'
import { i18n } from '@/locales/i18n'

/**
 * Zod を利用したバリデーションルールを返します。
 * 現在はメールアドレス形式のチェックのみを提供しています。
 * @returns バリデーションルールのオブジェクト
 * @example
 * const rules = ValidationItems()
 * rules.email.safeParse('test@example.com').success // true
 * rules.email.safeParse('invalid').success // false
 */
export function ValidationItems() {
  const { t } = i18n.global
  const validationItems = {
    email: z.string().email(t('email')),
  }
  return validationItems
}

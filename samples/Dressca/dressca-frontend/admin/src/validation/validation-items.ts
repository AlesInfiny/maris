import { toTypedSchema } from '@vee-validate/zod'
import { z } from 'zod'

// バリデーション定義（一元化）
export const validationItems = {
  email: z.string().email('メールアドレスの形式で入力してください。'),
}

/**
 * カタログアイテムのバリデーションを定義する Zod スキーマです。
 */
export const catalogItemZodSchema = z.object({
  itemName: z.string().min(1, 'アイテム名は必須です。').max(256, '256文字以下で入力してください。'),
  itemDescription: z
    .string()
    .min(1, '説明は必須です。')
    .max(1024, '1024文字以下で入力してください。'),
  price: z
    .string()
    .min(1, '単価は必須です。')
    .regex(/^[1-9]\d*$/, '1以上の整数を半角数字で入力してください'),
  productCode: z
    .string()
    .min(1, '商品コードは必須です。')
    .max(128, '128文字以下で入力してください。')
    .regex(/^[0-9a-zA-Z]+$/, '半角英数字で入力してください。'),
})

export type CatalogItemFormValues = z.infer<typeof catalogItemZodSchema>

/**
 * カタログアイテムのバリデーションを定義する型付きのスキーマです。
 */
export const catalogItemSchema = toTypedSchema(catalogItemZodSchema)

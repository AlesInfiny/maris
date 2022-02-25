/** キャンペーン */
export interface Campaign {
  campaignCode: string;
  imageId: string;
}

/** セール品 */
export interface SaleItem {
  productCode: string;
  imageId: string;
}

export type SpecialContent = Campaign | SaleItem;

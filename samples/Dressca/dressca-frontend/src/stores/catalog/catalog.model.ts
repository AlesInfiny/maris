export interface Category {
  id: number;
  name: string;
}

export interface Brand {
  id: number;
  name: string;
}

export interface Item {
  catalogCategoryId: number;
  catalogBrandId: number;
  description: string;
  name: string;
  price: number;
  productCode: string;
  imageIds: string[];
}

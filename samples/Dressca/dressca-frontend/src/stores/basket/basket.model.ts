export interface Item {
  name: string;
  price: number;
  productCode: string;
  imageId: string;
}

export interface Basket extends Item {
  quantity: number;
}

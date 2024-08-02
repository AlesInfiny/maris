import { catalogItemsApi } from '@/api-client';
import type { PostCatalogItemRequest } from '@/generated/api-client';
import { useCatalogStore } from '@/stores/catalog/catalog';

const catalogStore = useCatalogStore();

export async function fetchCategoriesAndBrands() {
  catalogStore.fetchCategories();
  catalogStore.fetchBrands();
}

export async function fetchItems(categoryId: number, brandsId: number) {
  catalogStore.fetchItems(categoryId, brandsId);
}

export async function postCatalogItem(
  name: string,
  description: string,
  price: number,
  productCode: string,
  catalogCategoryId: number,
  catalogBrandId: number,
) {
  const postCatalogItemInput: PostCatalogItemRequest = {
    name: name,
    description: description,
    price: price,
    productCode: productCode,
    catalogCategoryId: catalogCategoryId,
    catalogBrandId: catalogBrandId,
  };
  try {
    await catalogItemsApi.postCatalogItem(postCatalogItemInput);
  } catch (e) {
    throw new Error('Failed to add');
  }
}

import { catalogItemsApi } from '@/api-client';
import type {
  CatalogItemResponse,
  PostCatalogItemRequest,
  PutCatalogItemRequest,
} from '@/generated/api-client';
import { useCatalogStore } from '@/stores/catalog/catalog';

const catalogStore = useCatalogStore();

export async function fetchCategoriesAndBrands() {
  catalogStore.fetchCategories();
  catalogStore.fetchBrands();
}

export async function fetchItems(categoryId: number, brandsId: number) {
  catalogStore.fetchItems(categoryId, brandsId);
}

export async function fetchItem(itemId: number): Promise<CatalogItemResponse> {
  try {
    const itemResponse = await catalogItemsApi.getById(itemId);
    return itemResponse.data;
  } catch (e) {
    throw new Error('Failed to get catalogItem');
  }
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

export async function updateCatalogItem(
  id: number,
  name: string,
  description: string,
  price: number,
  productCode: string,
  catalogCategoryId: number,
  catalogBrandId: number,
) {
  const putCatalogItemRequest: PutCatalogItemRequest = {
    id: id,
    name: name,
    description: description,
    price: price,
    productCode: productCode,
    catalogCategoryId: catalogCategoryId,
    catalogBrandId: catalogBrandId,
  }
  try {
    await catalogItemsApi.putCatalogItem(putCatalogItemRequest);
  } catch (e) {
    throw new Error('Failed to update');
  }
}

export async function deleteCatalogItem(id: number) {
  try {
    await catalogItemsApi.deleteCatalogItem(id);
  } catch (e) {
    throw new Error('Failed to update');
  }
}

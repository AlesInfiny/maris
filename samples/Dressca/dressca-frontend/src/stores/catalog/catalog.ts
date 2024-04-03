import { defineStore } from 'pinia';
import type {
  CatalogCategoryResponse,
  CatalogBrandResponse,
  PagedListOfCatalogItemResponse,
} from '@/generated/api-client';
import {
  catalogCategoriesApi,
  catalogBrandsApi,
  catalogItemsApi,
} from '@/api-client';

export const useCatalogStore = defineStore({
  id: 'catalog',
  state: () => ({
    categories: [] as CatalogCategoryResponse[],
    brands: [] as CatalogBrandResponse[],
    catalogItemPage: {} as PagedListOfCatalogItemResponse,
  }),
  actions: {
    async fetchCategories() {
      const response =
        await catalogCategoriesApi.catalogCategoriesGetCatalogCategories();
      this.categories = response.data;
      this.categories.unshift({ id: 0, name: 'すべて' });
    },
    async fetchBrands() {
      const response = await catalogBrandsApi.catalogBrandsGetCatalogBrands();
      this.brands = response.data;
      this.brands.unshift({ id: 0, name: 'すべて' });
    },
    async fetchItems(categoryId: number, brandId: number, page?: number) {
      const response = await catalogItemsApi.catalogItemsGetByQuery(
        brandId === 0 ? undefined : brandId,
        categoryId === 0 ? undefined : categoryId,
        page,
        undefined,
      );
      this.catalogItemPage = response.data;
    },
  },
  getters: {
    getCategories(state) {
      return state.categories;
    },
    getBrands(state) {
      return state.brands;
    },
    getItems(state) {
      return state.catalogItemPage.items;
    },
    getBrandName: (state) => {
      return (id: number) =>
        state.brands.find((brand) => brand.id === id)?.name;
    },
  },
});

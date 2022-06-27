import { defineStore } from 'pinia';
import axios from 'axios';
import type { CatalogCategoryResponse } from '@/api-client/models/catalog-category-response';
import type { CatalogBrandResponse } from '@/api-client/models/catalog-brand-response';
import type { PagedListOfCatalogItemResponse } from '@/api-client/models/paged-list-of-catalog-item-response';

export const useCatalogStore = defineStore({
  id: 'catalog',
  state: () => ({
    categories: [] as CatalogCategoryResponse[],
    brands: [] as CatalogBrandResponse[],
    catalogItemPage: {} as PagedListOfCatalogItemResponse,
  }),
  actions: {
    async fetchCategories() {
      const response = await axios.get('catalog-categories');
      this.categories = response.data;
      this.categories.unshift({ id: 0, name: 'すべて' });
    },
    async fetchBrands() {
      const response = await axios.get('catalog-brands');
      this.brands = response.data;
      this.brands.unshift({ id: 0, name: 'すべて' });
    },
    async fetchItems(categoryId: number, brandId: number, page?: number) {
      const params = new GetCatalogItemsQuery();
      params.page = page;

      if (categoryId !== 0) {
        params.categoryId = categoryId;
      }

      if (brandId !== 0) {
        params.brandId = brandId;
      }

      const response = await axios.get('catalog-items', { params: params });
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
  },
});

class GetCatalogItemsQuery {
  /**
   * カタログカテゴリIdを取得または設定します。
   * @type {number}
   * @memberof GetCatalogItemsQuery
   */
  categoryId?: number;
  /**
   * カタログブランドIdを取得または設定します。
   * @type {number}
   * @memberof GetCatalogItemsQuery
   */
  brandId?: number;
  /**
   * ページ番号を取得または設定します。
   * @type {number}
   * @memberof GetCatalogItemsQuery
   */
  page?: number;
  /**
   * ページサイズを取得または設定します。
   * @type {number}
   * @memberof GetCatalogItemsQuery
   */
  pageSize?: number;
}

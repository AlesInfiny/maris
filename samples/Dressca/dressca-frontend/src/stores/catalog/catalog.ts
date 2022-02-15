import { defineStore } from 'pinia';
import axios from 'axios';
import type { Category, Brand, Item } from '@/stores/catalog/catalog.model';

export const useCatalogStore = defineStore({
  id: 'catalog',
  state: () => ({
    categories: [] as Category[],
    brands: [] as Brand[],
    items: [] as Item[],
  }),
  actions: {
    async fetchCategories() {
      const response = await axios.get('/categories');
      this.categories = response.data;
    },
    async fetchBrands() {
      const response = await axios.get('/brands');
      this.brands = response.data;
    },
    async fetchItems(categoryId: number, brandId: number) {
      const response = await axios.get('/items');
      this.items = response.data;
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
      return state.items;
    },
  },
});

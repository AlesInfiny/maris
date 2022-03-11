import { defineStore } from 'pinia';
import axios from 'axios';
import type { Basket } from '@/stores/basket/basket.model';

export const useBasketStore = defineStore({
  id: 'bascet',
  state: () => ({
    items: [] as Basket[],
  }),
  actions: {
    async add(productCode: string) {
      const params = { productCode: productCode };
      await axios.put(
        `${import.meta.env.VITE_BACKEND_ENDPOINT_PATH}basket`,
        params,
      );
    },
    async update(productCode: string, quantity: number) {
      const params = { productCode: productCode, quantity: quantity };
      await axios.patch(
        `${import.meta.env.VITE_BACKEND_ENDPOINT_PATH}basket`,
        params,
      );
      const target = this.items.find(
        (item) => item.productCode === productCode,
      );
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      target!.quantity = quantity;
    },
    async remove(productCode: string) {
      const params = { productCode: productCode };
      await axios.delete(
        `${import.meta.env.VITE_BACKEND_ENDPOINT_PATH}basket`,
        { data: params },
      );
      this.items = this.items.filter(
        (item) => item.productCode !== productCode,
      );
    },
    async fetch() {
      const response = await axios.get(
        `${import.meta.env.VITE_BACKEND_ENDPOINT_PATH}basket`,
      );
      this.items = response.data;
    },
  },
  getters: {
    getBasket(state) {
      return state.items;
    },
  },
});

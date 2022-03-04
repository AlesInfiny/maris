import { defineStore } from 'pinia';
import type { Basket } from '@/stores/basket/basket.model';

export const useBasketStore = defineStore({
  id: 'bascet',
  state: () => ({
    items: [] as Basket[],
  }),
  actions: {
    add(productCode: string) {
      const target = this.items.filter(
        (item) => item.productCode === productCode,
      );

      if (target.length === 0) {
        this.items.push({ productCode: productCode, quantity: 1 });
        return;
      }

      target[0].quantity += 1;
    },
    update(productCode: string, quantity: number) {
      const target = this.items.filter(
        (item) => item.productCode === productCode,
      );

      if (target.length === 0) {
        return;
      }

      target[0].quantity = quantity;
    },
    remove(productCode: string) {
      this.items = this.items.filter(
        (item) => item.productCode !== productCode,
      );
    },
  },
  getters: {
    getBasket(state) {
      return state.items;
    },
  },
});

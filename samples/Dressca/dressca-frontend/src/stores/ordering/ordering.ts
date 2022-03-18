import { defineStore } from 'pinia';
import axios from 'axios';
import type { Item } from '@/stores/ordering/ordering.model';

export const useOrderingStore = defineStore({
  id: 'ordering',
  actions: {
    async order(items: Item[]) {
      await axios.post('ordering/order', items);
    },
  },
});

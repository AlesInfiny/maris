import { defineStore } from 'pinia';
import axios from 'axios';
// import type { Basket } from '@/stores/basket/basket.model';
import type { BasketDto } from '@/api-client/models/basket-dto';
import type { BasketItemDto } from '@/api-client/models/basket-item-dto';
import type { PutBasketItemsInputDto } from '@/api-client/models/put-basket-items-input-dto';
import type { PostBasketItemsInputDto } from '@/api-client/models/post-basket-items-input-dto';

export const useBasketStore = defineStore({
  id: 'bascet',
  state: () => ({
    basket: {} as BasketDto,
  }),
  actions: {
    async add(catalogItemId: number) {
      const params: PostBasketItemsInputDto = {
        catalogItemId: catalogItemId,
        addedQuantity: 1,
      };
      await axios.post('basket-items', params);
    },
    async update(catalogItemId: number, newQuantity: number) {
      const params: PutBasketItemsInputDto[] = [
        {
          catalogItemId: catalogItemId,
          quantity: newQuantity,
        },
      ];
      await axios.put('basket-items', params);
    },
    async remove(catalogItemId: number) {
      await axios.delete(`/basket-items/${catalogItemId}`);
    },
    async fetch() {
      const response = await axios.get('basket-items');
      this.basket = response.data;
    },
  },
  getters: {
    getBasket(state) {
      return state.basket;
    },
  },
});

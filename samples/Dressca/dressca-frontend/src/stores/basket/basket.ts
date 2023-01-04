import { defineStore } from 'pinia';
import type {
  BasketResponse,
  PutBasketItemsRequest,
  PostBasketItemsRequest,
} from '@/generated/api-client';
import { basketItemsApi } from '@/api-client';

export const useBasketStore = defineStore({
  id: 'basket',
  state: () => ({
    basket: {} as BasketResponse,
  }),
  actions: {
    async add(catalogItemId: number) {
      const params: PostBasketItemsRequest = {
        catalogItemId: catalogItemId,
        addedQuantity: 1,
      };
      await basketItemsApi.basketItemsPostBasketItem(params);
    },
    async update(catalogItemId: number, newQuantity: number) {
      const params: PutBasketItemsRequest[] = [
        {
          catalogItemId: catalogItemId,
          quantity: newQuantity,
        },
      ];
      await basketItemsApi.basketItemsPutBasketItems(params);
    },
    async remove(catalogItemId: number) {
      await basketItemsApi.basketItemsDeleteBasketItem(catalogItemId);
    },
    async fetch() {
      const response = await basketItemsApi.basketItemsGetBasketItems();
      this.basket = response.data;
    },
  },
  getters: {
    getBasket(state): BasketResponse {
      return state.basket;
    },
  },
});

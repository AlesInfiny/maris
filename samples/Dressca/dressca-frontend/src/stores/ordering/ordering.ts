import { defineStore } from 'pinia';
import axios from 'axios';
import type { OrderDto } from '@/api-client/models/order-dto';
import type { PostOrderInputDto } from '@/api-client/models/post-order-input-dto';

export const useOrderingStore = defineStore({
  id: 'ordering',
  state: () => ({
    lastOrder: undefined as OrderDto | undefined,
  }),
  actions: {
    async order(
      fullName: string,
      postalCode: string,
      todofuken: string,
      shikuchoson: string,
      azanaAndOthers: string,
    ) {
      const postOrderInput: PostOrderInputDto = {
        fullName: fullName,
        postalCode: postalCode,
        todofuken: todofuken,
        shikuchoson: shikuchoson,
        azanaAndOthers: azanaAndOthers,
      };
      const orderResponse = await axios.post('orders', postOrderInput);
      const url = new URL(orderResponse.headers.location);
      const paths = url.pathname.split('/');
      const orderId = paths[paths.length - 1];
      console.log(orderId);
      const orderResultResponse = await axios.get(`orders/${orderId}`);
      this.lastOrder = orderResultResponse.data;
    },
    clearLastOrder() {
      this.lastOrder = undefined;
    },
  },
  getters: {
    getLastOrder(state) {
      return state.lastOrder;
    },
  },
});

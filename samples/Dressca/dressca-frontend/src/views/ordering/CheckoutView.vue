<script setup lang="ts">
import { onMounted, reactive, toRefs } from 'vue';
import { useBasketStore } from '@/stores/basket/basket';
import type { Basket } from '@/stores/basket/basket.model';
import { useAccountStore } from '@/stores/account/account';
import type { Item } from '@/stores/ordering/ordering.model';
import { useOrderingStore } from '@/stores/ordering/ordering';
import { useRouter } from 'vue-router';

const accountStore = useAccountStore();

const state = reactive({
  items: [] as Basket[],
  deliveryCharge: 500,
  total: 0,
  address: accountStore.getAddress,
});

const { items, deliveryCharge, total, address } = toRefs(state);
const router = useRouter();

const getImageUrl = (imageId: string) => {
  return `${import.meta.env.VITE_ASSET_URL}${imageId}`;
};

const toLocaleString = (price: number) => {
  return price.toLocaleString('ja-JP', { style: 'currency', currency: 'JPY' });
};

const orderingStore = useOrderingStore();

const checkout = async () => {
  const items = state.items.map<Item>((item) => ({
    productCode: item.productCode,
    quantity: item.quantity,
  }));
  await orderingStore.order(items);
  router.push({ name: 'ordering/done' });
};

const basketStore = useBasketStore();

onMounted(async () => {
  await basketStore.fetch();
  state.items = basketStore.getBasket;
  state.total = state.items.reduce((p, c) => p + c.price * c.quantity, 0);
});
</script>

<template>
  <div class="container mx-auto my-4 max-w-4xl">
    <div
      class="mx-2 grid grid-cols-1 lg:grid-cols-3 lg:gap-x-12 flex items-center"
    >
      <button
        class="lg:col-end-3 mx-auto w-36 bg-orange-500 hover:bg-amber-700 text-white font-bold py-2 px-4 rounded"
        @click="checkout()"
      >
        注文を確定する
      </button>
      <table
        class="lg:row-start-1 lg:col-span-1 table-fixed mt-2 lg:mt-0 border-t border-b lg:border"
      >
        <tbody>
          <tr>
            <td>商品合計</td>
            <td class="text-right">{{ toLocaleString(total) }}</td>
          </tr>
          <tr>
            <td>送料</td>
            <td class="text-right">{{ toLocaleString(deliveryCharge) }}</td>
          </tr>
          <tr>
            <td>合計金額</td>
            <td class="text-right text-xl font-bold text-red-500">
              {{ toLocaleString(total + deliveryCharge) }}
            </td>
          </tr>
        </tbody>
      </table>
      <table
        class="lg:col-span-3 table-fixed mt-2 lg:mt-4 border-t border-b lg:border"
      >
        <tbody>
          <tr>
            <td rowspan="3" class="w-24 pl-2 border-r">お届け先</td>
            <td class="pl-2">{{ address.name }}</td>
          </tr>
          <tr>
            <td class="pl-2">{{ `〒${address.zipCode}` }}</td>
          </tr>
          <tr>
            <td class="pl-2">{{ address.address }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="mt-8 mx-2">
      <div
        v-for="item in items"
        :key="item.productCode"
        class="grid grid-cols-5 lg:grid-cols-8 mt-4 flex items-center"
      >
        <div class="col-span-4 lg:col-span-5">
          <div class="grid grid-cols-2">
            <img
              :src="getImageUrl(item.imageId)"
              class="h-[150px] pointer-events-none"
            />
            <div class="ml-2">
              <p>{{ item.name }}</p>
              <p class="mt-4">{{ `価格: ${toLocaleString(item.price)}` }}</p>
              <p class="mt-4">{{ `数量: ${item.quantity}` }}</p>
              <p class="mt-4">
                {{ toLocaleString(item.price * item.quantity) }}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

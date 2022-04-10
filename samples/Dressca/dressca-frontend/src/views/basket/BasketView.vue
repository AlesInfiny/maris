<script setup lang="ts">
import { onMounted, reactive, toRefs } from 'vue';
import { useBasketStore } from '@/stores/basket/basket';
import type { Basket } from '@/stores/basket/basket.model';
import { useRouter } from 'vue-router';
import BasketItem from '@/components/basket/BasketItem.vue';

const props = defineProps<{
  productCode?: string;
}>();

const state = reactive({
  items: [] as Basket[],
  added: null as Basket | null,
});

const { items, added } = toRefs(state);
const router = useRouter();

const getImageUrl = (imageId: string) => {
  return `${import.meta.env.VITE_ASSET_URL}${imageId}`;
};

const toLocaleString = (price: number) => {
  return price.toLocaleString('ja-JP', { style: 'currency', currency: 'JPY' });
};

const isEmpty = () => {
  return state.items.length === 0;
};

const goCatalog = () => {
  router.push({ name: 'catalog' });
};

const update = async (productCode: string, quantity: number) => {
  await basketStore.update(productCode, quantity);
  state.items = basketStore.getBasket;
};

const remove = async (productCode: string) => {
  await basketStore.remove(productCode);
  state.items = basketStore.getBasket;
};

const order = () => {
  router.push({ name: 'ordering/checkout' });
};

const basketStore = useBasketStore();

onMounted(async () => {
  if (props.productCode) {
    basketStore.add(props.productCode);
  }

  await basketStore.fetch();
  state.items = JSON.parse(JSON.stringify(basketStore.getBasket));

  if (props.productCode) {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    state.added = state.items.find(
      (item) => item.productCode === props.productCode,
    )!;
  }
});
</script>

<template>
  <div class="container mx-auto my-4 max-w-4xl">
    <div v-if="props.productCode && !!added" class="mx-2">
      <span class="text-lg font-medium text-green-500"
        >以下の商品が追加されました。</span
      >
      <div class="grid grid-cols-1 lg:grid-cols-3 mt-4 flex items-center">
        <img
          :src="getImageUrl(added.imageId)"
          class="h-[150px] m-auto pointer-events-none"
        />
        <span class="text-center lg:text-left">{{ added.name }}</span>
        <span class="text-center lg:text-left">{{
          toLocaleString(added.price)
        }}</span>
      </div>
    </div>
    <div v-if="isEmpty()" class="mt-4 mx-2">
      <span class="text-2xl font-medium">買い物かごに商品がありません。</span>
    </div>
    <div v-if="!isEmpty()" class="mt-8 mx-2">
      <span class="text-2xl font-medium">現在のカートの中身</span>
      <div
        class="hidden lg:grid grid-cols-1 lg:grid-cols-5 mt-4 flex items-center"
      >
        <div class="text-lg font-medium text-center lg:col-span-3">商品</div>
        <div class="text-lg font-medium text-right lg:col-span-1">数量</div>
      </div>
      <div
        v-for="item in items"
        :key="item.productCode"
        class="grid grid-cols-5 lg:grid-cols-8 mt-4 flex items-center"
      >
        <BasketItem :item="item" @update="update" @remove="remove"></BasketItem>
      </div>
    </div>
    <div class="flex justify-between">
      <button
        class="w-36 mt-4 ml-4 bg-teal-500 hover:bg-teal-700 text-white font-bold py-2 px-4 rounded"
        @click="goCatalog()"
      >
        買い物を続ける
      </button>
      <span v-if="!isEmpty()">
        <button
          class="w-36 mt-4 mr-4 bg-orange-500 hover:bg-amber-700 text-white font-bold py-2 px-4 rounded"
          @click="order()"
        >
          レジに進む
        </button>
      </span>
    </div>
  </div>
</template>

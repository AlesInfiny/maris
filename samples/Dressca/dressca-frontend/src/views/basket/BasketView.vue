<script setup lang="ts">
import { onMounted, reactive, toRefs } from 'vue';
import { useBasketStore } from '@/stores/basket/basket';
import type { Basket } from '@/stores/basket/basket.model';
import { useRouter } from 'vue-router';
import { TrashIcon } from '@heroicons/vue/outline';

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
  state.added = null;
  await basketStore.update(productCode, quantity);
  state.items = basketStore.getBasket;
};

const remove = async (productCode: string) => {
  state.added = null;
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
  state.items = basketStore.getBasket;

  if (props.productCode) {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    state.added = basketStore.getBasket.find(
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
        v-for="(item, index) in items"
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
              <p class="mt-4">{{ toLocaleString(item.price) }}</p>
            </div>
          </div>
        </div>
        <div class="lg:col-span-3">
          <div class="grid grid-cols-1 lg:grid-cols-3">
            <div
              class="lg:col-span-2 grid place-items-end lg:flex lg:flex-row lg:items-center"
            >
              <p class="basis-3/5 mt-2 ml-2 mr-2 lg:pr-10 text-right">
                <input
                  :id="`quantity[${index}]`"
                  v-model="item.quantity"
                  type="number"
                  required
                  min="1"
                  class="w-full px-4 py-2 border-b focus:outline-none focus:border-b-2 focus:border-indigo-500 placeholder-gray-500 placeholder-opacity-50"
                />
              </p>
              <div class="basis-2/5">
                <button
                  class="w-12 mt-2 mr-2 bg-transparent hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 border border-blue-500 hover:border-transparent rounded"
                  @click="update(item.productCode, item.quantity)"
                >
                  更新
                </button>
              </div>
            </div>
            <div class="mt-2 mb-1 ml-4 mr-2 grid place-items-end">
              <TrashIcon
                class="h-8 w-8 text-gray-500 hover:text-gray-700"
                @click="remove(item.productCode)"
              />
            </div>
          </div>
        </div>
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

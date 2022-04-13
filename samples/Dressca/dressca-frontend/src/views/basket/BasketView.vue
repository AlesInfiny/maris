<script setup lang="ts">
import { onMounted, reactive, toRefs } from 'vue';
import { useBasketStore } from '@/stores/basket/basket';
import type { BasketDto } from '@/api-client/models/basket-dto';
import type { BasketItemDto } from '@/api-client/models/basket-item-dto';
import { useRouter } from 'vue-router';
import { TrashIcon } from '@heroicons/vue/outline';

const props = defineProps<{
  catalogItemId?: number;
}>();

const state = reactive({
  basket: {} as BasketDto,
  added: null as BasketItemDto | null,
});

const { basket, added } = toRefs(state);
const router = useRouter();

const getFirstImageUrl = (assetCodes: string[] | undefined) => {
  if (
    typeof assetCodes === 'undefined' ||
    assetCodes == null ||
    assetCodes.length === 0
  ) {
    // TODO: Now printingな画像にしたい。
    return '＃';
  }
  return getImageUrl(assetCodes[0]);
};

const getImageUrl = (assetCode: string) => {
  if (assetCode === '') {
    // TODO: Now printingな画像にしたい。
    return '＃';
  }
  return `${import.meta.env.VITE_ASSET_URL}${assetCode}`;
};

const toLocaleString = (price: number | undefined) => {
  if (typeof price === 'undefined') {
    return '-';
  }
  return price.toLocaleString('ja-JP', { style: 'currency', currency: 'JPY' });
};

const isEmpty = () => {
  return (
    typeof state.basket.basketItems === 'undefined' ||
    state.basket.basketItems.length === 0
  );
};

const goCatalog = () => {
  router.push({ name: 'catalog' });
};

const update = async (catalogItemId: number, newQuantity: number) => {
  state.added = null;
  await basketStore.update(catalogItemId, newQuantity);
  await basketStore.fetch();
  state.basket = basketStore.getBasket;
};

const remove = async (catalogItemId: number) => {
  state.added = null;
  await basketStore.remove(catalogItemId);
  await basketStore.fetch();
  state.basket = basketStore.getBasket;
};

const order = () => {
  router.push({ name: 'ordering/checkout' });
};

const basketStore = useBasketStore();

onMounted(async () => {
  if (props.catalogItemId) {
    await basketStore.add(props.catalogItemId);
  }

  await basketStore.fetch();
  state.basket = basketStore.getBasket;

  if (
    props.catalogItemId &&
    typeof basketStore.getBasket.basketItems !== 'undefined'
  ) {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    state.added = basketStore.getBasket.basketItems.find(
      (item) => item.catalogItemId === props.catalogItemId,
    )!;
  }
});
</script>

<template>
  <div class="container mx-auto my-4 max-w-4xl">
    <div v-if="props.catalogItemId && !!added" class="mx-2">
      <span class="text-lg font-medium text-green-500"
        >以下の商品が追加されました。</span
      >
      <div class="grid grid-cols-1 lg:grid-cols-3 mt-4 flex items-center">
        <img
          :src="getFirstImageUrl(added.catalogItem?.assetCodes)"
          class="h-[150px] m-auto pointer-events-none"
        />
        <span class="text-center lg:text-left">{{
          added.catalogItem?.name
        }}</span>
        <span class="text-center lg:text-left">{{
          toLocaleString(added.unitPrice)
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
        v-for="(item, index) in basket.basketItems"
        :key="item.catalogItemId"
        class="grid grid-cols-5 lg:grid-cols-8 mt-4 flex items-center"
      >
        <div class="col-span-4 lg:col-span-5">
          <div class="grid grid-cols-2">
            <img
              :src="getFirstImageUrl(item.catalogItem?.assetCodes)"
              class="h-[150px] pointer-events-none"
            />
            <div class="ml-2">
              <p>{{ item.catalogItem?.name }}</p>
              <p class="mt-4">{{ toLocaleString(item.unitPrice) }}</p>
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
                  @click="update(item.catalogItemId, item.quantity)"
                >
                  更新
                </button>
              </div>
            </div>
            <div class="mt-2 mb-1 ml-4 mr-2 grid place-items-end">
              <TrashIcon
                class="h-8 w-8 text-gray-500 hover:text-gray-700"
                @click="remove(item.catalogItemId)"
              />
            </div>
          </div>
          <div class="text-right mt-4 mr-3">
            小計： <span>{{ toLocaleString(item.subTotal) }}</span>
          </div>
        </div>
      </div>
      <hr class="mt-4" />
      <div class="mt-4 mr-2 text-right">
        <table class="inline-block border-separate">
          <tr>
            <th>税抜き合計</th>
            <td>{{ toLocaleString(basket.account?.totalItemsPrice) }}</td>
          </tr>
          <tr>
            <th>送料</th>
            <td>{{ toLocaleString(basket.account?.deliveryCharge) }}</td>
          </tr>
          <tr>
            <th>消費税</th>
            <td>{{ toLocaleString(basket.account?.consumptionTax) }}</td>
          </tr>
          <tr>
            <th>合計</th>
            <td class="">{{ toLocaleString(basket.account?.totalPrice) }}</td>
          </tr>
        </table>
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

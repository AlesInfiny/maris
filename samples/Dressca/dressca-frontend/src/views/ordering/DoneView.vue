<script setup lang="ts">
import { onMounted, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { useOrderingStore } from '@/stores/ordering/ordering';
import type { OrderDto } from '@/api-client/models/order-dto';

const orderingStore = useOrderingStore();
const router = useRouter();
const state = reactive({
  lastOrdered: null as OrderDto | null,
});

const goCatalog = () => {
  router.push({ name: 'catalog' });
};

const getFirstImageUrl = (assetCodes: string[] | undefined) => {
  if (
    typeof assetCodes === 'undefined' ||
    assetCodes == null ||
    assetCodes.length === 0
  ) {
    return `${import.meta.env.VITE_NO_ASSET_URL}`;
  }
  return getImageUrl(assetCodes[0]);
};

const getImageUrl = (assetCode: string) => {
  if (assetCode === '') {
    return `${import.meta.env.VITE_NO_ASSET_URL}`;
  }
  return `${import.meta.env.VITE_ASSET_URL}${assetCode}`;
};

onMounted(() => {
  const lastOrder = orderingStore.getLastOrder;
  if (typeof lastOrder === 'undefined') {
    router.push('/');
    return;
  }
  state.lastOrdered = lastOrder;
  orderingStore.clearLastOrder();
});
</script>

<template>
  <div class="container mx-auto my-4 max-w-4xl">
    <span class="text-lg font-medium text-green-500">
      注文が完了しました。
    </span>
  </div>
  <div class="container mx-auto my-4 max-w-4xl">
    <div
      class="mx-2 grid grid-cols-1 lg:grid-cols-3 lg:gap-x-12 flex items-center"
    >
      <table
        class="lg:row-start-1 lg:col-span-1 table-fixed mt-2 lg:mt-0 border-t border-b lg:border"
      >
        <tbody>
          <tr>
            <td>税抜き合計</td>
            <td class="text-right">
              {{
                $filters.toCurrencyJPY(
                  state.lastOrdered?.account?.totalItemsPrice,
                )
              }}
            </td>
          </tr>
          <tr>
            <td>送料</td>
            <td class="text-right">
              {{
                $filters.toCurrencyJPY(
                  state.lastOrdered?.account?.deliveryCharge,
                )
              }}
            </td>
          </tr>
          <tr>
            <td>消費税</td>
            <td class="text-right">
              {{
                $filters.toCurrencyJPY(
                  state.lastOrdered?.account?.consumptionTax,
                )
              }}
            </td>
          </tr>
          <tr>
            <td>合計</td>
            <td class="text-right text-xl font-bold text-red-500">
              {{
                $filters.toCurrencyJPY(state.lastOrdered?.account?.totalPrice)
              }}
            </td>
          </tr>
        </tbody>
      </table>
      <table
        class="lg:col-span-2 table-fixed mt-2 lg:mt-4 border-t border-b lg:border"
      >
        <tbody>
          <tr>
            <td rowspan="5" class="w-24 pl-2 border-r">お届け先</td>
            <td class="pl-2">{{ state.lastOrdered?.fullName }}</td>
          </tr>
          <tr>
            <td class="pl-2">{{ `〒${state.lastOrdered?.postalCode}` }}</td>
          </tr>
          <tr>
            <td class="pl-2">{{ state.lastOrdered?.todofuken }}</td>
          </tr>
          <tr>
            <td class="pl-2">{{ state.lastOrdered?.shikuchoson }}</td>
          </tr>
          <tr>
            <td class="pl-2">{{ state.lastOrdered?.azanaAndOthers }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="mt-8 mx-2">
      <div
        v-for="item in state.lastOrdered?.orderItems"
        :key="item.itemOrdered?.id"
        class="grid grid-cols-5 lg:grid-cols-8 mt-4 flex items-center"
      >
        <div class="col-span-4 lg:col-span-5">
          <div class="grid grid-cols-2">
            <img
              :src="getFirstImageUrl(item.itemOrdered?.assetCodes)"
              class="h-[150px] pointer-events-none"
            />
            <div class="ml-2">
              <p>{{ item.itemOrdered?.name }}</p>
              <p class="mt-4">
                {{ `価格: ${$filters.toCurrencyJPY(item.unitPrice)}` }}
              </p>
              <p class="mt-4">{{ `数量: ${item.quantity}` }}</p>
              <p class="mt-4">
                {{ $filters.toCurrencyJPY(item.subTotal) }}
              </p>
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
    </div>
  </div>
</template>

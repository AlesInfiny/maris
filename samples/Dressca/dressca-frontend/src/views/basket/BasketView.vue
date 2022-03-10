<script setup lang="ts">
import { onMounted, reactive, toRefs } from 'vue';
import { useBasketStore } from '@/stores/basket/basket';
import type { Basket } from '@/stores/basket/basket.model';

const props = defineProps<{
  productCode?: string;
}>();

const state = reactive({
  items: [] as Basket[],
});

const { items } = toRefs(state);

const getImageUrl = () => {
  const item = basketStore.getBasket.find(
    (item) => item.productCode === props.productCode,
  );

  if (!item) {
    return;
  }

  return `${import.meta.env.VITE_ASSET_URL}${item.imageId}`;
};

const basketStore = useBasketStore();

onMounted(async () => {
  if (props.productCode) {
    basketStore.add(props.productCode);
  }

  await basketStore.fetch();
  state.items = basketStore.getBasket;
});
</script>

<template>
  <div class="container mx-auto max-w-4xl">
    <div v-if="props.productCode && !!items" class="mt-4">
      <span class="text-lg font-medium text-green-500"
        >以下の商品が追加されました。</span
      >
      <div class="h-[150px]">
        <img :src="getImageUrl()" class="h-full m-auto pointer-events-none" />
      </div>
    </div>
  </div>
</template>

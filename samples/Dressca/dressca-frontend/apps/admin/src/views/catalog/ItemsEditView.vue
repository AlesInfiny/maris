<script setup lang="ts">
import { onMounted, reactive, toRefs } from 'vue';
import {
  fetchItem,
  updateCatalogItem,
  deleteCatalogItem,
} from '@/services/catalog/catalog-service';
import type { CatalogItemResponse } from '@/generated/api-client/models/catalog-item-response';
import assetHelper from '@/shared/helpers/assetHelper';

const props = defineProps<{
  itemId: number;
}>();

interface itemState {
  id: number;
  name: string;
  description: string;
  price: number;
  productCode: string;
  categoryId: number;
  brandId: number;
  assetCodes: string[] | undefined;
}

const state: itemState = reactive({
  id: 0,
  name: '',
  description: '',
  price: 0,
  productCode: '',
  categoryId: 0,
  brandId: 0,
  assetCodes: [],
});

const { getFirstAssetUrl } = assetHelper();

const updateItem = async () => {
  await updateCatalogItem(
    state.id,
    state.name,
    state.description,
    state.price,
    state.productCode,
    state.categoryId,
    state.brandId,
  );
};

const deleteItem = async () => {
  await deleteCatalogItem(state.id);
};

onMounted(async () => {
  const item = await fetchItem(props.itemId);
  state.id = item.id;
  state.name = item.name;
  state.description = item.description;
  state.price = item.price;
  state.productCode = item.productCode;
  state.categoryId = item.catalogCategoryId;
  state.brandId = item.catalogBrandId;
  state.assetCodes = item.assetCodes;
});
</script>

<template>
  <div class="container mx-auto flex-col justify-center">
    <div class="text-xl">カタログアイテム編集</div>
    <form>
      <div class="mb-4">
        <label for="item-id" class="block font-bold mb-2">アイテムID</label>
        <input
          id="item-name"
          v-model="state.id"
          type="text"
          name="item-name"
          class="border border-gray-300 px-4 py-2 w-full"
        />
      </div>
      <div class="mb-4">
        <label for="item-name" class="block font-bold mb-2">アイテム名</label>
        <input
          id="item-name"
          v-model="state.name"
          type="text"
          name="item-name"
          class="border border-gray-300 px-4 py-2 w-full"
        />
      </div>
      <div class="mb-4">
        <label for="description" class="block font-bold mb-2">説明</label>
        <textarea
          id="description"
          v-model="state.description"
          name="description"
          class="border border-gray-300 px-4 py-2 w-full"
        ></textarea>
      </div>
      <div class="mb-4">
        <label for="unit-price" class="block font-bold mb-2">単価</label>
        <input
          id="unit-price"
          v-model="state.price"
          type="number"
          name="unit-price"
          class="border border-gray-300 px-4 py-2 w-full"
        />
      </div>
      <div class="mb-4">
        <label for="product-code" class="block font-bold mb-2"
          >商品コード</label
        >
        <input
          id="product-code"
          v-model="state.productCode"
          name="product-code"
          class="border border-gray-300 px-4 py-2 w-full"
        />
      </div>
      <div class="mb-4">
        <label for="category" class="block font-bold mb-2">カテゴリ</label>
        <select
          id="category"
          v-model="state.categoryId"
          name="category"
          class="border border-gray-300 px-4 py-2 w-full"
        >
          <option value="1">カテゴリ1</option>
          <option value="2">カテゴリ2</option>
          <option value="3">カテゴリ3</option>
        </select>
      </div>
      <div class="mb-4">
        <label for="brand" class="block font-bold mb-2">ブランド</label>
        <select
          id="brand"
          v-model="state.brandId"
          name="brand"
          class="border border-gray-300 px-4 py-2 w-full"
        >
          <option value="1">ブランド1</option>
          <option value="2">ブランド2</option>
          <option value="3">ブランド3</option>
        </select>
      </div>
      <div class="mb-4">
        <label for="item-id" class="block font-bold mb-2">画像</label>
        <img class="h-[100px]" :src="getFirstAssetUrl(state.assetCodes)" />
      </div>
      <button
        type="submit"
        class="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded"
        @click="deleteItem"
      >
        削除
      </button>
      <button
        type="submit"
        class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        @click="updateItem"
      >
        更新
      </button>
    </form>
  </div>
</template>

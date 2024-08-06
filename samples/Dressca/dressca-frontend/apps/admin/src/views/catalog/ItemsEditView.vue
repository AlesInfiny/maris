<script setup lang="ts">
import { onMounted, reactive } from 'vue';
import { storeToRefs } from 'pinia';
import {
  fetchItem,
  updateCatalogItem,
  deleteCatalogItem,
  fetchCategoriesAndBrands,
} from '@/services/catalog/catalog-service';
import assetHelper from '@/shared/helpers/assetHelper';
import { useCatalogStore } from '@/stores/catalog/catalog';

const props = defineProps<{
  itemId: number;
}>();

const catalogStore = useCatalogStore();
const { getCategories, getBrands } = storeToRefs(catalogStore);
const { getFirstAssetUrl } = assetHelper();

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
  await fetchCategoriesAndBrands();
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
  <div
    class="container mx-auto flex flex-col items-center justify-center gap-6"
  >
    <div class="text-xl font-bold">カタログアイテム編集</div>
    <form>
      <div class="mb-4">
        <label for="item-id" class="mb-2 block font-bold">アイテムID</label>
        <input
          id="item-name"
          v-model.number="state.id"
          type="text"
          name="item-name"
          class="w-full border border-gray-300 px-4 py-2"
        />
      </div>
      <div class="mb-4">
        <label for="item-name" class="mb-2 block font-bold">アイテム名</label>
        <input
          id="item-name"
          v-model="state.name"
          type="text"
          name="item-name"
          class="w-full border border-gray-300 px-4 py-2"
        />
      </div>
      <div class="mb-4">
        <label for="description" class="mb-2 block font-bold">説明</label>
        <textarea
          id="description"
          v-model="state.description"
          name="description"
          class="w-full border border-gray-300 px-4 py-2"
        ></textarea>
      </div>
      <div class="mb-4">
        <label for="unit-price" class="mb-2 block font-bold">単価</label>
        <input
          id="unit-price"
          v-model.number="state.price"
          name="unit-price"
          class="w-full border border-gray-300 px-4 py-2"
        />
      </div>
      <div class="mb-4">
        <label for="product-code" class="mb-2 block font-bold"
          >商品コード</label
        >
        <input
          id="product-code"
          v-model="state.productCode"
          name="product-code"
          class="w-full border border-gray-300 px-4 py-2"
        />
      </div>
      <div class="mb-4">
        <label for="category" class="mb-2 block font-bold">カテゴリ</label>
        <select
          id="category"
          v-model.number="state.categoryId"
          name="category"
          class="w-full border border-gray-300 px-4 py-2"
        >
          <option
            v-for="category in getCategories.filter(
              (category) => category.id !== 0,
            )"
            :key="category.id"
            :value="category.id"
          >
            {{ category.name }}
          </option>
        </select>
      </div>
      <div class="mb-4">
        <label for="brand" class="mb-2 block font-bold">ブランド</label>
        <select
          id="brand"
          v-model.number="state.brandId"
          name="brand"
          class="w-full border border-gray-300 px-4 py-2"
        >
          <option
            v-for="brand in getBrands.filter((brand) => brand.id !== 0)"
            :key="brand.id"
            :value="brand.id"
          >
            {{ brand.name }}
          </option>
        </select>
      </div>
      <div class="mb-4">
        <label for="item-id" class="mb-2 block font-bold">画像</label>
        <img class="h-[100px]" :src="getFirstAssetUrl(state.assetCodes)" />
      </div>
      <button
        type="submit"
        class="rounded bg-red-800 px-4 py-2 font-bold text-white hover:bg-red-900"
        @click="deleteItem"
      >
        削除
      </button>
      <button
        type="submit"
        class="rounded bg-light-blue-600 px-4 py-2 font-bold text-white hover:bg-light-blue-800"
        @click="updateItem"
      >
        更新
      </button>
    </form>
  </div>
</template>

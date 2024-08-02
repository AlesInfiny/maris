<script setup lang="ts">
import { onMounted } from 'vue';
import {
  fetchCategoriesAndBrands,
  fetchItems,
} from '@/services/catalog/catalog-service';
import { storeToRefs } from 'pinia';
import { useCatalogStore } from '@/stores/catalog/catalog';
import { currencyHelper } from '@dressca-frontend/common';
import assetHelper from '@/shared/helpers/assetHelper';

const catalogStore = useCatalogStore();
const { getItems } = storeToRefs(catalogStore);

const { toCurrencyJPY } = currencyHelper();
const { getFirstAssetUrl } = assetHelper();

onMounted(async () => {
  fetchCategoriesAndBrands();
  await fetchItems(0, 0);
});
</script>

<template>
  <div class="container mx-auto flex-col justify-center">
    <div class="text-xl">カタログアイテム一覧</div>
    <table
      class="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400 justify-center"
    >
      <thead
        class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400"
      >
        <tr>
          <th class="bg-gray-200 px-4 py-2">アイテムID</th>
          <th class="bg-gray-200 px-4 py-2">画像</th>
          <th class="bg-gray-200 px-4 py-2">アイテム名</th>
          <th class="bg-gray-200 px-4 py-2">説明</th>
          <th class="bg-gray-200 px-4 py-2">単価</th>
          <th class="bg-gray-200 px-4 py-2">商品コード</th>
          <th class="bg-gray-200 px-4 py-2">カテゴリ</th>
          <th class="bg-gray-200 px-4 py-2">ブランド</th>
          <th class="bg-gray-200 px-4 py-2">最終更新日時</th>
          <th class="bg-gray-200 px-4 py-2">操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in getItems" :key="item.id">
          <td class="border px-4 py-2">{{ item.id }}</td>
          <td class="border px-4 py-2">
            <img class="h-[100px]" :src="getFirstAssetUrl(item.assetCodes)" />
          </td>
          <td class="border px-4 py-2">{{ item.name }}</td>
          <td class="border px-4 py-2">{{ item.description }}</td>
          <td class="border px-4 py-2">{{ toCurrencyJPY(item.price) }}</td>
          <td class="border px-4 py-2">{{ item.productCode }}</td>
          <td class="border px-4 py-2">
            {{ catalogStore.getCategoryName(item.catalogCategoryId) }}
          </td>
          <td class="border px-4 py-2">
            {{ catalogStore.getBrandName(item.catalogBrandId) }}
          </td>
          <td class="border px-4 py-2">2024-08-02 15:00:00</td>
          <td class="border px-4 py-2">
            <button
              class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
            >
              編集
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

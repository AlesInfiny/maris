<script setup lang="ts">
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';
import {
  fetchCategoriesAndBrands,
  fetchItems,
} from '@/services/catalog/catalog-service';
import { storeToRefs } from 'pinia';
import { useCatalogStore } from '@/stores/catalog/catalog';
import { currencyHelper } from '@dressca-frontend/common';
import assetHelper from '@/shared/helpers/assetHelper';

const router = useRouter();
const catalogStore = useCatalogStore();
const { getItems } = storeToRefs(catalogStore);

const { toCurrencyJPY } = currencyHelper();
const { getFirstAssetUrl } = assetHelper();

onMounted(async () => {
  fetchCategoriesAndBrands();
  await fetchItems(0, 0);
});

const goEdit = (id: number) => {
  router.push({ name: 'catalog/items/edit', params: { itemId: id } });
};
</script>

<template>
  <div
    class="container mx-auto flex flex-col items-center justify-center gap-6"
  >
    <div class="text-xl font-bold">カタログアイテム一覧</div>
    <table class="table-auto border-separate">
      <thead class="bg-light-blue-50">
        <tr>
          <th class="w-20">アイテムID</th>
          <th class="w-60">画像</th>
          <th>アイテム名</th>
          <th>説明</th>
          <th>単価</th>
          <th>商品コード</th>
          <th class="w-20">カテゴリ</th>
          <th>ブランド</th>
          <th>最終更新日時</th>
          <th class="w-20">操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in getItems" :key="item.id">
          <td class="border">{{ item.id }}</td>
          <td class="border">
            <img
              class="object-contain"
              :src="getFirstAssetUrl(item.assetCodes)"
            />
          </td>
          <td class="border">{{ item.name }}</td>
          <td class="border">{{ item.description }}</td>
          <td class="border">{{ toCurrencyJPY(item.price) }}</td>
          <td class="border">{{ item.productCode }}</td>
          <td class="border">
            {{ catalogStore.getCategoryName(item.catalogCategoryId) }}
          </td>
          <td class="border">
            {{ catalogStore.getBrandName(item.catalogBrandId) }}
          </td>
          <td class="border">2024-08-02 15:00:00</td>
          <td class="border text-center">
            <button
              class="rounded bg-light-blue-600 px-4 py-2 font-bold text-white hover:bg-light-blue-800"
              @click="goEdit(item.id)"
            >
              編集
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

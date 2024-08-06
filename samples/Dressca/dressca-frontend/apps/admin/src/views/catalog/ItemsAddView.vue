<script setup lang="ts">
import { reactive, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useCatalogStore } from '@/stores/catalog/catalog';
import {
  postCatalogItem,
  fetchCategoriesAndBrands,
} from '@/services/catalog/catalog-service';
import { showToast } from '@/services/notification/notificationService';
import { errorHandler } from '@/shared/error-handler/error-handler';

const catalogStore = useCatalogStore();
const { getCategories, getBrands } = storeToRefs(catalogStore);

const state = reactive({
  name: 'テスト用アイテム',
  description: 'テスト用のアイテムです。',
  price: 1980,
  productCode: 'TEST001',
  categoryId: 1,
  brandId: 1,
});

const AddItem = async () => {
  try{
  await postCatalogItem(
    state.name,
    state.description,
    state.price,
    state.productCode,
    state.categoryId,
    state.brandId,
  );
  }catch(error){
    errorHandler(error, () => {
      showToast('カタログアイテムの追加に失敗しました。');
    });
  }
};

onMounted(async () => {
  try{
    await fetchCategoriesAndBrands();
  }catch(error)
  {
    errorHandler(error,()=>{
    showToast('カテゴリとブランド情報の取得に失敗しました。');
    });
  }
  }
);

</script>

<template>
  <div
    class="container mx-auto flex flex-col items-center justify-center gap-6"
  >
    <div class="text-xl font-bold">カタログアイテム追加</div>
    <form>
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
          type="text"
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
        <label for="category" class="mb-2 block font-bold"
          >カテゴリ
          <select
            v-model="state.categoryId"
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
        </label>
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
      <button
        type="submit"
        class="rounded bg-light-blue-600 px-4 py-2 font-bold text-white hover:bg-light-blue-800"
        @click="AddItem"
      >
        追加
      </button>
    </form>
  </div>
</template>

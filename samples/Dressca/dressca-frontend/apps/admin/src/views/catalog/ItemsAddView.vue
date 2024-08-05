<script setup lang="ts">
import { reactive } from 'vue';
import { postCatalogItem } from '@/services/catalog/catalog-service';

const state = reactive({
  name: 'テスト用アイテム',
  description: 'テスト用のアイテムです。',
  price: 1980,
  productCode: 'TEST001',
  categoryId: 1,
  brandId: 1,
});

const AddItem = async () => {
  await postCatalogItem(
    state.name,
    state.description,
    state.price,
    state.productCode,
    state.categoryId,
    state.brandId,
  );
};
</script>

<template>
  <div class="container mx-auto flex-col justify-center">
    <div class="text-xl">カタログアイテム追加</div>
    <form>
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
          v-model.number="state.price"
          type="text"
          name="unit-price"
          class="border border-gray-300 px-4 py-2 w-full"
        />
      </div>
      <div class="mb-4">
        <label for="product-code" class="block font-bold mb-2"
          >商品コード</label
        >
        <select
          id="product-code"
          v-model="state.productCode"
          name="product-code"
          class="border border-gray-300 px-4 py-2 w-full"
        >
          <option value="TEST001">TEST001</option>
          <option value="TEST002">TEST002</option>
          <option value="TEST003">TEST003</option>
        </select>
      </div>
      <div class="mb-4">
        <label for="category" class="block font-bold mb-2">カテゴリ</label>
        <select
          id="category"
          v-model.number="state.categoryId"
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
          v-model.number="state.brandId"
          name="brand"
          class="border border-gray-300 px-4 py-2 w-full"
        >
          <option value="1">ブランド1</option>
          <option value="2">ブランド2</option>
          <option value="3">ブランド3</option>
        </select>
      </div>
      <button
        type="submit"
        class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        @click="AddItem"
      >
        追加
      </button>
    </form>
  </div>
</template>

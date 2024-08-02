<script setup lang="ts"></script>

<template>
  <div class="container mx-auto flex-col justify-center">
    <div class="text-xl">カタログアイテム追加</div>
    <form>
      <div class="mb-4">
        <label for="item-name" class="block font-bold mb-2">アイテム名</label>
        <input
          id="item-name"
          type="text"
          name="item-name"
          class="border border-gray-300 px-4 py-2 w-full"
        />
      </div>
      <div class="mb-4">
        <label for="description" class="block font-bold mb-2">説明</label>
        <textarea
          id="description"
          name="description"
          class="border border-gray-300 px-4 py-2 w-full"
        ></textarea>
      </div>
      <div class="mb-4">
        <label for="unit-price" class="block font-bold mb-2">単価</label>
        <input
          id="unit-price"
          type="number"
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
          name="product-code"
          class="border border-gray-300 px-4 py-2 w-full"
        >
          <option value="code1">コード1</option>
          <option value="code2">コード2</option>
          <option value="code3">コード3</option>
        </select>
      </div>
      <div class="mb-4">
        <label for="category" class="block font-bold mb-2">カテゴリ</label>
        <select
          id="category"
          name="category"
          class="border border-gray-300 px-4 py-2 w-full"
        >
          <option value="category1">カテゴリ1</option>
          <option value="category2">カテゴリ2</option>
          <option value="category3">カテゴリ3</option>
        </select>
      </div>
      <div class="mb-4">
        <label for="brand" class="block font-bold mb-2">ブランド</label>
        <select
          id="brand"
          name="brand"
          class="border border-gray-300 px-4 py-2 w-full"
        >
          <option value="brand1">ブランド1</option>
          <option value="brand2">ブランド2</option>
          <option value="brand3">ブランド3</option>
        </select>
      </div>
      <button
        type="submit"
        class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
      >
        登録
      </button>
    </form>
  </div>
</template>

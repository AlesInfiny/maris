<script setup lang="ts">
import { RouterLink } from 'vue-router';
import { catalogBrandsApi } from '@/api-client';

async function ApiCall() {
  const catalogBrands = await catalogBrandsApi.getCatalogBrands();
  console.log(catalogBrands);
  window.alert(JSON.stringify(catalogBrands));
}
</script>

<template>
  <div class="flex flex-col h-screen justify-between">
    <header>
      <nav
        aria-label="Jump links"
        class="text-lg font-medium text-gray-900 py-5 ring-1 ring-gray-900 ring-opacity-5 shadow-sm"
      >
        <div class="mx-auto flex justify-between px-4 md:px-24 lg:px-24">
          <div><router-link class="text-2xl" to="/">Dressca 管理</router-link></div>
          <div><router-link class="text-2xl" to="/catalog">カタログアイテム一覧</router-link></div>
        </div>
      </nav>
    </header>

    <main class="mb-auto">
      <router-view />
    </main>

    <footer
      class="w-full mx-auto border-t py-4 px-24 text-base bg-black text-gray-500"
    >
      <p>&copy; 2024 - Dressca - Privacy</p>
    </footer>
  </div>
</template>

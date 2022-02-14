<script setup lang="ts">
import { ShoppingCartIcon } from '@heroicons/vue/solid';
</script>

<template>
  <div class="flex flex-col h-screen justify-between">
    <header>
      <nav
        aria-label="Jump links"
        class="sticky top-0 z-40 bg-white text-lg font-medium text-gray-900 py-5 ring-1 ring-gray-900 ring-opacity-5 shadow-sm"
      >
        <div class="max-w-container mx-auto flex justify-between px-24">
          <div>
            <router-link class="text-2xl" to="/">Dressca</router-link>
          </div>
          <div class="flex space-x-5 sm:space-x-8 lg:space-x-12">
            <router-link to="/basket">
              <ShoppingCartIcon class="h-8 w-8 text-amber-600" />
            </router-link>
            <router-link to="/account/login">ログイン</router-link>
          </div>
        </div>
      </nav>
    </header>

    <main class="mb-auto">
      <router-view />
    </main>

    <footer
      class="w-full max-w-container mx-auto border-t py-4 px-24 text-base bg-black text-gray-500"
    >
      <p>&copy; 2020 - Dressca - Privacy</p>
    </footer>
  </div>
</template>

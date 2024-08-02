<script setup lang="ts"></script>
<template>
  <header></header>

  <nav
    id="default-sidebar"
    class="fixed top-0 left-0 z-40 w-64W"
    aria-label="Sidebar"
  >
    <div class="h-full px-3 py-4 overflow-y-auto bg-gray-50 dark:bg-gray-800">
      <ul class="space-y-2 font-medium">
        <li>
          <router-link
            class="flex items-center p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group"
            to="/"
          >
            <span class="ms-3">Dressca 管理</span>
          </router-link>
        </li>
        <li>
          <router-link
            class="flex items-center p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group"
            to="/catalog"
          >
            <span class="flex-1 ms-3 whitespace-nowrap">カタログ管理</span>
          </router-link>
        </li>
      </ul>
    </div>
  </nav>

  <main class="flex mb-auto">
    <router-view />
  </main>
</template>

<script setup lang="ts"></script>
<template>
  <div class="flex gap-16">
    <nav id="default-sidebar" class="mb-auto">
      <div class="overflow-y-auto bg-light-blue-50 px-3 py-4 text-white">
        <ul class="font-bold">
          <li>
            <router-link
              class="flex items-center p-4 text-gray-900 hover:bg-light-blue-800 hover:text-white"
              to="/"
            >
              <span>Dressca 管理</span>
            </router-link>
          </li>
          <li>
            <router-link
              class="flex items-center p-4 text-gray-900 hover:bg-light-blue-800 hover:text-white"
              to="/catalog"
            >
              <span>カタログ管理</span>
            </router-link>
          </li>
        </ul>
      </div>
    </nav>

    <main class="mb-auto">
      <router-view />
    </main>
  </div>
</template>

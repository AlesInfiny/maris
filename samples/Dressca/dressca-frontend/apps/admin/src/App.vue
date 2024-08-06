<script setup lang="ts">
import NotificationToast from '@/components/NotificationToast.vue';
import { storeToRefs } from 'pinia';
import { useAuthenticationStore } from '@/stores/authentication/authentication';

const authenticationStore = useAuthenticationStore();
const { authenticationState, userName, userRole } =
  storeToRefs(authenticationStore);
</script>
<template>
  <div class="z-2">
    <NotificationToast />
  </div>
  <div class="flex gap-16">
    <nav id="default-sidebar" class="mb-auto">
      <div class="overflow-y-auto bg-light-blue-50 px-3 py-4 text-white">
        <ul class="font-bold">
          <li>
            <router-link
              class="flex items-center p-4 text-3xl text-gray-900 hover:bg-light-blue-800 hover:text-white"
              to="/"
            >
              <span>Dressca 管理</span>
            </router-link>
          </li>

          <div v-if="authenticationState">
            <li>
              <router-link
                class="flex items-center p-4 text-xl text-gray-900 hover:bg-light-blue-800 hover:text-white"
                to="/catalog"
              >
                <span>カタログ管理</span>
              </router-link>
            </li>
          </div>

          <div v-if="!authenticationState">
            <li>
              <router-link
                class="flex items-center p-4 text-xl text-gray-900 hover:bg-light-blue-800 hover:text-white"
                to="/authentication/login"
              >
                <span>ログイン</span>
              </router-link>
            </li>
          </div>

          <div v-if="authenticationState">
            <li class="flex items-center p-4 text-xl text-gray-900">
              <span>{{ userName }}</span>
            </li>
            <li class="flex items-center p-4 text-xl text-gray-900">
              <span>{{ userRole }}</span>
            </li>
          </div>
        </ul>
      </div>
    </nav>

    <main class="mb-auto">
      <router-view />
    </main>
  </div>
</template>

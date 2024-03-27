<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { useNotificationStore } from '@/stores/notification/notification';

import { ExclamationCircleIcon, XMarkIcon } from '@heroicons/vue/24/outline';

const notificationStore = useNotificationStore();
const { getMessage } = storeToRefs(notificationStore);

const close = () => {
  notificationStore.clearMessage();
};
</script>

<template>
  <transition
    enter-from-class="translate-y-[-150%] opacity-0"
    leave-to-class="translate-y-[-150%] opacity-0"
    enter-active-class="transition duration-300"
    leave-active-class="transition duration-300"
  >
    <div
      v-if="!(getMessage === '')"
      class="fixed inline-flex items-center w-5/6 inset-x-0 max-w-m mx-auto mt-2 p-4 text-gray-500 bg-red-500 rounded-lg shadow"
    >
      <div
        class="inline-flex items-center justify-center flex-shrink-0 w-8 h-8 text-red-500 bg-red-100 rounded-lg"
      >
        <ExclamationCircleIcon class="w-5 h-5" />
        <span class="sr-only">Error icon</span>
      </div>
      <div class="ms-3 text-white text-sm font-normal">{{ getMessage }}</div>
      <button
        type="button"
        class="ms-auto -mx-1.5 -my-1.5 h-8 w-8 bg-red-100 rounded-lg focus:ring-2 focus:ring-gray-300 hover:bg-gray-100 inline-flex items-center justify-center"
        @click="close"
      >
        <XMarkIcon class="w-5 h-5" />
      </button>
    </div>
  </transition>
</template>

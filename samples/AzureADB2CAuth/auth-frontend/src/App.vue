<script setup lang="ts">
import { useAuthenticationStore } from '@/stores/authentication/authentication';
import { useUserStore } from './stores/user/user';
import { authenticationService } from '@/services/authentication/authentication-service';
import { fetchServerTime } from '@/services/server-time/server-time-service';
import { onMounted, reactive, toRefs } from 'vue';

const authenticationStore = useAuthenticationStore();
const userStore = useUserStore();

const state = reactive({
  serverTime: '',
});
const { serverTime } = toRefs(state);

const signIn = async () => {
  await authenticationService.signIn();
};

const updateServerTime = async () => {
  serverTime.value = await fetchServerTime();
};

onMounted(async () => {
  serverTime.value = await fetchServerTime();
});
</script>

<template>
  <header><h1>Azure AD B2C 認証サンプル</h1></header>
  <div>
    <span>現在時刻: {{ serverTime }}</span>
    <button @click="updateServerTime()">更新</button>
  </div>
  <div>
    <button v-if="!authenticationStore.isAuthenticated" @click="signIn()">
      ログイン
    </button>
    <span v-if="authenticationStore.isAuthenticated"
      >ユーザーID: {{ userStore.getUserId }}</span
    >
  </div>
</template>

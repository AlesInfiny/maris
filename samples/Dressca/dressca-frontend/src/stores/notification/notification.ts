import { defineStore } from 'pinia';

const REMAINING_TIME = 5000;

export const useNotificationStore = defineStore({
  id: 'notification',
  state: () => ({
    message: '',
  }),
  actions: {
    setMessage(message: string) {
      this.message = message;

      setTimeout(() => {
        this.clearMessage();
      }, REMAINING_TIME);
    },

    clearMessage() {
      this.message = '';
    },
  },
  getters: {
    getMessage(state) {
      return state.message;
    },
  },
});

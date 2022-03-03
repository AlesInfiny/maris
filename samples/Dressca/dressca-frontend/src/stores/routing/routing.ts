import { defineStore } from 'pinia';

export const useRoutingStore = defineStore({
  id: 'routing',
  state: () => ({
    redirectFrom: null as null | string,
  }),
  actions: {
    setRedirectFrom(from: string) {
      this.redirectFrom = from;
    },
    redirectToRequestRoute() {
      if (!this.redirectFrom) {
        this.router.push('/');
        return;
      }

      this.router.push({ name: this.redirectFrom });
      this.redirectFrom = null;
    },
  },
});

import { defineStore } from 'pinia';
import type { SpecialContent } from '@/stores/special-content/special-content.model';

export const useSpecialContentStore = defineStore({
  id: 'special-content',
  state: () => ({
    contents: [
      {
        campaignCode: 'LTOX48Q',
        imageId: '1_g0FlA6lGEHHJtluqftq',
      },
      {
        campaignCode: 'EKHQGBB',
        imageId: 'b8uiRXt1UyJ3rji5BoRGB',
      },
      {
        productCode: 'S000000002',
        imageId: 'AFyMr6XPZ-w_qIrXtbzgp',
      },
    ] as SpecialContent[],
  }),
  actions: {},
  getters: {
    getSpecialContents(state) {
      return state.contents;
    },
  },
});

<script setup lang="ts">
import { reactive, toRefs, onMounted, watch } from 'vue';
import { storeToRefs } from 'pinia';
import { useSpecialContentStore } from '@/stores/special-content/special-content';
import { useCatalogStore } from '@/stores/catalog/catalog';
import { VirtualCarousel } from 'vue-virtual-carousel';
import { useRouter } from 'vue-router';

const specialContentStore = useSpecialContentStore();
const catalogStore = useCatalogStore();
const { getSpecialContents } = storeToRefs(specialContentStore);
const { getCategories, getBrands, getItems } = storeToRefs(catalogStore);
const router = useRouter();

const state = reactive({
  selectedCategory: 0,
  selectedBrand: 0,
});

const { selectedCategory, selectedBrand } = toRefs(state);

const toPriceString = (price: number) => {
  return `\\${price.toLocaleString()}`;
};

const getBrandName = (catalogBrandId: number) => {
  return getBrands.value.find((brand) => brand.id === catalogBrandId)?.name;
};

const getImageUrl = (imageId: string) => {
  return `${import.meta.env.VITE_ASSET_URL}${imageId}`;
};

const addBasket = (productCode: string) => {
  // FIXME: add basket
  router.push('/basket');
};

onMounted(() => {
  catalogStore.fetchCategories();
  catalogStore.fetchBrands();
  catalogStore.fetchItems(selectedCategory.value, selectedBrand.value);
});

watch([selectedCategory, selectedBrand], () => {
  catalogStore.fetchItems(selectedCategory.value, selectedBrand.value);
});
</script>

<template>
  <div class="container mx-auto">
    <div class="flex justify-center m-4">
      <VirtualCarousel :items="getSpecialContents" class="h-[350px] w-full">
        <template #default="{ item }">
          <img
            :src="getImageUrl(item.imageId)"
            class="h-full m-auto pointer-events-none"
          />
        </template>
      </VirtualCarousel>
    </div>
    <div class="flex justify-center m-4 text-lg">
      <div>
        <label class="mr-2 font-bold">カテゴリ</label>
        <select v-model="selectedCategory" class="w-48 border-2">
          <option
            v-for="category in getCategories"
            :key="category.id"
            :value="category.id"
          >
            {{ category.name }}
          </option>
        </select>
      </div>
      <div class="ml-10">
        <label class="mr-2 font-bold">ブランド</label>
        <select v-model="selectedBrand" class="w-48 border-2">
          <option v-for="brand in getBrands" :key="brand.id" :value="brand.id">
            {{ brand.name }}
          </option>
        </select>
      </div>
    </div>
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-4">
      <div v-for="item in getItems" :key="item.productCode">
        <div class="justify-center border-2 p-2 h-80 w-240 mx-auto">
          <img
            class="h-180"
            :src="getImageUrl(item.imageIds[0])"
            alt="Sunset in the mountains"
          />
          <div class="w-full">
            <p class="text-md mb-2 w-full">
              {{ getBrandName(item.catalogBrandId) }}
            </p>
            <p class="font-bold text-lg">
              {{ toPriceString(item.price) }}
            </p>
            <div class="mt-4 flex items-center justify-center">
              <button
                class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                @click="addBasket(item.productCode)"
              >
                買い物かごに入れる
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

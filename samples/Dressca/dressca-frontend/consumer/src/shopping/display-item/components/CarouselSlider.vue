<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref } from 'vue'
import { ChevronLeftIcon, ChevronRightIcon, MinusSmallIcon } from '@heroicons/vue/24/solid'

const props = defineProps<{
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  items: any[]
}>()

const MOVE_THRESHOLD = 30

const hasItems = ref(true)
const currentIndex = ref(0)
const startX = ref<number | null>(null)
const diffX = ref(0)

const nextSlide = () => {
  currentIndex.value = (currentIndex.value + 1 + props.items.length) % props.items.length
}

const prevSlide = () => {
  currentIndex.value = (currentIndex.value - 1 + props.items.length) % props.items.length
}

const selectSlide = (index: number) => {
  currentIndex.value = index
}

const isCurrentSlide = (slideNumber: number) => {
  return slideNumber === currentIndex.value + 1
}

const getItem = (index: number) => {
  return props.items[(index + props.items.length) % props.items.length]
}

const onTouchMove = (event: MouseEvent | TouchEvent) => {
  if (startX.value == null) {
    return
  }
  const currentX = 'touches' in event ? event.touches[0]?.clientX : event.clientX
  if (currentX == null) {
    return
  }
  diffX.value = currentX - startX.value
}

const onTouchEnd = () => {
  if (startX.value == null) {
    return
  }
  if (diffX.value > MOVE_THRESHOLD) {
    prevSlide()
  } else if (diffX.value < -MOVE_THRESHOLD) {
    nextSlide()
  }
  startX.value = null
  diffX.value = 0
}

const onTouchStart = (event: MouseEvent | TouchEvent) => {
  startX.value = ('touches' in event ? event.touches[0]?.clientX : event.clientX) ?? null
}

onMounted(() => {
  if (props.items.length === 0) {
    hasItems.value = false
    return
  }
  window.addEventListener('mousemove', onTouchMove)
  window.addEventListener('touchmove', onTouchMove)
  window.addEventListener('mouseup', onTouchEnd)
  window.addEventListener('touchend', onTouchEnd)
})

onBeforeUnmount(() => {
  window.removeEventListener('mousemove', onTouchMove)
  window.removeEventListener('touchmove', onTouchMove)
  window.removeEventListener('mouseup', onTouchEnd)
  window.removeEventListener('touchend', onTouchEnd)
})
</script>

<template>
  <template v-if="hasItems">
    <div data-test="body" class="container touch-pan-y">
      <div
        class="flex items-center justify-center"
        @touchstart.passive="onTouchStart"
        @mousedown="onTouchStart"
      >
        <button
          type="button"
          aria-label="前のスライド"
          class="text-gray-300 hover:text-gray-500"
          @click="prevSlide"
        >
          <ChevronLeftIcon class="h-10 w-10" />
        </button>
        <template v-for="index in props.items.length">
          <template v-if="isCurrentSlide(index)">
            <div
              :key="index"
              data-test="slider"
              :style="{
                transform: `translate3d(${diffX}px, 0, 0)`,
              }"
            >
              <slot v-bind="{ item: getItem(currentIndex) }"></slot>
            </div>
          </template>
        </template>
        <button
          type="button"
          aria-label="次のスライド"
          class="text-gray-300 hover:text-gray-500"
          @click="nextSlide"
        >
          <ChevronRightIcon class="h-10 w-10" />
        </button>
      </div>
      <div class="flex justify-center">
        <button
          v-for="index in props.items.length"
          :key="index"
          type="button"
          :aria-label="`${index} 枚目のスライドを表示`"
          :aria-current="isCurrentSlide(index)"
          :class="isCurrentSlide(index) ? 'text-gray-500' : 'text-gray-300'"
          @click="selectSlide(index - 1)"
        >
          <MinusSmallIcon class="h-10 w-10" />
        </button>
      </div>
    </div>
  </template>
</template>

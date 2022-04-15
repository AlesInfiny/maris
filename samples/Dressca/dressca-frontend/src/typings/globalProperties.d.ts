import type { GlobalFilters } from '@/shared/filters/globalFilters';

declare module '@vue/runtime-core' {
  export interface ComponentCustomProperties {
    $filters: GlobalFilters;
  }
}

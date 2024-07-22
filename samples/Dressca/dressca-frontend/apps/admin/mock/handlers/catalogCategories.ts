import type { CatalogCategoryResponse } from '@/generated/api-client';
import { HttpResponse, http } from 'msw';

export const catalogCategoriesHandlers = [
  http.get('/api/catalog-categories', () => {
    return HttpResponse.json(catalogCategories, { status: 200 });
  }),
];

const catalogCategories: CatalogCategoryResponse[] = [
  {
    id: 1,
    name: '服',
  },
  {
    id: 2,
    name: 'バッグ',
  },
  {
    id: 3,
    name: 'シューズ',
  },
];

import type { CatalogBrandResponse } from '@/generated/api-client';
import { HttpResponse, http } from 'msw';

export const catalogBrandsHandlers = [
  http.get('/api/catalog-brands', () => {
    return HttpResponse.json(catalogBrands, { status: 200 });
  }),
];

const catalogBrands: CatalogBrandResponse[] = [
  {
    id: 1,
    name: '高級なブランド',
  },
  {
    id: 2,
    name: 'カジュアルなブランド',
  },
  {
    id: 3,
    name: 'ノーブランド',
  },
];

import { HttpResponse, http } from 'msw';
import { pagedListCatalogItem, catalogItems } from '../data/catalogItems';

type GetCatalogItemParams = {
  catalogItemId: string;
};

export const catalogItemsHandlers = [
  http.get('/api/catalog-items', () => {
    return HttpResponse.json(pagedListCatalogItem, { status: 200 });
  }),
  http.post('/api/catalog-items', () => {
    return new HttpResponse(null, { status: 201 });
  }),
  http.get<
    GetCatalogItemParams,
    never,
    never,
    '/api/catalog-items/:catalogItemId'
  >('/api/catalog-items/:catalogItemId', async ({ params }) => {
    const { catalogItemId } = params;
    const item = catalogItems.find(
      (items) => items.id === Number(catalogItemId),
    );
    return HttpResponse.json(item, { status: 200 });
  }),
  http.delete('/api/catalog-items/:catalogItemId', () => {
    return new HttpResponse(null, { status: 204 });
  }),
  http.put('/api/catalog-items/:catalogItemId', () => {
    return new HttpResponse(null, { status: 200 });
  }),
];

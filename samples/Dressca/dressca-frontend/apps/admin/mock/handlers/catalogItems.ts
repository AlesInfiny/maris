import { HttpResponse, http } from 'msw';
import { pagedListCatalogItem, catalogItem } from '../data/catalogItems';

export const catalogItemsHandlers = [
  http.get('/api/catalog-items', () => {
    return HttpResponse.json(pagedListCatalogItem, { status: 200 });
  }),
  http.get('/api/catalog-items/1', () => {
    return HttpResponse.json(catalogItem, { status: 200 });
  }),
  http.post('/api/catalog-items/add', () => {
    return HttpResponse.json({}, { status: 201 });
  }),
  http.delete('/api/catalog-items/edit/:itemId', () => {
    return HttpResponse.json({}, { status: 204 });
  }),
];

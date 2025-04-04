import type { Connect } from 'vite';
import { assetMock } from './asset/asset.mock';
import { catalogApiMock } from './catalog/catalog.api.mock';
import { basketApiMock } from './basket/basket.api.mock';
import { orderingApiMock } from './ordering/ordering.api.mock';

export const createMockServer = (middlewares: Connect.Server) => {
  assetMock(middlewares);
  catalogApiMock(middlewares);
  basketApiMock(middlewares);
  orderingApiMock(middlewares);
};

import { assetMock } from './asset/asset.mock';
import { catalogApiMock } from './catalog/catalog.api.mock';
import { basketApiMock } from './basket/basket.api.mock';

export const createMockServer = (middlewares) => {
  assetMock(middlewares);
  catalogApiMock(middlewares);
  basketApiMock(middlewares);
};

import { assetMock } from './asset/asset.mock';
import { catalogApiMock } from './catalog/catalog.api.mock';

export const createMockServer = (middlewares) => {
  assetMock(middlewares);
  catalogApiMock(middlewares);
};

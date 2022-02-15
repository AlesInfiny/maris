import { catalogApiMock } from './catalog/catalog.api.mock';

export const createMockServer = (middlewares) => {
  catalogApiMock(middlewares);
};

import { catalogItemsHandlers } from './handlers/catalogItems';
import { catalogBrandsHandlers } from './handlers/catalogBrands';
import { catalogCategoriesHandlers } from './handlers/catalogCategories';
import { assetsHandlers } from './handlers/assets';

export const handlers = [
  ...catalogItemsHandlers,
  ...catalogBrandsHandlers,
  ...catalogCategoriesHandlers,
  ...assetsHandlers,
];

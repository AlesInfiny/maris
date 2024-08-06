import type {
  CatalogItemResponse,
  PagedListOfCatalogItemResponse,
} from '@/generated/api-client';

export const catalogItem: CatalogItemResponse = {
  id: 1,
  catalogCategoryId: 1,
  catalogBrandId: 3,
  description: '定番の無地ロングTシャツです。',
  name: 'クルーネック Tシャツ - ブラック',
  price: 1980,
  productCode: 'C000000001',
  assetCodes: [],
};

export const pagedListCatalogItem: PagedListOfCatalogItemResponse = {
  hasNext: false,
  hasPrevious: false,
  page: 1,
  pageSize: 20,
  totalCount: 3,
  totalPages: 1,
  items: [
    {
      id: 1,
      catalogCategoryId: 1,
      catalogBrandId: 3,
      description: '定番の無地ロングTシャツです。',
      name: 'クルーネック Tシャツ - ブラック',
      price: 1980,
      productCode: 'C000000001',
      assetCodes: [],
    },
    {
      id: 2,
      catalogCategoryId: 1,
      catalogBrandId: 2,
      description: '暖かいのに着膨れしない起毛デニムです。',
      name: '裏起毛 スキニーデニム',
      price: 4800,
      productCode: 'C000000002',
      assetCodes: ['4aed07c4ed5d45a5b97f11acedfbb601'],
    },
    {
      id: 3,
      catalogCategoryId: 1,
      catalogBrandId: 1,
      description: 'あたたかく肌ざわりも良いウール100%のロングコートです。',
      name: 'ウールコート',
      price: 49800,
      productCode: 'C000000003',
      assetCodes: ['082b37439ecc44919626ba00fc60ee85'],
    },
    {
      id: 4,
      catalogCategoryId: 1,
      catalogBrandId: 2,
      description:
        'コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。',
      name: '無地 ボタンダウンシャツ',
      price: 2800,
      productCode: 'C000000004',
      assetCodes: ['f5f89954281747fa878129c29e1e0f83'],
    },
    {
      id: 5,
      catalogCategoryId: 2,
      catalogBrandId: 3,
      description: 'コンパクトサイズのバッグですが収納力は抜群です。',
      name: 'レザーハンドバッグ',
      price: 18800,
      productCode: 'B000000001',
      assetCodes: ['a8291ef2e8e14869a7048e272915f33c'],
    },
    {
      id: 6,
      catalogCategoryId: 2,
      catalogBrandId: 2,
      description: 'エイジング加工したレザーを使用しています。',
      name: 'ショルダーバッグ',
      price: 38000,
      productCode: 'B000000002',
      assetCodes: ['66237018c769478a90037bd877f5fba1'],
    },
    {
      id: 7,
      catalogCategoryId: 2,
      catalogBrandId: 3,
      description:
        '春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。',
      name: 'トートバッグ ポーチ付き',
      price: 24800,
      productCode: 'B000000003',
      assetCodes: ['d136d4c81b86478990984dcafbf08244'],
    },
    {
      id: 8,
      catalogCategoryId: 2,
      catalogBrandId: 1,
      description: 'さらりと気軽に纏える、キュートなミニサイズショルダー。',
      name: 'ショルダーバッグ',
      price: 2800,
      productCode: 'B000000004',
      assetCodes: ['47183f32f6584d7fb661f9216e11318b'],
    },
    {
      id: 9,
      catalogCategoryId: 2,
      catalogBrandId: 1,
      description: 'エレガントな雰囲気を放つキルティングデザインです。',
      name: 'レザー チェーンショルダーバッグ',
      price: 258000,
      productCode: 'B000000005',
      assetCodes: ['cf151206efd344e1b86854f4aa49fdef'],
    },
    {
      id: 10,
      catalogCategoryId: 3,
      catalogBrandId: 2,
      description: '柔らかいソールは快適な履き心地で、ランニングに最適です。',
      name: 'ランニングシューズ - ブルー',
      price: 12800,
      productCode: 'S000000001',
      assetCodes: ['ab2e78eb7fe3408aadbf1e17a9945a8c'],
    },
    {
      id: 11,
      catalogCategoryId: 3,
      catalogBrandId: 1,
      description: 'イタリアの職人が丁寧に手作業で作り上げた一品です。',
      name: 'メダリオン ストレートチップ ドレスシューズ',
      price: 23800,
      productCode: 'S000000002',
      assetCodes: ['0e557e96bc054f10bc91c27405a83e85'],
    },
  ],
};

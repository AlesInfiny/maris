import { Category, Brand, Item } from '../../src/stores/catalog/catalog.model';

const categories: Category[] = [
  {
    id: 0,
    name: 'すべて',
  },
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

const brands: Brand[] = [
  {
    id: 0,
    name: 'すべて',
  },
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

const items: Item[] = [
  {
    catalogCategoryId: 1,
    catalogBrandId: 3,
    description: '定番の無地ロングTシャツです。',
    name: 'クルーネック Tシャツ - グレー',
    price: 1980,
    productCode: 'C000000001',
  },
  {
    catalogCategoryId: 1,
    catalogBrandId: 2,
    description: '裏起毛で温かいパーカーです。',
    name: '無地 パーカー',
    price: 5800,
    productCode: 'C000000002',
  },
  {
    catalogCategoryId: 1,
    catalogBrandId: 1,
    description:
      'ウール生地のテーラードジャケットです。セットアップだけでなく単体でも使いやすい商品となっています。',
    name: 'テーラードジャケット',
    price: 49800,
    productCode: 'C000000003',
  },
  {
    catalogCategoryId: 1,
    catalogBrandId: 2,
    description: 'ファー襟付きのデニムジャケットです。',
    name: 'デニムジャケット',
    price: 19800,
    productCode: 'C000000004',
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 3,
    description: 'シンプルなデザインのトートバッグです。',
    name: 'トートバッグ',
    price: 18800,
    productCode: 'B000000001',
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 2,
    description: '軽くしなやかなレザーを使用しています。',
    name: 'ショルダーバッグ',
    price: 38000,
    productCode: 'B000000002',
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 3,
    description:
      '外側は高級感のある牛革、内側に丈夫で傷つきにくい豚革を採用した細身で持ち運びやすいビジネスバッグです。',
    name: 'ビジネスバッグ',
    price: 24800,
    productCode: 'B000000003',
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 1,
    description:
      '丁寧に編み込まれた馬革ハンドバッグです。落ち着いた色調で、オールシーズン使いやすくなっています。',
    name: '編み込みトートバッグ',
    price: 58800,
    productCode: 'B000000004',
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 1,
    description:
      '卓越した素材と緻密な縫製作業によって、デザインが完璧に表現されています。',
    name: 'ハンドバッグ',
    price: 258000,
    productCode: 'B000000005',
  },
  {
    catalogCategoryId: 3,
    catalogBrandId: 2,
    description: '定番のハイカットスニーカーです。',
    name: 'ハイカットスニーカー - ブラック',
    price: 12800,
    productCode: 'S000000001',
  },
  {
    catalogCategoryId: 3,
    catalogBrandId: 1,
    description: 'イタリアの職人が丁寧に手作業で作り上げた一品です。',
    name: 'メダリオン ストレートチップ ドレスシューズ',
    price: 23800,
    productCode: 'S000000002',
  },
];

export const catalogApiMock = (middlewares) => {
  middlewares.use('/categories', (_, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.write(JSON.stringify(categories));
    res.end();
  });
  middlewares.use('/brands', (_, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.write(JSON.stringify(brands));
    res.end();
  });
  middlewares.use('/items', (_, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.write(JSON.stringify(items));
    res.end();
  });
};

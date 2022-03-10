const base = 'api';
import * as url from 'url';
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
    description: 'ロングTシャツです。',
    name: 'クルーネック Tシャツ - ブラック',
    price: 1980,
    productCode: 'C000000001',
    imageIds: ['M62iLPiA9bU2DNQDHF3bs'],
  },
  {
    catalogCategoryId: 1,
    catalogBrandId: 2,
    description: '暖かいのに着膨れしない起毛デニムです。',
    name: '裏起毛 スキニーデニム',
    price: 12800,
    productCode: 'C000000002',
    imageIds: ['-MgjYE6rR9NOJXBgZhSVO'],
  },
  {
    catalogCategoryId: 1,
    catalogBrandId: 1,
    description: 'あたたかく肌ざわりも良いウール100%のロングコートです。',
    name: 'ウールコート',
    price: 49800,
    productCode: 'C000000003',
    imageIds: ['nRgpa1AkdnO1z0pa1fQuO'],
  },
  {
    catalogCategoryId: 1,
    catalogBrandId: 2,
    description:
      'コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。',
    name: '無地 ボタンダウンシャツ',
    price: 9800,
    productCode: 'C000000004',
    imageIds: ['SCXsZbePV9aZ5k8Ufglon'],
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 2,
    description: 'コンパクトサイズのバッグですが収納力は抜群です',
    name: 'レザーハンドバッグ',
    price: 18800,
    productCode: 'B000000001',
    imageIds: ['JzlZjf4zdKkbjpFjiwTgN'],
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 1,
    description: 'エイジング加工したレザーを使用しています。',
    name: 'ショルダーバッグ',
    price: 38000,
    productCode: 'B000000002',
    imageIds: ['tMvJnIWXoKwrKv0qe9TUp'],
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 2,
    description:
      '春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。',
    name: 'トートバッグ ポーチ付き',
    price: 24800,
    productCode: 'B000000003',
    imageIds: ['i-QKRrqdpMSTJdBYK8rEV'],
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 3,
    description: 'さらりと気軽に纏える、キュートなミニサイズショルダー。',
    name: 'ショルダーバッグ',
    price: 18800,
    productCode: 'B000000004',
    imageIds: ['qK3ZtQsSU1aGka8gOx30n'],
  },
  {
    catalogCategoryId: 2,
    catalogBrandId: 1,
    description: 'エレガントな雰囲気を放つキルティングデザインです。',
    name: 'レザー チェーンショルダーバッグ',
    price: 258000,
    productCode: 'B000000005',
    imageIds: ['5xEf0R-ZytD7TK48Lctu-'],
  },
  {
    catalogCategoryId: 3,
    catalogBrandId: 2,
    description: '柔らかいソールは快適な履き心地で、ランニングに最適です。',
    name: 'ランニングシューズ - ブルー',
    price: 12800,
    productCode: 'S000000001',
    imageIds: ['NlPn8IhLUgOKn-u9-x8Xv'],
  },
  {
    catalogCategoryId: 3,
    catalogBrandId: 1,
    description: 'イタリアの職人が丁寧に手作業で作り上げた一品です。',
    name: 'メダリオン ストレートチップ ドレスシューズ',
    price: 23800,
    productCode: 'S000000002',
    imageIds: ['HjeaF_8Ci4m0ujUjlmVNS'],
  },
];

export const catalogApiMock = (middlewares) => {
  middlewares.use(`/${base}/categories`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.write(JSON.stringify(categories));
    res.end();
  });
  middlewares.use(`/${base}/brands`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.write(JSON.stringify(brands));
    res.end();
  });
  middlewares.use(`/${base}/items`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    const query = url.parse(_.url, true).query;
    let filterdItems = items;

    if (!!query && !!query.categoryId) {
      filterdItems = filterdItems.filter(
        // 文字列で比較
        (item) => item.catalogCategoryId == query.categoryId,
      );
    }

    if (!!query && !!query.brandId) {
      filterdItems = filterdItems.filter(
        // 文字列で比較
        (item) => item.catalogBrandId == query.brandId,
      );
    }

    res.write(JSON.stringify(filterdItems));
    res.end();
  });
};

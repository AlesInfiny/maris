const base = 'api';
import { Basket } from '../../src/stores/basket/basket.model';

let items: Basket[] = [];

const mockItems: Basket[] = [
  {
    name: 'クルーネック Tシャツ - ブラック',
    price: 1980,
    productCode: 'C000000001',
    imageId: 'M62iLPiA9bU2DNQDHF3bs',
    quantity: 0,
  },
  {
    name: '裏起毛 スキニーデニム',
    price: 12800,
    productCode: 'C000000002',
    imageId: '-MgjYE6rR9NOJXBgZhSVO',
    quantity: 0,
  },
  {
    name: 'ウールコート',
    price: 49800,
    productCode: 'C000000003',
    imageId: 'nRgpa1AkdnO1z0pa1fQuO',
    quantity: 0,
  },
  {
    name: '無地 ボタンダウンシャツ',
    price: 9800,
    productCode: 'C000000004',
    imageId: 'SCXsZbePV9aZ5k8Ufglon',
    quantity: 0,
  },
  {
    name: 'レザーハンドバッグ',
    price: 18800,
    productCode: 'B000000001',
    imageId: 'JzlZjf4zdKkbjpFjiwTgN',
    quantity: 0,
  },
  {
    name: 'ショルダーバッグ',
    price: 38000,
    productCode: 'B000000002',
    imageId: 'tMvJnIWXoKwrKv0qe9TUp',
    quantity: 0,
  },
  {
    name: 'トートバッグ ポーチ付き',
    price: 24800,
    productCode: 'B000000003',
    imageId: 'i-QKRrqdpMSTJdBYK8rEV',
    quantity: 0,
  },
  {
    name: 'ショルダーバッグ',
    price: 18800,
    productCode: 'B000000004',
    imageId: 'qK3ZtQsSU1aGka8gOx30n',
    quantity: 0,
  },
  {
    name: 'レザー チェーンショルダーバッグ',
    price: 258000,
    productCode: 'B000000005',
    imageId: '5xEf0R-ZytD7TK48Lctu-',
    quantity: 0,
  },
  {
    name: 'ランニングシューズ - ブルー',
    price: 12800,
    productCode: 'S000000001',
    imageId: 'NlPn8IhLUgOKn-u9-x8Xv',
    quantity: 0,
  },
  {
    name: 'メダリオン ストレートチップ ドレスシューズ',
    price: 23800,
    productCode: 'S000000002',
    imageId: 'HjeaF_8Ci4m0ujUjlmVNS',
    quantity: 0,
  },
];

export const basketApiMock = (middlewares) => {
  middlewares.use(`/${base}/basket`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    if (_.method === 'GET') {
      res.write(JSON.stringify(items));
      res.end();
      return;
    }

    let body = '';
    _.on('data', (chunk) => (body += chunk));
    _.on('end', () => {
      const productCode = JSON.parse(body).productCode;

      if (_.method === 'DELETE') {
        items = items.filter((item) => item.productCode !== productCode);
        console.log('items: ', items);
        res.end();
        return;
      }

      const target = items.filter((item) => item.productCode === productCode);

      if (_.method === 'PUT') {
        if (target.length === 0) {
          const addItem = mockItems.find(
            (item) => item.productCode === productCode,
          );
          addItem.quantity = 1;
          items.push(addItem);
          res.end();
          return;
        }

        target[0].quantity += 1;
        res.end();
        res.end();
        return;
      }

      // PATCH
      const quantity = JSON.parse(body).quantity;
      target[0].quantity = quantity;

      res.end();
    });
  });
};

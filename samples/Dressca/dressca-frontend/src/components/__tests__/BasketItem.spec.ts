import { describe, it, expect } from 'vitest';
import { mount } from '@vue/test-utils';
import BasketItem from '@/components/basket/BasketItem.vue';
import type { BasketItemResponse } from '../../api-client/models/basket-item-response';

describe('BasketItemコンポーネントのテスト', () => {
  it('小計が日本円形式で表示できる', () => {
    const basketItemResponse: BasketItemResponse = {
      catalogItemId: 10,
      quantity: 2,
      subTotal: 100000,
      unitPrice: 50000,
    };
    const wrapper = mount(BasketItem, { props: { item: basketItemResponse } });
    expect(wrapper.text()).toContain('￥100,000');
  });

  it('単価が日本円形式で表示できる', () => {
    const basketItemResponse: BasketItemResponse = {
      catalogItemId: 11,
      quantity: 3,
      subTotal: 150000,
      unitPrice: 50000,
    };
    const wrapper = mount(BasketItem, { props: { item: basketItemResponse } });
    expect(wrapper.text()).toContain('￥50,000');
  });

  it('商品の画像が正しく表示できる');

  /*
  it('数量入力で文字列を入力するとupdateボタンを押下してもイベントが発行されない', async () => {
    const basketItem: BasketItemResponse = {
      catalogItemId: 1,
      unitPrice: 10000,
      quantity: 3,
      subTotal: 30000,
    };

    const wrapper = mount(BasketItem, { props: { item: basketItem } });

    expect(wrapper.vm.isUpdateDisabled).toBe(false);
    await wrapper.find('input').setValue('abcde');
    expect(wrapper.vm.isUpdateDisabled).toBe(true);
    wrapper.find('[id="updateButton"]').trigger('click');
    expect(wrapper.emitted()).not.toHaveProperty('update');
  });
  */

  /*
  it('数量入力で0以下の数値を入力するとupdateボタンが押下できない', async () => {
    const basketItem: BasketItemResponse = {
      catalogItemId: 1,
      unitPrice: 10000,
      quantity: 3,
      subTotal: 30000,
    };

    const wrapper = mount(BasketItem, { props: { item: basketItem } });
    const button = wrapper.find('[id="updateButton"]');

    expect(wrapper.vm.isUpdateDisabled).toBe(true);
    expect(button.attributes().disabled).toBe('');
    await wrapper.find('input').setValue(4);
    expect(wrapper.vm.isUpdateDisabled).toBe(false);
    expect(button.attributes().disabled).toBeUndefined();
    await wrapper.find('input').setValue(-100);
    expect(button.attributes().disabled).toBe('');
  });
  */

  /*
  it('数量入力で1000以上の数値を入力するとupdateボタンを押下してもイベントが発行されない', async () => {
    const basketItem: BasketItemResponse = {
      catalogItemId: 1,
      unitPrice: 10000,
      quantity: 3,
      subTotal: 30000,
    };

    const wrapper = mount(BasketItem, { props: { item: basketItem } });
    await wrapper.find('input').setValue(1000);
    wrapper.find('[id="updateButton"]').trigger('click');

    expect(wrapper.emitted()).not.toHaveProperty('update');
  });
  */

  it('updateボタン押下時、カタログ番号と変更した数量を引数のupdateイベントをemitする', async () => {
    const basketItem: BasketItemResponse = {
      catalogItemId: 1,
      unitPrice: 10000,
      quantity: 3,
      subTotal: 30000,
    };

    const wrapper = mount(BasketItem, { props: { item: basketItem } });
    const setValue = 4;
    await wrapper.find('input').setValue(setValue);
    wrapper.find('[id="updateButton"]').trigger('click');

    expect(wrapper.emitted()).toHaveProperty('update'); // updateイベントの発火を確認
    expect(wrapper.emitted().update).toHaveLength(1); // updateイベントが1回だけ発火したことを確認
    expect(wrapper.emitted().update[0]).toEqual([
      basketItem.catalogItemId,
      setValue,
    ]); // updateイベントの引数が期待通りか確認
  });

  it('removeボタン押下時、数量が0になりremoveイベントをemitする', async () => {
    const basketItem: BasketItemResponse = {
      catalogItemId: 1,
      unitPrice: 10000,
      quantity: 3,
      subTotal: 30000,
    };

    const wrapper = mount(BasketItem, { props: { item: basketItem } });
    wrapper.find('[id="removeButton"]').trigger('click');

    expect(wrapper.emitted()).toHaveProperty('remove'); // removeイベントの発火を確認
    expect(wrapper.emitted().remove).toHaveLength(1); // removeイベントが1回だけ発火したことを確認
    expect(wrapper.emitted().remove[0]).toEqual([basketItem.catalogItemId]); // removeイベントの引数が期待通りか確認
  });
});

export const globalFilters: GlobalFilters = {
  toCurrencyJPY(price: number | undefined): string {
    if (typeof price === 'undefined') {
      return '-';
    }
    return price.toLocaleString('ja-JP', {
      style: 'currency',
      currency: 'JPY',
    });
  },
};

export interface GlobalFilters {
  toCurrencyJPY(price: number | undefined): string;
}

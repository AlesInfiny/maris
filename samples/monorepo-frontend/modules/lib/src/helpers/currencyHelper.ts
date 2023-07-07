export const toCurrencyJPY = (price: number | undefined) => {
    if (typeof price === 'undefined') {
        return '-';
    }

    return price.toLocaleString('ja-JP', {
        style: 'currency',
        currency: 'JPY',
    });
};
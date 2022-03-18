const base = 'api';

export const orderingApiMock = (middlewares) => {
  middlewares.use(`/${base}/ordering/order`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end();
  });
};

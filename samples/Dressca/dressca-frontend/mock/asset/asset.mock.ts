const base = 'asset';
import * as fs from 'fs';
import * as path from 'path';

const readFile = (file, response) => {
  fs.readFile(path.resolve(__dirname, file), (errors, data) => {
    response.end(data);
  });
};

export const assetMock = (middlewares) => {
  middlewares.use(`/${base}/1_g0FlA6lGEHHJtluqftq`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`1_g0FlA6lGEHHJtluqftq`, res);
  });
  middlewares.use(`/${base}/b8uiRXt1UyJ3rji5BoRGB`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`b8uiRXt1UyJ3rji5BoRGB`, res);
  });
  middlewares.use(`/${base}/AFyMr6XPZ-w_qIrXtbzgp`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`AFyMr6XPZ-w_qIrXtbzgp`, res);
  });
  middlewares.use(`/${base}/M62iLPiA9bU2DNQDHF3bs`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`M62iLPiA9bU2DNQDHF3bs`, res);
  });
  middlewares.use(`/${base}/-MgjYE6rR9NOJXBgZhSVO`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`-MgjYE6rR9NOJXBgZhSVO`, res);
  });
  middlewares.use(`/${base}/nRgpa1AkdnO1z0pa1fQuO`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`nRgpa1AkdnO1z0pa1fQuO`, res);
  });
  middlewares.use(`/${base}/SCXsZbePV9aZ5k8Ufglon`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`SCXsZbePV9aZ5k8Ufglon`, res);
  });
  middlewares.use(`/${base}/JzlZjf4zdKkbjpFjiwTgN`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`JzlZjf4zdKkbjpFjiwTgN`, res);
  });
  middlewares.use(`/${base}/tMvJnIWXoKwrKv0qe9TUp`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`tMvJnIWXoKwrKv0qe9TUp`, res);
  });
  middlewares.use(`/${base}/i-QKRrqdpMSTJdBYK8rEV`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`i-QKRrqdpMSTJdBYK8rEV`, res);
  });
  middlewares.use(`/${base}/qK3ZtQsSU1aGka8gOx30n`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`qK3ZtQsSU1aGka8gOx30n`, res);
  });
  middlewares.use(`/${base}/5xEf0R-ZytD7TK48Lctu-`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`5xEf0R-ZytD7TK48Lctu-`, res);
  });
  middlewares.use(`/${base}/NlPn8IhLUgOKn-u9-x8Xv`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`NlPn8IhLUgOKn-u9-x8Xv`, res);
  });
  middlewares.use(`/${base}/HjeaF_8Ci4m0ujUjlmVNS`, (_, res) => {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    readFile(`HjeaF_8Ci4m0ujUjlmVNS`, res);
  });
};

import { AxiosError } from 'axios';

export abstract class CustomError extends Error {}

export class NetworkError extends CustomError {
  constructor(message: string) {
    super(message);
    this.name = 'NetworkError';
  }
}

export class UnauthorizedError extends CustomError {
  constructor(message: string) {
    super(message);
    this.name = 'UnauthorizedError';
  }
}

export class ServerError extends CustomError {
  constructor(message: string) {
    super(message);
    this.name = 'ServerError';
  }
}

export class HttpError extends AxiosError {
  constructor(axiosError: AxiosError) {
    super(
      axiosError.message,
      axiosError.code,
      axiosError.config,
      axiosError.request,
      axiosError.response,
    );
    this.name = 'HttpError';
  }
}

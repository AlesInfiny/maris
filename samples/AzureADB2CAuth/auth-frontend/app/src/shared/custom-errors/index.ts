/* eslint max-classes-per-file: 0 */

/**
 * カスタムエラーの基底クラスです。
 */
export abstract class CustomErrorBase extends Error {
  cause?: Error | null

  constructor(message: string, cause?: Error) {
    super(message)
    // ラップ前のエラーを cause として保持
    this.cause = cause
  }
}

/**
 * 原因不明のエラーを表すカスタムエラーです。
 */
export class UnknownError extends CustomErrorBase {
  constructor(message: string, cause?: Error) {
    super(message, cause)
    this.name = 'UnknownError'
  }
}

/**
 * HTTP 通信でのエラーを表すカスタムエラーです。
 */
export class HttpError extends CustomErrorBase {
  constructor(message: string, cause?: Error) {
    super(message, cause)
    this.name = 'HttpError'
  }
}

/**
 * ネットワークエラーを表すカスタムエラーです。
 */
export class NetworkError extends HttpError {
  constructor(message: string, cause?: Error) {
    super(message, cause)
    this.name = 'NetworkError'
  }
}

/**
 * 401 Unauthorized を表すカスタムエラーです。
 */
export class UnauthorizedError extends HttpError {
  constructor(message: string, cause?: Error) {
    super(message, cause)
    this.name = 'UnauthorizedError'
  }
}

/**
 * 500 Internal Server Error を表すカスタムエラーです。
 */
export class ServerError extends HttpError {
  constructor(message: string, cause?: Error) {
    super(message, cause)
    this.name = 'ServerError'
  }
}

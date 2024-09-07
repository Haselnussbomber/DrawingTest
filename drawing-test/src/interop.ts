declare global {
  function setInterval(handler: Function, timeout?: number, ...arguments: any[]): number; // string as handler is not supported
  function setTimeout(handler: Function, timeout?: number, ...arguments: any[]): number; // string as handler is not supported
  function clearInterval(id: number | undefined): void;
  function clearTimeout(id: number | undefined): void;
}

globalThis.setInterval = function setInterval(handler: Function, timeout?: number, ...args: any[]) {
  return Common.setInterval(handler.bind(null, args), timeout);
}

globalThis.clearInterval = function clearInterval(id: number | undefined) {
  if (id) {
    Common.clearInterval(id);
  }
}

globalThis.setTimeout = function setTimeout(handler: Function, timeout?: number, ...args: any[]) {
  return Common.setTimeout(handler.bind(null, args), timeout);
}

globalThis.clearTimeout = function clearTimeout(id: number | undefined) {
  if (id) {
    Common.clearTimeout(id);
  }
}

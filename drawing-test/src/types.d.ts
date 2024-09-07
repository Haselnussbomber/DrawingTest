interface Node {
  Add(node: Node): void;
  Remove(node: Node): void;
}

interface Document extends Node {
  CreateElement(type: string): Document;
  CreateTextElement(): Text;
}

interface Text extends Node {
  
}

interface Common {
  setInterval(handler: Function, timeout?: number): number;
  setTimeout(handler: Function, timeout?: number): number;
  clearInterval(id: number | undefined): void;
  clearTimeout(id: number | undefined): void;
}

declare const document: Document;
declare const Common: Common;

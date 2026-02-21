import { Book } from '../../types/book';

const CART_KEY = 'bookshop-cart';

export interface CartItem {
  book: Book;
  quantity: number;
}

export const loadCart = (): CartItem[] => {
  try {
    const raw = localStorage.getItem(CART_KEY);
    return raw ? (JSON.parse(raw) as CartItem[]) : [];
  } catch {
    return [];
  }
};

export const saveCart = (items: CartItem[]): void => {
  localStorage.setItem(CART_KEY, JSON.stringify(items));
};

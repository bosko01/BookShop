import { createContext, ReactNode, useContext, useEffect, useMemo, useState } from 'react';
import { Book } from '../../types/book';
import { CartItem, loadCart, saveCart } from './cartStorage';

interface CartContextValue {
  items: CartItem[];
  itemCount: number;
  subtotal: number;
  addToCart: (book: Book, quantity?: number) => void;
  updateQuantity: (bookId: string, quantity: number) => void;
  removeFromCart: (bookId: string) => void;
  clearCart: () => void;
}

const CartContext = createContext<CartContextValue | undefined>(undefined);

export const CartProvider = ({ children }: { children: ReactNode }) => {
  const [items, setItems] = useState<CartItem[]>([]);

  useEffect(() => {
    setItems(loadCart());
  }, []);

  useEffect(() => {
    saveCart(items);
  }, [items]);

  const addToCart = (book: Book, quantity = 1) => {
    setItems((current) => {
      const existing = current.find((entry) => entry.book.id === book.id);
      if (existing) {
        return current.map((entry) =>
          entry.book.id === book.id ? { ...entry, quantity: entry.quantity + quantity } : entry,
        );
      }
      return [...current, { book, quantity }];
    });
  };

  const updateQuantity = (bookId: string, quantity: number) => {
    if (quantity < 1) return;
    setItems((current) =>
      current.map((entry) => (entry.book.id === bookId ? { ...entry, quantity } : entry)),
    );
  };

  const removeFromCart = (bookId: string) => {
    setItems((current) => current.filter((entry) => entry.book.id !== bookId));
  };

  const clearCart = () => setItems([]);

  const value = useMemo(
    () => ({
      items,
      itemCount: items.reduce((sum, item) => sum + item.quantity, 0),
      subtotal: items.reduce((sum, item) => sum + item.book.price * item.quantity, 0),
      addToCart,
      updateQuantity,
      removeFromCart,
      clearCart,
    }),
    [items],
  );

  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
};

export const useCart = (): CartContextValue => {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error('useCart must be used within CartProvider');
  }
  return context;
};

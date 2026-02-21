import { Book } from '../types/book';
import { Order, OrderStatus } from '../types/order';
import { getBooks } from './bookService';

let adminBooks: Book[] = getBooks();

let adminOrders: Order[] = [
  { id: 'ORD-1001', customer: 'Ava Wilson', date: '2026-01-11', total: 54.5, items: 3, status: 'Created' },
  { id: 'ORD-1002', customer: 'Noah Garcia', date: '2026-01-13', total: 120.0, items: 5, status: 'Paid' },
  { id: 'ORD-1003', customer: 'Liam Brown', date: '2026-01-15', total: 33.99, items: 2, status: 'Shipped' },
  { id: 'ORD-1004', customer: 'Emma Johnson', date: '2026-01-16', total: 89.45, items: 4, status: 'Cancelled' },
];

export const getAdminBooks = (): Book[] => [...adminBooks];

export const createAdminBook = (book: Omit<Book, 'id' | 'rating' | 'reviewsCount' | 'description' | 'shortDescription' | 'year' | 'image'>): Book => {
  const newBook: Book = {
    ...book,
    id: String(Date.now()),
    rating: 4.2,
    reviewsCount: 0,
    description: `${book.title} by ${book.author}.`,
    shortDescription: `${book.title} by ${book.author}.`,
    year: new Date().getFullYear(),
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=New+Book',
  };
  adminBooks = [newBook, ...adminBooks];
  return newBook;
};

export const updateAdminBook = (id: string, payload: Partial<Book>): Book | undefined => {
  let updated: Book | undefined;
  adminBooks = adminBooks.map((book) => {
    if (book.id === id) {
      updated = { ...book, ...payload };
      return updated;
    }
    return book;
  });
  return updated;
};

export const deleteAdminBook = (id: string): void => {
  adminBooks = adminBooks.filter((book) => book.id !== id);
};

export const getAdminOrders = (): Order[] => [...adminOrders];

export const updateOrderStatus = (id: string, status: OrderStatus): Order | undefined => {
  let updated: Order | undefined;
  adminOrders = adminOrders.map((order) => {
    if (order.id === id) {
      updated = { ...order, status };
      return updated;
    }
    return order;
  });
  return updated;
};

import { Book } from '../types/book';
import { Order, OrderStatus } from '../types/order';
import { request } from './apiClient';
import { getBookById } from './bookService';

interface ApiOrder {
  id: number;
  userId: number;
  totalAmount: number;
  status: OrderStatus;
  createdAtUtc: string;
  itemCount: number;
}

interface ApiBookListResponse {
  id: number;
}

const toOrder = (order: ApiOrder): Order => ({
  id: `ORD-${order.id}`,
  customer: `User #${order.userId}`,
  date: new Date(order.createdAtUtc).toISOString().slice(0, 10),
  total: order.totalAmount,
  items: order.itemCount,
  status: order.status,
});

export const getAdminBooks = async (): Promise<Book[]> => {
  const books = await request<ApiBookListResponse[]>('/api/book');
  const details = await Promise.all(books.map((book) => getBookById(String(book.id))));
  return details.filter((book): book is Book => Boolean(book));
};

export const createAdminBook = async (
  token: string,
  book: Omit<Book, 'id' | 'rating' | 'reviewsCount' | 'description' | 'shortDescription' | 'year' | 'image'>,
): Promise<void> => {
  await request<number>('/api/book', {
    method: 'POST',
    token,
    body: {
      title: book.title,
      description: `${book.title} by ${book.author}`,
      imageUrl: null,
      pageCount: 100,
      price: book.price,
      quantityInStock: book.stock,
      authorId: 1,
      publisherId: 1,
      genreId: 1,
      bindingId: 1,
    },
  });
};

export const updateAdminBook = async (token: string, id: string, payload: Partial<Book>): Promise<void> => {
  const existing = await getBookById(id);
  if (!existing) return;

  await request(`/api/book/${id}`, {
    method: 'PUT',
    token,
    body: {
      title: payload.title ?? existing.title,
      price: payload.price ?? existing.price,
      quantityInStock: payload.stock ?? existing.stock,
      pageCount: existing.year,
      publisherId: 1,
      authorId: 1,
      genreId: 1,
      bindingId: 1,
      description: payload.description ?? existing.description,
      imageUrl: payload.image ?? existing.image,
    },
  });
};

export const deleteAdminBook = async (token: string, id: string): Promise<void> => {
  await request(`/api/book/${id}`, {
    method: 'DELETE',
    token,
  });
};

export const getAdminOrders = async (token: string): Promise<Order[]> => {
  const orders = await request<ApiOrder[]>('/api/orders', { token });
  return orders.map(toOrder);
};

export const updateOrderStatus = async (token: string, id: string, status: OrderStatus): Promise<void> => {
  const numericId = Number(id.replace('ORD-', ''));
  const endpointByStatus: Partial<Record<OrderStatus, string>> = {
    Paid: 'paid',
    Shipped: 'shipped',
    Delivered: 'delivered',
    Cancelled: 'cancel',
  };

  const endpoint = endpointByStatus[status];
  if (!endpoint) {
    return;
  }

  await request(`/api/orders/${numericId}/${endpoint}`, {
    method: 'PATCH',
    token,
  });
};

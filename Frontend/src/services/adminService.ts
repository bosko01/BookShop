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


interface ApiAdminAnalytics {
  totalSales: number;
  orders: number;
  books: number;
  customers: number;
}

export interface AdminGenre {
  id: number;
  name: string;
}

export interface AdminBinding {
  id: number;
  name: string;
}

export interface AdminPublisher {
  id: number;
  name: string;
  country?: string | null;
  address?: string | null;
  city?: string | null;
  phoneNumber?: string | null;
}

export interface AdminUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: 'Customer' | 'Admin';
  createdAtUtc: string;
}

export interface AdminAnalytics {
  totalSales: number;
  orders: number;
  books: number;
  customers: number;
}

const toOrder = (order: ApiOrder): Order => ({
  orderId: order.id,
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

export const getAdminAnalytics = async (token: string): Promise<AdminAnalytics> => {
  return request<ApiAdminAnalytics>('/api/admin/analytics', { token });
};

export const getAdminGenres = async (): Promise<AdminGenre[]> => {
  return request<AdminGenre[]>('/api/genres');
};

export const createAdminGenre = async (token: string, name: string): Promise<void> => {
  await request('/api/genres', { method: 'POST', token, body: { name } });
};

export const updateAdminGenre = async (token: string, id: number, name: string): Promise<void> => {
  await request(`/api/genres/${id}`, { method: 'PUT', token, body: { name } });
};

export const deleteAdminGenre = async (token: string, id: number): Promise<void> => {
  await request(`/api/genres/${id}`, { method: 'DELETE', token });
};

export const getAdminBindings = async (): Promise<AdminBinding[]> => {
  return request<AdminBinding[]>('/api/bindings');
};

export const createAdminBinding = async (token: string, name: string): Promise<void> => {
  await request('/api/bindings', { method: 'POST', token, body: { name } });
};

export const updateAdminBinding = async (token: string, id: number, name: string): Promise<void> => {
  await request(`/api/bindings/${id}`, { method: 'PUT', token, body: { name } });
};

export const deleteAdminBinding = async (token: string, id: number): Promise<void> => {
  await request(`/api/bindings/${id}`, { method: 'DELETE', token });
};

export const getAdminPublishers = async (): Promise<AdminPublisher[]> => {
  return request<AdminPublisher[]>('/api/publishers');
};

export const createAdminPublisher = async (token: string, publisher: Pick<AdminPublisher, 'name' | 'country'>): Promise<void> => {
  await request('/api/publishers', {
    method: 'POST',
    token,
    body: {
      name: publisher.name,
      country: publisher.country ?? null,
      address: null,
      city: null,
      phoneNumber: null,
    },
  });
};

export const updateAdminPublisher = async (token: string, id: number, publisher: Pick<AdminPublisher, 'name' | 'country'>): Promise<void> => {
  await request(`/api/publishers/${id}`, {
    method: 'PUT',
    token,
    body: {
      name: publisher.name,
      country: publisher.country ?? null,
      address: null,
      city: null,
      phoneNumber: null,
    },
  });
};

export const deleteAdminPublisher = async (token: string, id: number): Promise<void> => {
  await request(`/api/publishers/${id}`, { method: 'DELETE', token });
};

export const getAdminUsers = async (token: string): Promise<AdminUser[]> => {
  return request<AdminUser[]>('/api/users', { token });
};

export const createAdminUser = async (
  token: string,
  payload: Pick<AdminUser, 'firstName' | 'lastName' | 'email' | 'role'> & { password: string },
): Promise<void> => {
  await request('/api/users', {
    method: 'POST',
    token,
    body: payload,
  });
};

export const updateAdminUser = async (
  token: string,
  id: number,
  payload: Pick<AdminUser, 'firstName' | 'lastName' | 'email'>,
): Promise<void> => {
  await request(`/api/users/${id}`, {
    method: 'PUT',
    token,
    body: payload,
  });
};

export const deleteAdminUser = async (token: string, id: number): Promise<void> => {
  await request(`/api/users/${id}`, { method: 'DELETE', token });
};

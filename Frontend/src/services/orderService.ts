import { request } from './apiClient';
import { Order, OrderStatus } from '../types/order';

interface ApiOrder {
  id: number;
  userId: number;
  totalAmount: number;
  status: OrderStatus;
  createdAtUtc: string;
  itemCount: number;
}

const toOrder = (order: ApiOrder): Order => ({
  id: `ORD-${order.id}`,
  customer: `User #${order.userId}`,
  date: new Date(order.createdAtUtc).toISOString().slice(0, 10),
  total: order.totalAmount,
  items: order.itemCount,
  status: order.status,
});

export const getMyOrders = async (token: string): Promise<Order[]> => {
  const orders = await request<ApiOrder[]>('/api/orders', { token });
  return orders.map(toOrder);
};

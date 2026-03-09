import { request } from './apiClient';
import { Order, OrderStatus } from '../types/order';

interface ApiOrder {
  id: number;
  userId: number;
  totalAmount: number;
  status: OrderStatus | number;
  createdAtUtc: string;
  itemCount: number;
  items?: Array<{
    orderItemId: number;
  }>;
}

const orderStatusMap: Record<number, OrderStatus> = {
  0: 'Created',
  1: 'Paid',
  2: 'Shipped',
  3: 'Delivered',
  4: 'Cancelled',
};

const toOrderStatus = (status: ApiOrder['status']): OrderStatus => {
  if (typeof status === 'number') {
    return orderStatusMap[status] ?? 'Created';
  }

  return status;
};

const toOrder = (order: ApiOrder): Order => ({
  orderId: order.id,
  id: `ORD-${order.id}`,
  customer: `User #${order.userId}`,
  date: new Date(order.createdAtUtc).toISOString().slice(0, 10),
  total: order.totalAmount,
  items: order.items?.length ?? order.itemCount,
  invoiceNumber: undefined,
  status: toOrderStatus(order.status),
});

export const getMyOrders = async (token: string): Promise<Order[]> => {
  const orders = await request<ApiOrder[]>('/api/orders', { token });
  return orders.map(toOrder);
};

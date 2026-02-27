import { OrderStatus } from '../types/order';
import { request } from './apiClient';

interface StripeCheckoutSessionResponse {
  sessionId: string;
  url: string;
}

interface ApiOrderItem {
  orderItemId: number;
  bookId: number;
  bookTitle: string;
  quantity: number;
  unitPrice: number;
}

interface ApiOrderResponse {
  id: number;
  userId: number;
  totalAmount: number;
  status: OrderStatus;
  createdAtUtc: string;
  itemCount: number;
  items: ApiOrderItem[];
}

export interface CheckoutOrderItem {
  bookId: number;
  quantity: number;
}

export interface OrderDetails {
  id: number;
  status: OrderStatus;
  totalAmount: number;
  items: ApiOrderItem[];
}

export const createOrder = (token: string, userId: number, items: CheckoutOrderItem[]) =>
  request<number>('/api/orders', {
    method: 'POST',
    token,
    body: { userId, items },
  });

export const createCheckoutSession = (
  token: string,
  payload: {
    orderId: number;
    userId: number;
    amount: number;
    currency: string;
    successUrl: string;
    cancelUrl: string;
  },
) =>
  request<StripeCheckoutSessionResponse>('/api/payment/checkout-session', {
    method: 'POST',
    token,
    body: payload,
  });

export const getOrderDetails = async (token: string, orderId: number): Promise<OrderDetails> => {
  const response = await request<ApiOrderResponse>(`/api/orders/${orderId}`, {
    method: 'GET',
    token,
  });

  return {
    id: response.id,
    status: response.status,
    totalAmount: response.totalAmount,
    items: response.items,
  };
};

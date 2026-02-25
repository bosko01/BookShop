import { OrderStatus } from '../types/order';
import { request } from './apiClient';

interface StripeCheckoutSessionResponse {
  sessionId: string;
  url: string;
}

interface ApiOrderStatusResponse {
  status: OrderStatus;
}

export const createOrder = (token: string, userId: number) =>
  request<number>('/api/orders', {
    method: 'POST',
    token,
    body: { userId },
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

export const getOrderStatus = async (token: string, orderId: number): Promise<OrderStatus | null> => {
  try {
    const response = await request<ApiOrderStatusResponse>(`/api/orders/${orderId}`, {
      method: 'GET',
      token,
    });

    return response.status;
  } catch {
    return null;
  }
};

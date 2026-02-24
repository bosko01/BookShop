import { request } from './apiClient';

interface StripeCheckoutSessionResponse {
  sessionId: string;
  url: string;
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

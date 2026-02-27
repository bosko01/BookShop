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

interface ApiInvoiceResponse {
  id: string;
  orderId: number;
  paymentMethodId: number;
  amount: number;
  isPaid: boolean;
  issuedAtUtc: string;
  paidAtUtc: string | null;
  provider: string | null;
  providerReference: string | null;
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

export interface InvoiceDetails {
  id: string;
  isPaid: boolean;
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

export const getInvoiceByOrderId = async (token: string, orderId: number): Promise<InvoiceDetails> => {
  const response = await request<ApiInvoiceResponse>(`/api/invoices/by-order/${orderId}`, {
    method: 'GET',
    token,
  });

  return {
    id: response.id,
    isPaid: response.isPaid,
  };
};

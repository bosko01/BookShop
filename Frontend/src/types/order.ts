export type OrderStatus = 'Created' | 'Paid' | 'Shipped' | 'Cancelled' | 'Delivered';

export interface Order {
  orderId: number;
  id: string;
  customer: string;
  date: string;
  total: number;
  items: number;
  invoiceNumber?: string;
  status: OrderStatus;
}

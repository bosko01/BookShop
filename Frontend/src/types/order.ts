export type OrderStatus = 'Created' | 'Paid' | 'Shipped' | 'Cancelled' | 'Delivered';

export interface Order {
  id: string;
  customer: string;
  date: string;
  total: number;
  items: number;
  status: OrderStatus;
}

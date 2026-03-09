import { useEffect, useState } from 'react';
import { DataTable } from '../components/admin/DataTable';
import { getInvoiceByOrderId } from '../services/checkoutService';
import { getMyOrders } from '../services/orderService';
import { useAuth } from '../state/auth/AuthContext';
import { Order } from '../types/order';

const MyOrdersPage = () => {
  const { accessToken } = useAuth();
  const [orders, setOrders] = useState<Order[]>([]);

  useEffect(() => {
    if (!accessToken) {
      setOrders([]);
      return;
    }

    const loadOrders = async () => {
      try {
        const userOrders = await getMyOrders(accessToken);
        const ordersWithInvoices = await Promise.all(
          userOrders.map(async (order) => {
            try {
              const invoice = await getInvoiceByOrderId(accessToken, order.orderId);
              return { ...order, invoiceNumber: invoice.id };
            } catch {
              return { ...order, invoiceNumber: 'U pripremi...' };
            }
          }),
        );

        setOrders(ordersWithInvoices);
      } catch {
        setOrders([]);
      }
    };

    loadOrders();
  }, [accessToken]);

  return (
    <section className="space-y-4">
      <h1 className="text-3xl font-bold text-slate-900">My Orders</h1>
      <DataTable
        headers={['Order ID', 'Invoice number', 'Total', 'Date', 'Status']}
        rows={orders.map((order) => (
          <tr key={order.id} className="border-t border-slate-100">
            <td className="px-4 py-3 font-medium">{order.id}</td>
            <td className="px-4 py-3">{order.invoiceNumber ?? 'U pripremi...'}</td>
            <td className="px-4 py-3">${order.total.toFixed(2)}</td>
            <td className="px-4 py-3">{order.date}</td>
            <td className="px-4 py-3">{order.status}</td>
          </tr>
        ))}
      />
    </section>
  );
};

export default MyOrdersPage;

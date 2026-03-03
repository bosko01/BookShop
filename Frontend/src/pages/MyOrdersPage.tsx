import { useEffect, useState } from 'react';
import { DataTable } from '../components/admin/DataTable';
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

    getMyOrders(accessToken).then(setOrders).catch(() => setOrders([]));
  }, [accessToken]);

  return (
    <section className="space-y-4">
      <h1 className="text-3xl font-bold text-slate-900">My Orders</h1>
      <DataTable
        headers={['Order ID', 'Items', 'Total', 'Date', 'Status']}
        rows={orders.map((order) => (
          <tr key={order.id} className="border-t border-slate-100">
            <td className="px-4 py-3 font-medium">{order.id}</td>
            <td className="px-4 py-3">{order.items}</td>
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

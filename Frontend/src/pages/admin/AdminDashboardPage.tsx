import { useEffect, useState } from 'react';
import { DataTable } from '../../components/admin/DataTable';
import { StatCards } from '../../components/admin/StatCards';
import { getAdminOrders } from '../../services/adminService';
import { Order } from '../../types/order';

const AdminDashboardPage = () => {
  const [recent, setRecent] = useState<Order[]>([]);

  useEffect(() => {
    getAdminOrders().then((orders) => setRecent(orders.slice(0, 4))).catch(() => setRecent([]));
  }, []);

  return (
    <div className="space-y-6">
      <StatCards />
      <section className="space-y-3">
        <h2 className="text-xl font-bold text-slate-900">Recent Orders</h2>
        <DataTable
          headers={['Order ID', 'Customer', 'Date', 'Total', 'Status']}
          rows={recent.map((order) => (
            <tr key={order.id} className="border-t border-slate-100">
              <td className="px-4 py-3 font-medium">{order.id}</td>
              <td className="px-4 py-3">{order.customer}</td>
              <td className="px-4 py-3">{order.date}</td>
              <td className="px-4 py-3">${order.total.toFixed(2)}</td>
              <td className="px-4 py-3">{order.status}</td>
            </tr>
          ))}
        />
      </section>
    </div>
  );
};

export default AdminDashboardPage;

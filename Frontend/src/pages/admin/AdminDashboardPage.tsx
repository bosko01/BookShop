import { useEffect, useState } from 'react';
import { DataTable } from '../../components/admin/DataTable';
import { StatCards } from '../../components/admin/StatCards';
import { AdminAnalytics, getAdminAnalytics, getAdminOrders } from '../../services/adminService';
import { useAuth } from '../../state/auth/AuthContext';
import { Order } from '../../types/order';

const emptyStats: AdminAnalytics = {
  totalSales: 0,
  orders: 0,
  books: 0,
  customers: 0,
};

const AdminDashboardPage = () => {
  const [recent, setRecent] = useState<Order[]>([]);
  const [stats, setStats] = useState<AdminAnalytics>(emptyStats);
  const { accessToken } = useAuth();

  useEffect(() => {
    if (!accessToken) {
      setRecent([]);
      setStats(emptyStats);
      return;
    }

    const loadDashboard = async () => {
      try {
        const [orders, analytics] = await Promise.all([
          getAdminOrders(accessToken),
          getAdminAnalytics(accessToken),
        ]);

        setRecent(orders.slice(0, 4));
        setStats(analytics);
      } catch {
        setRecent([]);
      }
    };

    void loadDashboard();
    const intervalId = window.setInterval(() => {
      void loadDashboard();
    }, 5000);

    return () => window.clearInterval(intervalId);
  }, [accessToken]);

  return (
    <div className="space-y-6">
      <StatCards stats={stats} />
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

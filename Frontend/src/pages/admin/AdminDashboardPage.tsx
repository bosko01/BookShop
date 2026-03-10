import { useEffect, useState } from 'react';
import { DataTable } from '../../components/admin/DataTable';
import { StatCards } from '../../components/admin/StatCards';
import { AdminAnalytics, getAdminAnalytics, getAdminOrders } from '../../services/adminService';
import { useAuth } from '../../state/auth/AuthContext';
import { Order } from '../../types/order';
import { formatCurrency } from '../../utils/currency';

const emptyStats: AdminAnalytics = {
  totalSales: 0,
  orders: 0,
  books: 0,
  customers: 0,
};

const AdminDashboardPage = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [stats, setStats] = useState<AdminAnalytics>(emptyStats);
  const [orderSearch, setOrderSearch] = useState('');
  const { accessToken } = useAuth();

  useEffect(() => {
    if (!accessToken) {
      setOrders([]);
      setStats(emptyStats);
      setOrderSearch('');
      return;
    }

    const loadDashboard = async () => {
      try {
        const [orders, analytics] = await Promise.all([
          getAdminOrders(accessToken),
          getAdminAnalytics(accessToken),
        ]);

        setOrders(orders);
        setStats(analytics);
      } catch {
        setOrders([]);
      }
    };

    void loadDashboard();
    const intervalId = window.setInterval(() => {
      void loadDashboard();
    }, 5000);

    return () => window.clearInterval(intervalId);
  }, [accessToken]);

  const normalizedSearch = orderSearch.trim().toLowerCase();
  const displayedOrders = normalizedSearch
    ? orders.filter((order) => order.id.toLowerCase().includes(normalizedSearch))
    : orders.slice(0, 4);

  return (
    <div className="space-y-6">
      <StatCards stats={stats} />
      <section className="space-y-3">
        <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <h2 className="text-xl font-bold text-slate-900">Recent Orders</h2>
          <input
            value={orderSearch}
            onChange={(e) => setOrderSearch(e.target.value)}
            placeholder="Search Order ID (e.g. ORD-12)"
            className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm text-slate-700 outline-none ring-brand-200 focus:ring sm:w-72"
          />
        </div>
        <DataTable
          headers={['Order ID', 'Customer', 'Date', 'Total', 'Status']}
          rows={displayedOrders.length > 0
            ? displayedOrders.map((order) => (
                <tr key={order.id} className="border-t border-slate-100">
                  <td className="px-4 py-3 font-medium">{order.id}</td>
                  <td className="px-4 py-3">{order.customer}</td>
                  <td className="px-4 py-3">{order.date}</td>
                  <td className="px-4 py-3">{formatCurrency(order.total)}</td>
                  <td className="px-4 py-3">{order.status}</td>
                </tr>
              ))
            : (
              <tr className="border-t border-slate-100">
                <td className="px-4 py-3 text-slate-500" colSpan={5}>No orders found.</td>
              </tr>
              )}
        />
      </section>
    </div>
  );
};

export default AdminDashboardPage;

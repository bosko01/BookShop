import { useState } from 'react';
import { DataTable } from '../../components/admin/DataTable';
import { getAdminOrders, updateOrderStatus } from '../../services/adminService';
import { OrderStatus } from '../../types/order';

const statuses: OrderStatus[] = ['Created', 'Paid', 'Shipped', 'Cancelled'];

const AdminOrdersPage = () => {
  const [orders, setOrders] = useState(getAdminOrders());

  return (
    <div className="space-y-4">
      <h2 className="text-2xl font-bold text-slate-900">Orders Management</h2>
      <DataTable
        headers={['Order ID', 'Customer', 'Items', 'Total', 'Date', 'Status']}
        rows={orders.map((order) => (
          <tr key={order.id} className="border-t border-slate-100">
            <td className="px-4 py-3 font-medium">{order.id}</td>
            <td className="px-4 py-3">{order.customer}</td>
            <td className="px-4 py-3">{order.items}</td>
            <td className="px-4 py-3">${order.total.toFixed(2)}</td>
            <td className="px-4 py-3">{order.date}</td>
            <td className="px-4 py-3">
              <select
                value={order.status}
                onChange={(e) => {
                  updateOrderStatus(order.id, e.target.value as OrderStatus);
                  setOrders(getAdminOrders());
                }}
                className="rounded-xl border border-slate-200 px-2 py-1 text-xs"
              >
                {statuses.map((status) => (
                  <option key={status} value={status}>{status}</option>
                ))}
              </select>
            </td>
          </tr>
        ))}
      />
    </div>
  );
};

export default AdminOrdersPage;

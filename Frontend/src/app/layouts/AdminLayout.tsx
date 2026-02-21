import { LayoutDashboard, Package, ShoppingBag } from 'lucide-react';
import { Link, NavLink, Outlet } from 'react-router-dom';

const links = [
  { to: '/admin', label: 'Dashboard', icon: LayoutDashboard },
  { to: '/admin/books', label: 'Books', icon: Package },
  { to: '/admin/orders', label: 'Orders', icon: ShoppingBag },
];

export const AdminLayout = () => (
  <div className="min-h-screen bg-slate-50 lg:grid lg:grid-cols-[240px_1fr]">
    <aside className="border-r border-slate-200 bg-white p-4">
      <Link to="/" className="mb-6 block text-2xl font-bold text-slate-900">BookShop</Link>
      <nav className="space-y-2">
        {links.map((link) => {
          const Icon = link.icon;
          return (
            <NavLink key={link.to} to={link.to} end={link.to === '/admin'} className={({ isActive }) => `flex items-center gap-2 rounded-xl px-3 py-2 text-sm font-medium ${isActive ? 'bg-brand-500 text-white' : 'text-slate-600 hover:bg-slate-100'}`}>
              <Icon size={16} /> {link.label}
            </NavLink>
          );
        })}
      </nav>
    </aside>
    <section>
      <header className="border-b border-slate-200 bg-white p-4"><h1 className="font-semibold text-slate-800">Admin Panel</h1></header>
      <main className="p-4 sm:p-6"><Outlet /></main>
    </section>
  </div>
);

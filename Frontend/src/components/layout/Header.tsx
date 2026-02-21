import { ShoppingCart } from 'lucide-react';
import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { useCart } from '../../state/cart/CartContext';

export const Header = () => {
  const [search, setSearch] = useState('');
  const navigate = useNavigate();
  const { itemCount } = useCart();

  const onSearch = (e: React.FormEvent) => {
    e.preventDefault();
    navigate(`/shop?search=${encodeURIComponent(search)}`);
  };

  return (
    <header className="border-b border-slate-200 bg-white/90 backdrop-blur">
      <div className="container-base flex flex-wrap items-center gap-3 py-4">
        <Link to="/" className="text-2xl font-bold text-slate-900">BookShop</Link>
        <nav className="ml-auto flex items-center gap-3 text-sm font-medium sm:ml-0 sm:gap-5">
          <NavLink to="/" className="text-slate-600 hover:text-brand-500">Home</NavLink>
          <NavLink to="/shop" className="text-slate-600 hover:text-brand-500">Shop</NavLink>
          <NavLink to="/admin" className="text-slate-600 hover:text-brand-500">Admin</NavLink>
        </nav>
        <form onSubmit={onSearch} className="order-3 w-full sm:order-none sm:ml-auto sm:w-64">
          <input
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Search books"
            className="w-full rounded-xl border border-slate-200 px-4 py-2 text-sm focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-orange-200"
          />
        </form>
        <Link to="/cart" className="relative rounded-xl bg-slate-100 p-2 text-slate-700 hover:bg-slate-200">
          <ShoppingCart size={20} />
          <span className="absolute -right-2 -top-2 inline-flex h-5 min-w-5 items-center justify-center rounded-full bg-brand-500 px-1 text-xs text-white">{itemCount}</span>
        </Link>
      </div>
    </header>
  );
};

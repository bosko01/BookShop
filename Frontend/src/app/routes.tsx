import { createBrowserRouter } from 'react-router-dom';
import { ReactNode } from 'react';
import { AdminLayout } from './layouts/AdminLayout';
import { PublicLayout } from './layouts/PublicLayout';
import HomePage from '../pages/HomePage';
import ShopPage from '../pages/ShopPage';
import BookDetailsPage from '../pages/BookDetailsPage';
import CartPage from '../pages/CartPage';
import LoginPage from '../pages/LoginPage';
import AdminDashboardPage from '../pages/admin/AdminDashboardPage';
import AdminBooksPage from '../pages/admin/AdminBooksPage';
import AdminOrdersPage from '../pages/admin/AdminOrdersPage';
import { useAuth } from '../state/auth/AuthContext';
import { Navigate } from 'react-router-dom';

const RequireAdmin = ({ children }: { children: ReactNode }) => {
  const { isAdmin } = useAuth();
  if (!isAdmin) {
    return <Navigate to="/login" replace />;
  }
  return <>{children}</>;
};

export const router = createBrowserRouter([
  {
    path: '/',
    element: <PublicLayout />,
    children: [
      { index: true, element: <HomePage /> },
      { path: 'shop', element: <ShopPage /> },
      { path: 'books/:id', element: <BookDetailsPage /> },
      { path: 'cart', element: <CartPage /> },
      { path: 'login', element: <LoginPage /> },
    ],
  },
  {
    path: '/admin',
    element: <RequireAdmin><AdminLayout /></RequireAdmin>,
    children: [
      { index: true, element: <AdminDashboardPage /> },
      { path: 'books', element: <AdminBooksPage /> },
      { path: 'orders', element: <AdminOrdersPage /> },
    ],
  },
]);

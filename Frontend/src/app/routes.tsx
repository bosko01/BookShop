import { createBrowserRouter } from 'react-router-dom';
import { ReactNode } from 'react';
import { AdminLayout } from './layouts/AdminLayout';
import { PublicLayout } from './layouts/PublicLayout';
import HomePage from '../pages/HomePage';
import ShopPage from '../pages/ShopPage';
import BookDetailsPage from '../pages/BookDetailsPage';
import CartPage from '../pages/CartPage';
import LoginPage from '../pages/LoginPage';
import RegisterPage from '../pages/RegisterPage';
import CheckoutSuccessPage from '../pages/CheckoutSuccessPage';
import MyOrdersPage from '../pages/MyOrdersPage';
import AdminDashboardPage from '../pages/admin/AdminDashboardPage';
import AdminBooksPage from '../pages/admin/AdminBooksPage';
import AdminOrdersPage from '../pages/admin/AdminOrdersPage';
import AdminEntitiesPage from '../pages/admin/AdminEntitiesPage';
import { useAuth } from '../state/auth/AuthContext';
import { Navigate, useLocation } from 'react-router-dom';

const RequireAuth = ({ children }: { children: ReactNode }) => {
  const { accessToken } = useAuth();
  const location = useLocation();
  if (!accessToken) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }
  return <>{children}</>;
};

const RequireAdmin = ({ children }: { children: ReactNode }) => {
  const { isAdmin } = useAuth();
  const location = useLocation();
  if (!isAdmin) {
    return <Navigate to="/login" replace state={{ from: location }} />;
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
      { path: 'register', element: <RegisterPage /> },
      { path: 'checkout/success', element: <CheckoutSuccessPage /> },
      { path: 'dashboard', element: <RequireAuth><MyOrdersPage /></RequireAuth> },
    ],
  },
  {
    path: '/admin',
    element: <RequireAdmin><AdminLayout /></RequireAdmin>,
    children: [
      { index: true, element: <AdminDashboardPage /> },
      { path: 'books', element: <AdminBooksPage /> },
      { path: 'orders', element: <AdminOrdersPage /> },
      { path: 'entities', element: <AdminEntitiesPage /> },
    ],
  },
]);

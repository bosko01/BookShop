import { useEffect, useMemo, useState } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { getInvoiceByOrderId, getOrderDetails, InvoiceDetails, OrderDetails } from '../services/checkoutService';
import { useAuth } from '../state/auth/AuthContext';
import { useCart } from '../state/cart/CartContext';

const CheckoutSuccessPage = () => {
  const { accessToken } = useAuth();
  const { clearCart } = useCart();
  const [searchParams] = useSearchParams();
  const [order, setOrder] = useState<OrderDetails | null>(null);
  const [invoice, setInvoice] = useState<InvoiceDetails | null>(null);
  const [error, setError] = useState('');

  const orderId = useMemo(() => Number(searchParams.get('orderId')), [searchParams]);

  useEffect(() => {
    if (!accessToken || !orderId) {
      setError('Nije moguće učitati porudžbinu.');
      return;
    }

    getOrderDetails(accessToken, orderId)
      .then((response) => {
        setOrder(response);
        clearCart();
      })
      .catch(() => {
        setError('Neuspešno učitavanje podataka o porudžbini.');
      });
  }, [accessToken, clearCart, orderId]);

  useEffect(() => {
    if (!accessToken || !orderId) {
      return;
    }

    let isMounted = true;
    let attempts = 0;

    const fetchInvoice = () => {
      attempts += 1;

      getInvoiceByOrderId(accessToken, orderId)
        .then((response) => {
          if (isMounted) {
            setInvoice(response);
          }
        })
        .catch(() => {
          if (isMounted && attempts < 5) {
            window.setTimeout(fetchInvoice, 2000);
          }
        });
    };

    fetchInvoice();

    return () => {
      isMounted = false;
    };
  }, [accessToken, orderId]);

  if (error) {
    return (
      <section className="rounded-2xl bg-white p-6 shadow-md">
        <h1 className="text-2xl font-bold text-slate-900">Plaćanje je završeno</h1>
        <p className="mt-3 text-sm text-red-600">{error}</p>
      </section>
    );
  }

  if (!order) {
    return <p className="rounded-2xl bg-white p-6 shadow-md">Učitavanje porudžbine...</p>;
  }

  return (
    <section className="space-y-4 rounded-2xl bg-white p-6 shadow-md">
      <h1 className="text-2xl font-bold text-slate-900">Plaćanje uspešno</h1>
      <p className="text-slate-700">
        Broj Invoice-a:{' '}
        <span className="font-semibold">{invoice ? invoice.id : 'U pripremi...'}</span>
      </p>
      <p className="text-slate-700">Status: <span className="font-semibold text-emerald-600">{order.status}</span></p>

      <div>
        <h2 className="mb-2 text-lg font-semibold text-slate-900">Stavke porudžbine</h2>
        <ul className="space-y-2">
          {order.items.map((item) => (
            <li key={item.orderItemId} className="rounded-xl border border-slate-200 p-3 text-sm text-slate-700">
              <div className="font-medium">{item.bookTitle}</div>
              <div>Količina: {item.quantity}</div>
              <div>Cena: ${item.unitPrice.toFixed(2)}</div>
            </li>
          ))}
        </ul>
      </div>

      <p className="text-slate-700">Ukupno: <span className="font-semibold">${order.totalAmount.toFixed(2)}</span></p>
      <Link to="/shop" className="inline-flex rounded-xl bg-brand-500 px-4 py-2 font-semibold text-white hover:bg-brand-600">Nastavi kupovinu</Link>
    </section>
  );
};

export default CheckoutSuccessPage;

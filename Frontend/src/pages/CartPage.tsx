import { useEffect, useState } from 'react';
import { CartSummary } from '../components/cart/CartSummary';
import { CartTable } from '../components/cart/CartTable';
import { getUserIdFromJwt } from '../services/authService';
import { createCheckoutSession, createOrder, getOrderStatus } from '../services/checkoutService';
import { useAuth } from '../state/auth/AuthContext';
import { useCart } from '../state/cart/CartContext';

const PAYMENT_STATUS_POLL_RETRIES = 6;
const PAYMENT_STATUS_POLL_INTERVAL_MS = 1000;

const sleep = (ms: number) => new Promise((resolve) => {
  setTimeout(resolve, ms);
});

const CartPage = () => {
  const { items, subtotal, updateQuantity, removeFromCart, clearCart } = useCart();
  const { accessToken } = useAuth();
  const [checkoutError, setCheckoutError] = useState('');
  const [isCheckoutLoading, setIsCheckoutLoading] = useState(false);

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    const paymentState = params.get('payment');

    if (paymentState !== 'success') {
      return;
    }

    clearCart();
    params.delete('payment');

    const nextQuery = params.toString();
    const nextUrl = nextQuery ? `${window.location.pathname}?${nextQuery}` : window.location.pathname;
    window.history.replaceState({}, '', nextUrl);
  }, [clearCart]);

  const onCheckout = async () => {
    setCheckoutError('');

    if (!accessToken) {
      setCheckoutError('Morate biti prijavljeni kako biste nastavili na checkout.');
      return;
    }

    const userId = getUserIdFromJwt(accessToken);
    if (!userId) {
      setCheckoutError('Nije moguće prepoznati korisnika iz tokena.');
      return;
    }

    const shipping = subtotal > 0 ? 4.99 : 0;
    const total = subtotal + shipping;

    if (total <= 0) {
      setCheckoutError('Korpa je prazna.');
      return;
    }

    setIsCheckoutLoading(true);
    try {
      const orderId = await createOrder(accessToken, userId);
      const session = await createCheckoutSession(accessToken, {
        orderId,
        userId,
        amount: total,
        currency: 'usd',
        successUrl: `${window.location.origin}/cart?payment=success&orderId=${orderId}`,
        cancelUrl: `${window.location.origin}/cart?payment=cancelled`,
      });

      if (!session.url) {
        throw new Error('Missing Stripe checkout URL.');
      }

      window.location.href = session.url;
    } catch {
      setCheckoutError('Checkout nije uspeo. Pokušajte ponovo.');
      setIsCheckoutLoading(false);
    }
  };

  return (
    <div className="grid gap-6 lg:grid-cols-[1fr_320px]">
      <section>
        <h1 className="mb-4 text-3xl font-bold text-slate-900">Your Cart</h1>
        {items.length === 0 ? <p className="rounded-2xl bg-white p-6 shadow-md">Your cart is empty.</p> : <CartTable items={items} onQuantityChange={updateQuantity} onRemove={removeFromCart} />}
      </section>
      <CartSummary subtotal={subtotal} onCheckout={onCheckout} isCheckoutLoading={isCheckoutLoading} checkoutError={checkoutError} />
    </div>
  );
};

export default CartPage;

import { useState } from 'react';
import { CartSummary } from '../components/cart/CartSummary';
import { CartTable } from '../components/cart/CartTable';
import { getUserIdFromJwt } from '../services/authService';
import { createCheckoutSession, createOrder } from '../services/checkoutService';
import { useAuth } from '../state/auth/AuthContext';
import { useCart } from '../state/cart/CartContext';

const CartPage = () => {
  const { items, subtotal, updateQuantity, removeFromCart } = useCart();
  const { accessToken } = useAuth();
  const [checkoutError, setCheckoutError] = useState('');
  const [isCheckoutLoading, setIsCheckoutLoading] = useState(false);


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
      const orderItems = items.map((item) => ({
        bookId: Number(item.book.id),
        quantity: item.quantity,
      }));

      const orderId = await createOrder(accessToken, userId, orderItems);
      const session = await createCheckoutSession(accessToken, {
        orderId,
        userId,
        amount: total,
        currency: 'usd',
        successUrl: `${window.location.origin}/checkout/success?orderId=${orderId}`,
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

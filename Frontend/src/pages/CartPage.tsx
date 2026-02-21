import { CartSummary } from '../components/cart/CartSummary';
import { CartTable } from '../components/cart/CartTable';
import { useCart } from '../state/cart/CartContext';

const CartPage = () => {
  const { items, subtotal, updateQuantity, removeFromCart } = useCart();

  return (
    <div className="grid gap-6 lg:grid-cols-[1fr_320px]">
      <section>
        <h1 className="mb-4 text-3xl font-bold text-slate-900">Your Cart</h1>
        {items.length === 0 ? <p className="rounded-2xl bg-white p-6 shadow-md">Your cart is empty.</p> : <CartTable items={items} onQuantityChange={updateQuantity} onRemove={removeFromCart} />}
      </section>
      <CartSummary subtotal={subtotal} />
    </div>
  );
};

export default CartPage;

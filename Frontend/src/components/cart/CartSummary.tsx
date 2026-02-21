interface CartSummaryProps {
  subtotal: number;
}

export const CartSummary = ({ subtotal }: CartSummaryProps) => {
  const shipping = subtotal > 0 ? 4.99 : 0;
  const total = subtotal + shipping;
  return (
    <aside className="rounded-2xl bg-white p-5 shadow-md">
      <h3 className="text-xl font-bold text-slate-900">Order Summary</h3>
      <div className="mt-4 space-y-2 text-sm">
        <div className="flex justify-between"><span className="text-slate-500">Subtotal</span><span>${subtotal.toFixed(2)}</span></div>
        <div className="flex justify-between"><span className="text-slate-500">Shipping</span><span>${shipping.toFixed(2)}</span></div>
        <div className="mt-2 flex justify-between border-t border-slate-200 pt-3 font-semibold text-slate-900"><span>Total</span><span className="text-brand-500">${total.toFixed(2)}</span></div>
      </div>
      <button className="mt-5 w-full rounded-xl bg-brand-500 py-3 font-semibold text-white hover:bg-brand-600">Proceed to Checkout</button>
    </aside>
  );
};

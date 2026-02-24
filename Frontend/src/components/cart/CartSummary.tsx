interface CartSummaryProps {
  subtotal: number;
  onCheckout: () => Promise<void>;
  isCheckoutLoading: boolean;
  checkoutError: string;
}

export const CartSummary = ({ subtotal, onCheckout, isCheckoutLoading, checkoutError }: CartSummaryProps) => {
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
      {checkoutError ? <p className="mt-3 text-sm text-red-500">{checkoutError}</p> : null}
      <button
        disabled={isCheckoutLoading || total <= 0}
        onClick={() => void onCheckout()}
        className="mt-5 w-full rounded-xl bg-brand-500 py-3 font-semibold text-white hover:bg-brand-600 disabled:cursor-not-allowed disabled:bg-slate-300"
      >
        {isCheckoutLoading ? 'Redirecting to Stripe...' : 'Proceed to Checkout'}
      </button>
    </aside>
  );
};

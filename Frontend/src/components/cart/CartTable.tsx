import { Trash2 } from 'lucide-react';
import { CartItem } from '../../state/cart/cartStorage';
import { QuantityInput } from './QuantityInput';

interface CartTableProps {
  items: CartItem[];
  onQuantityChange: (bookId: string, quantity: number) => void;
  onRemove: (bookId: string) => void;
}

export const CartTable = ({ items, onQuantityChange, onRemove }: CartTableProps) => (
  <div className="space-y-3">
    {items.map((item) => (
      <article key={item.book.id} className="grid gap-3 rounded-2xl bg-white p-4 shadow-md sm:grid-cols-[90px_1fr_auto_auto] sm:items-center">
        <img src={item.book.image} alt={item.book.title} className="h-20 w-full rounded-xl object-cover" />
        <div>
          <h3 className="font-semibold text-slate-900">{item.book.title}</h3>
          <p className="text-sm text-slate-500">${item.book.price.toFixed(2)}</p>
        </div>
        <QuantityInput value={item.quantity} onChange={(qty) => onQuantityChange(item.book.id, qty)} />
        <button onClick={() => onRemove(item.book.id)} className="justify-self-start rounded-xl p-2 text-red-500 hover:bg-red-50 sm:justify-self-end"><Trash2 size={18} /></button>
      </article>
    ))}
  </div>
);

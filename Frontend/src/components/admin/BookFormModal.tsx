import { useEffect, useState } from 'react';
import { Book } from '../../types/book';

interface BookFormModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: { title: string; author: string; price: number; stock: number; category: Book['category']; image: string }) => void;
  initial?: Book | null;
}

export const BookFormModal = ({ open, onClose, onSubmit, initial }: BookFormModalProps) => {
  const [title, setTitle] = useState('');
  const [author, setAuthor] = useState('');
  const [price, setPrice] = useState('');
  const [stock, setStock] = useState('');
  const [category, setCategory] = useState<Book['category']>('Fiction');
  const [image, setImage] = useState('');

  useEffect(() => {
    if (initial) {
      setTitle(initial.title);
      setAuthor(initial.author);
      setPrice(String(initial.price));
      setStock(String(initial.stock));
      setCategory(initial.category);
      setImage(initial.image);
      return;
    }
    setTitle('');
    setAuthor('');
    setPrice('');
    setStock('');
    setCategory('Fiction');
    setImage('');
  }, [initial, open]);

  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/50 p-4">
      <form
        className="w-full max-w-lg rounded-2xl bg-white p-6 shadow-xl"
        onSubmit={(e) => {
          e.preventDefault();
          onSubmit({ title, author, price: Number(price), stock: Number(stock), category, image });
          onClose();
        }}
      >
        <h3 className="text-xl font-bold text-slate-900">{initial ? 'Edit Book' : 'Create Book'}</h3>
        <div className="mt-4 grid gap-3 sm:grid-cols-2">
          <input required value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Title" className="rounded-xl border border-slate-200 px-4 py-2" />
          <input required value={author} onChange={(e) => setAuthor(e.target.value)} placeholder="Author" className="rounded-xl border border-slate-200 px-4 py-2" />
          <input required type="number" min="0" value={price} onChange={(e) => setPrice(e.target.value)} placeholder="Price" className="rounded-xl border border-slate-200 px-4 py-2" />
          <input required type="number" min="0" value={stock} onChange={(e) => setStock(e.target.value)} placeholder="Stock" className="rounded-xl border border-slate-200 px-4 py-2" />
          <select value={category} onChange={(e) => setCategory(e.target.value as Book['category'])} className="rounded-xl border border-slate-200 px-4 py-2 sm:col-span-2">
            <option>Fiction</option><option>Science</option><option>Biography</option><option>Technology</option><option>History</option>
          </select>
          <input value={image} onChange={(e) => setImage(e.target.value)} placeholder="Image path/link (e.g. /images/books/book1.jpg)" className="rounded-xl border border-slate-200 px-4 py-2 sm:col-span-2" />
        </div>
        <div className="mt-5 flex justify-end gap-2">
          <button type="button" onClick={onClose} className="rounded-xl bg-slate-100 px-4 py-2">Cancel</button>
          <button type="submit" className="rounded-xl bg-brand-500 px-4 py-2 font-semibold text-white hover:bg-brand-600">Save</button>
        </div>
      </form>
    </div>
  );
};

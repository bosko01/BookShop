import { Book } from '../../types/book';
import { RatingStars } from './RatingStars';

interface BookDetailsProps {
  book: Book;
  quantity: number;
  onQuantityChange: (quantity: number) => void;
  onAddToCart: () => void;
}

export const BookDetails = ({ book, quantity, onQuantityChange, onAddToCart }: BookDetailsProps) => {
  console.log("DETAIL book:", book);

  return (
  <section className="grid gap-8 rounded-2xl bg-white p-6 shadow-md lg:grid-cols-2">
    <div className="flex h-[420px] w-full items-center justify-center overflow-hidden rounded-xl bg-brand-50">
      <img src={book.image} alt={book.title} className="h-full w-full object-contain" />
    </div>
    <div className="space-y-4">
      <span className="inline-flex rounded-full bg-orange-100 px-3 py-1 text-xs font-semibold text-brand-600">{book.category}</span>
      <h1 className="text-3xl font-bold text-slate-900">{book.title}</h1>
      <p className="text-slate-500">by {book.author}</p>
      <RatingStars rating={book.rating} />
      <p className="text-3xl font-bold text-brand-500">${book.price.toFixed(2)}</p>
      <p className="text-slate-600">{book.shortDescription}</p>
      <div className="flex items-center gap-3">
        <input
          type="number"
          min={1}
          value={quantity}
          onChange={(e) => onQuantityChange(Number(e.target.value))}
          className="w-20 rounded-xl border border-slate-200 px-3 py-2 focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-orange-200"
        />
        <button onClick={onAddToCart} className="rounded-xl bg-brand-500 px-5 py-2.5 font-semibold text-white hover:bg-brand-600">
          Add to Cart
        </button>
      </div>
    </div>
  </section>

  );
};

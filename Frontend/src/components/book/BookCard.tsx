import { Link } from 'react-router-dom';
import { Book } from '../../types/book';
import { RatingStars } from './RatingStars';

export const BookCard = ({ book }: { book: Book }) => (
  <article className="rounded-2xl bg-white p-4 shadow-md transition hover:shadow-lg">
    <img src={book.image} alt={book.title} className="h-52 w-full rounded-xl object-cover" />
    <div className="mt-4 space-y-2">
      <p className="text-xs font-medium text-slate-500">{book.author}</p>
      <h3 className="line-clamp-1 text-lg font-semibold text-slate-900">{book.title}</h3>
      <RatingStars rating={book.rating} reviewsCount={book.reviewsCount} />
      <div className="flex items-center justify-between">
        <span className="text-lg font-bold text-brand-500">${book.price.toFixed(2)}</span>
        <Link to={`/books/${book.id}`} className="rounded-xl bg-brand-500 px-4 py-2 text-sm font-semibold text-white hover:bg-brand-600">
          View
        </Link>
      </div>
    </div>
  </article>
);

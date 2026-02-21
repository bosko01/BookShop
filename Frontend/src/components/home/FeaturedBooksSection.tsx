import { useEffect, useState } from 'react';
import { getFeaturedBooks } from '../../services/bookService';
import { BookGrid } from '../book/BookGrid';

export const FeaturedBooksSection = () => {
  const [books, setBooks] = useState<Awaited<ReturnType<typeof getFeaturedBooks>>>([]);

  useEffect(() => {
    getFeaturedBooks().then(setBooks).catch(() => setBooks([]));
  }, []);

  return (
    <section className="space-y-4">
      <div className="flex items-end justify-between">
        <h2 className="text-2xl font-bold text-slate-900">Featured Books</h2>
        <span className="text-sm text-slate-500">Editor picks</span>
      </div>
      <BookGrid books={books} />
    </section>
  );
};

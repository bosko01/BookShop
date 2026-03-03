import { useEffect, useState } from 'react';
import { getBooks } from '../../services/bookService';
import { Book } from '../../types/book';
import { BookCard } from '../book/BookCard';

const pickRandomBooks = (books: Book[], count: number): Book[] => {
  const shuffled = [...books].sort(() => Math.random() - 0.5);
  return shuffled.slice(0, count);
};

export const PromoBanner = () => {
  const [randomBooks, setRandomBooks] = useState<Book[]>([]);

  useEffect(() => {
    getBooks()
      .then((books) => setRandomBooks(pickRandomBooks(books, 3)))
      .catch(() => setRandomBooks([]));
  }, []);

  return (
    <section className="space-y-4">
      <div className="flex items-end justify-between">
        <h2 className="text-2xl font-bold text-slate-900">Discover Something New</h2>
        <span className="text-sm text-slate-500">3 random picks</span>
      </div>
      <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
        {randomBooks.map((book) => (
          <BookCard key={book.id} book={book} />
        ))}
      </div>
    </section>
  );
};

import { Book } from '../../types/book';
import { BookGrid } from './BookGrid';

export const RelatedBooks = ({ books }: { books: Book[] }) => (
  <section className="space-y-4">
    <h2 className="text-2xl font-bold text-slate-900">Related Books</h2>
    <BookGrid books={books} />
  </section>
);

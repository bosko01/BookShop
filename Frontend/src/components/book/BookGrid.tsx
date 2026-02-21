import { Book } from '../../types/book';
import { BookCard } from './BookCard';

export const BookGrid = ({ books }: { books: Book[] }) => (
  <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
    {books.map((book) => (
      <BookCard key={book.id} book={book} />
    ))}
  </div>
);

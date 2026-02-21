import { useMemo, useState } from 'react';
import { useParams } from 'react-router-dom';
import { BookDetails } from '../components/book/BookDetails';
import { RelatedBooks } from '../components/book/RelatedBooks';
import { Tabs } from '../components/book/Tabs';
import { getBookById, getBooks } from '../services/bookService';
import { useCart } from '../state/cart/CartContext';

const BookDetailsPage = () => {
  const { id = '' } = useParams();
  const book = getBookById(id);
  const [quantity, setQuantity] = useState(1);
  const [tab, setTab] = useState<'details' | 'reviews'>('details');
  const { addToCart } = useCart();

  const related = useMemo(() => getBooks().filter((item) => item.category === book?.category && item.id !== book?.id).slice(0, 4), [book]);

  if (!book) return <p className="rounded-2xl bg-white p-6 shadow-md">Book not found.</p>;

  return (
    <div className="space-y-8">
      <BookDetails book={book} quantity={quantity} onQuantityChange={(next) => setQuantity(Math.max(1, next))} onAddToCart={() => addToCart(book, quantity)} />
      <section className="rounded-2xl bg-white p-6 shadow-md">
        <Tabs active={tab} onChange={setTab} />
        {tab === 'details' ? <p className="text-slate-600">{book.description}</p> : <p className="text-slate-600">Customers love this book for its engaging storytelling and practical insights.</p>}
      </section>
      <RelatedBooks books={related} />
    </div>
  );
};

export default BookDetailsPage;

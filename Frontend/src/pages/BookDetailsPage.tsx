import { FormEvent, useEffect, useMemo, useState } from 'react';
import { BookDetails } from '../components/book/BookDetails';
import { RelatedBooks } from '../components/book/RelatedBooks';
import { Tabs } from '../components/book/Tabs';
import { getUserIdFromJwt } from '../services/authService';
import { getBookById, getBooks } from '../services/bookService';
import { createReview, getReviewsByBookId, Review } from '../services/reviewService';
import { useAuth } from '../state/auth/AuthContext';
import { useCart } from '../state/cart/CartContext';
import { useParams } from 'react-router-dom';

const BookDetailsPage = () => {
  const { id = '' } = useParams();
  const [book, setBook] = useState<Awaited<ReturnType<typeof getBookById>>>();
  const [books, setBooks] = useState<Awaited<ReturnType<typeof getBooks>>>([]);
  const [quantity, setQuantity] = useState(1);
  const [tab, setTab] = useState<'details' | 'reviews'>('details');
  const [reviews, setReviews] = useState<Review[]>([]);
  const [rating, setRating] = useState(5);
  const [comment, setComment] = useState('');
  const [reviewError, setReviewError] = useState('');
  const [reviewSuccess, setReviewSuccess] = useState('');
  const [loginEmail, setLoginEmail] = useState('');
  const [loginPassword, setLoginPassword] = useState('');
  const [loginError, setLoginError] = useState('');
  const { addToCart } = useCart();
  const { accessToken, login } = useAuth();

  useEffect(() => {
    getBookById(id).then(setBook).catch(() => setBook(undefined));
    getBooks().then(setBooks).catch(() => setBooks([]));
    const numericId = Number(id);
    if (!Number.isNaN(numericId)) {
      getReviewsByBookId(numericId).then(setReviews).catch(() => setReviews([]));
    }
  }, [id]);

  const related = useMemo(() => books.filter((item) => item.category === book?.category && item.id !== book?.id).slice(0, 4), [book, books]);

  const onReviewSubmit = async (event: FormEvent) => {
    event.preventDefault();
    setReviewError('');
    setReviewSuccess('');

    if (!book || !accessToken) {
      setReviewError('Morate biti prijavljeni da biste ostavili recenziju.');
      return;
    }

    const userId = getUserIdFromJwt(accessToken);
    if (!userId) {
      console.log('accessToken:', accessToken);
console.log('user ID :', userId);
      setReviewError('Nije moguće prepoznati korisnika iz tokena.');
      return;
    }

    try {
      await createReview({
        bookId: Number(book.id),
        userId,
        rating,
        comment: comment.trim() || undefined,
      }, accessToken);
      setComment('');
      setRating(5);
      setReviewSuccess('Recenzija je uspešno dodata.');
      setReviews(await getReviewsByBookId(Number(book.id)));
    } catch {
      setReviewError('Dodavanje recenzije nije uspelo.');
    }
  };

  const onLoginSubmit = async (event: FormEvent) => {
    event.preventDefault();
    setLoginError('');

    try {
      await login(loginEmail, loginPassword);
      window.location.reload();
    } catch {
      setLoginError('Pogrešni kredencijali.');
    }
  };

  if (!book) return <p className="rounded-2xl bg-white p-6 shadow-md">Book not found.</p>;

  return (
    <div className="space-y-8">
      <BookDetails book={book} quantity={quantity} onQuantityChange={(next) => setQuantity(Math.max(1, next))} onAddToCart={() => addToCart(book, quantity)} />
      <section className="rounded-2xl bg-white p-6 shadow-md">
        <Tabs active={tab} onChange={setTab} />
        {tab === 'details' ? (
          <p className="text-slate-600">{book.description}</p>
        ) : (
          <div className="space-y-6">
            <div className="space-y-3">
              <h3 className="text-lg font-semibold text-slate-900">Recenzije</h3>
              {reviews.length === 0 ? <p className="text-sm text-slate-500">Još uvek nema recenzija za ovu knjigu.</p> : null}
              <ul className="space-y-3">
                {reviews.map((review) => (
                  <li key={review.id} className="rounded-xl border border-slate-200 p-4">
                    <p className="text-sm font-medium text-slate-900">Korisnik #{review.userId}</p>
                    <p className="text-sm text-amber-500">Ocena: {review.rating}/5</p>
                    <p className="text-sm text-slate-600">{review.comment || 'Bez komentara.'}</p>
                  </li>
                ))}
              </ul>
            </div>

            {accessToken ? (
              <form className="space-y-3" onSubmit={onReviewSubmit}>
                <h4 className="text-base font-semibold text-slate-900">Dodaj recenziju</h4>
                <select value={rating} onChange={(event) => setRating(Number(event.target.value))} className="w-full rounded-xl border border-slate-200 px-4 py-3">
                  {[5, 4, 3, 2, 1].map((value) => (
                    <option key={value} value={value}>{value}</option>
                  ))}
                </select>
                <textarea
                  value={comment}
                  onChange={(event) => setComment(event.target.value)}
                  placeholder="Napišite komentar"
                  className="w-full rounded-xl border border-slate-200 px-4 py-3"
                />
                {reviewError ? <p className="text-sm text-red-500">{reviewError}</p> : null}
                {reviewSuccess ? <p className="text-sm text-emerald-600">{reviewSuccess}</p> : null}
                <button type="submit" className="rounded-xl bg-brand-500 px-4 py-2 font-semibold text-white hover:bg-brand-600">Pošalji recenziju</button>
              </form>
            ) : (
              <form className="space-y-3" onSubmit={onLoginSubmit}>
                <h4 className="text-base font-semibold text-slate-900">Prijavite se za ostavljanje recenzije</h4>
                <input
                  value={loginEmail}
                  onChange={(event) => setLoginEmail(event.target.value)}
                  type="email"
                  placeholder="Email"
                  className="w-full rounded-xl border border-slate-200 px-4 py-3"
                />
                <input
                  value={loginPassword}
                  onChange={(event) => setLoginPassword(event.target.value)}
                  type="password"
                  placeholder="Password"
                  className="w-full rounded-xl border border-slate-200 px-4 py-3"
                />
                {loginError ? <p className="text-sm text-red-500">{loginError}</p> : null}
                <button type="submit" className="rounded-xl bg-brand-500 px-4 py-2 font-semibold text-white hover:bg-brand-600">Prijavi se</button>
              </form>
            )}
          </div>
        )}
      </section>
      <RelatedBooks books={related} />
    </div>
  );
};

export default BookDetailsPage;

import { Book, BookFilter } from '../types/book';
import { request } from './apiClient';

interface ApiBookListResponse {
  id: number;
  title: string;
  price: number;
  quantityInStock: number;
}

interface ApiAuthorResponse {
  firstName: string;
  lastName: string;
}

interface ApiBookResponse extends ApiBookListResponse {
  pageCount: number;
  publisherId: number;
  authorId: number;
  genreId: number;
  bindingId: number;
  description?: string | null;
  imageUrl?: string | null;
}

interface ApiReview {
  id: number;
  bookId: number;
  rating: number;
}

interface BookRating {
  rating: number;
  reviewsCount: number;
}

const getBookRatings = (reviews: ApiReview[]): Map<number, BookRating> => {
  const ratingTotals = new Map<number, { total: number; count: number }>();

  reviews.forEach((review) => {
    const current = ratingTotals.get(review.bookId) ?? { total: 0, count: 0 };
    ratingTotals.set(review.bookId, {
      total: current.total + review.rating,
      count: current.count + 1,
    });
  });

  return new Map(
    Array.from(ratingTotals.entries()).map(([bookId, value]) => [
      bookId,
      {
        rating: value.total / value.count,
        reviewsCount: value.count,
      },
    ]),
  );
};

const mapBookList = (book: ApiBookListResponse, ratings?: Map<number, BookRating>, author?: string): Book => {
  const bookRating = ratings?.get(book.id);

  return {
  id: String(book.id),
  title: book.title,
  author: author ?? 'Unknown author',
  description: book.title,
  shortDescription: book.title,
  price: book.price,
  year: new Date().getFullYear(),
  category: 'Fiction',
  stock: book.quantityInStock,
  rating: bookRating?.rating ?? 0,
  reviewsCount: bookRating?.reviewsCount ?? 0,
  image: 'https://placehold.co/600x800/e2e8f0/334155?text=Book',
  featured: false,
  };
};

const mapBookDetails = (book: ApiBookResponse, ratings?: Map<number, BookRating>): Book => ({
  ...mapBookList(book, ratings),
  author: `Author #${book.authorId}`,
  category: `Genre #${book.genreId}` as Book['category'],
  description: book.description ?? book.title,
  shortDescription: (book.description ?? book.title).slice(0, 100),
  year: book.pageCount,
  image: book.imageUrl ?? 'https://placehold.co/600x800/e2e8f0/334155?text=Book',
});

export const getBooks = async (): Promise<Book[]> => {
  const [books, reviews] = await Promise.all([
    request<ApiBookListResponse[]>('/api/Book'),
    request<ApiReview[]>('/api/reviews'),
  ]);

  const ratings = getBookRatings(reviews);

  const detailedBooks = await Promise.all(
    books.map(async (book) => {
      const details = await request<ApiBookResponse>(`/api/book/${book.id}`);
      const author = await request<ApiAuthorResponse>(`/api/authors/${details.authorId}`);
      return {
        book,
        author: `${author.firstName} ${author.lastName}`,
      };
    }),
  );

  return detailedBooks.map(({ book, author }) => mapBookList(book, ratings, author));
};

export const getFeaturedBooks = async (): Promise<Book[]> => (await getBooks()).slice(0, 4);
export const getCategories = (books: Book[]): string[] => [...new Set(books.map((book) => book.category))];

export const getBookById = async (id: string): Promise<Book | undefined> => {
  const numericId = Number(id);
  if (Number.isNaN(numericId)) return undefined;

  const [book, reviews] = await Promise.all([
    request<ApiBookResponse>(`/api/book/${numericId}`),
    request<ApiReview[]>(`/api/reviews/by-book/${numericId}`),
  ]);

  const ratings = getBookRatings(reviews);
  return mapBookDetails(book, ratings);
};

export const filterBooks = (books: Book[], filter: BookFilter): Book[] => {
  const filtered = books.filter((book) => {
    const matchesCategory = filter.categories.length === 0 || filter.categories.includes(book.category);
    const matchesMin = filter.minPrice === undefined || book.price >= filter.minPrice;
    const matchesMax = filter.maxPrice === undefined || book.price <= filter.maxPrice;
    const matchesYear = filter.year === undefined || book.year >= filter.year;
    const query = filter.search?.trim().toLowerCase();
    const matchesSearch =
      !query || book.title.toLowerCase().includes(query) || book.author.toLowerCase().includes(query);
    return matchesCategory && matchesMin && matchesMax && matchesYear && matchesSearch;
  });

  switch (filter.sort) {
    case 'price-low':
      return filtered.sort((a, b) => a.price - b.price);
    case 'price-high':
      return filtered.sort((a, b) => b.price - a.price);
    case 'rating':
      return filtered.sort((a, b) => b.rating - a.rating);
    default:
      return filtered.sort((a, b) => Number(Boolean(b.featured)) - Number(Boolean(a.featured)));
  }
};

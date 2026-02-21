import { Book, BookFilter } from '../types/book';
import { request } from './apiClient';

interface ApiBookListResponse {
  id: number;
  title: string;
  price: number;
  quantityInStock: number;
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

const mapBookList = (book: ApiBookListResponse): Book => ({
  id: String(book.id),
  title: book.title,
  author: 'Unknown author',
  description: book.title,
  shortDescription: book.title,
  price: book.price,
  year: new Date().getFullYear(),
  category: 'Fiction',
  stock: book.quantityInStock,
  rating: 0,
  reviewsCount: 0,
  image: 'https://placehold.co/600x800/e2e8f0/334155?text=Book',
  featured: false,
});

const mapBookDetails = (book: ApiBookResponse): Book => ({
  ...mapBookList(book),
  author: `Author #${book.authorId}`,
  category: `Genre #${book.genreId}` as Book['category'],
  description: book.description ?? book.title,
  shortDescription: (book.description ?? book.title).slice(0, 100),
  year: book.pageCount,
  image: book.imageUrl ?? 'https://placehold.co/600x800/e2e8f0/334155?text=Book',
});

export const getBooks = async (): Promise<Book[]> => {
  const books = await request<ApiBookListResponse[]>('/api/book');
  return books.map(mapBookList);
};

export const getFeaturedBooks = async (): Promise<Book[]> => (await getBooks()).slice(0, 4);
export const getCategories = (books: Book[]): string[] => [...new Set(books.map((book) => book.category))];

export const getBookById = async (id: string): Promise<Book | undefined> => {
  const numericId = Number(id);
  if (Number.isNaN(numericId)) return undefined;
  const book = await request<ApiBookResponse>(`/api/book/${numericId}`);
  return mapBookDetails(book);
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

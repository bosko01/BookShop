import { Book, BookFilter } from '../types/book';

const bookData: Book[] = [
  {
    id: '1',
    title: 'The Midnight Library',
    author: 'Matt Haig',
    description: 'A thought-provoking novel about life choices, regrets, and alternate realities.',
    shortDescription: 'A novel about second chances between life and death.',
    price: 16.99,
    year: 2020,
    category: 'Fiction',
    stock: 32,
    rating: 4.6,
    reviewsCount: 248,
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=Midnight+Library',
    featured: true,
  },
  {
    id: '2',
    title: 'Atomic Habits',
    author: 'James Clear',
    description: 'A practical guide to building good habits and breaking bad ones.',
    shortDescription: 'Tiny changes, remarkable results for your daily life.',
    price: 18.5,
    year: 2018,
    category: 'Science',
    stock: 44,
    rating: 4.8,
    reviewsCount: 520,
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=Atomic+Habits',
    featured: true,
  },
  {
    id: '3',
    title: 'Steve Jobs',
    author: 'Walter Isaacson',
    description: 'The exclusive biography of Apple co-founder Steve Jobs.',
    shortDescription: 'The life story of one of technology’s greatest innovators.',
    price: 22.99,
    year: 2011,
    category: 'Biography',
    stock: 15,
    rating: 4.5,
    reviewsCount: 301,
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=Steve+Jobs',
  },
  {
    id: '4',
    title: 'Clean Code',
    author: 'Robert C. Martin',
    description: 'A handbook of agile software craftsmanship and maintainable code practices.',
    shortDescription: 'Classic software engineering principles for clean, robust code.',
    price: 35,
    year: 2008,
    category: 'Technology',
    stock: 22,
    rating: 4.7,
    reviewsCount: 412,
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=Clean+Code',
    featured: true,
  },
  {
    id: '5',
    title: 'Sapiens',
    author: 'Yuval Noah Harari',
    description: 'A brief history of humankind from the Stone Age to modern times.',
    shortDescription: 'How Homo sapiens came to dominate the world.',
    price: 19.99,
    year: 2014,
    category: 'History',
    stock: 29,
    rating: 4.7,
    reviewsCount: 680,
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=Sapiens',
    featured: true,
  },
  {
    id: '6',
    title: 'Deep Work',
    author: 'Cal Newport',
    description: 'Rules for focused success in a distracted world.',
    shortDescription: 'Master concentration and produce better results.',
    price: 17.5,
    year: 2016,
    category: 'Science',
    stock: 21,
    rating: 4.4,
    reviewsCount: 198,
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=Deep+Work',
  },
  {
    id: '7',
    title: '1984',
    author: 'George Orwell',
    description: 'A dystopian classic exploring surveillance and totalitarian control.',
    shortDescription: 'A timeless warning about power, control, and truth.',
    price: 12.99,
    year: 1949,
    category: 'Fiction',
    stock: 56,
    rating: 4.6,
    reviewsCount: 755,
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=1984',
  },
  {
    id: '8',
    title: 'Educated',
    author: 'Tara Westover',
    description: 'A memoir about family, self-invention, and the transformative power of education.',
    shortDescription: 'From isolated childhood to Cambridge PhD.',
    price: 14.99,
    year: 2018,
    category: 'Biography',
    stock: 18,
    rating: 4.5,
    reviewsCount: 342,
    image: 'https://placehold.co/600x800/e2e8f0/334155?text=Educated',
  },
];

export const getBooks = (): Book[] => [...bookData];
export const getFeaturedBooks = (): Book[] => bookData.filter((book) => book.featured).slice(0, 4);
export const getCategories = (): string[] => [...new Set(bookData.map((book) => book.category))];
export const getBookById = (id: string): Book | undefined => bookData.find((book) => book.id === id);

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

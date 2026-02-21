export type BookCategory = 'Fiction' | 'Science' | 'Biography' | 'Technology' | 'History';

export interface Book {
  id: string;
  title: string;
  author: string;
  description: string;
  shortDescription: string;
  price: number;
  year: number;
  category: BookCategory;
  stock: number;
  rating: number;
  reviewsCount: number;
  image: string;
  featured?: boolean;
}

export interface BookFilter {
  categories: string[];
  minPrice?: number;
  maxPrice?: number;
  year?: number;
  search?: string;
  sort?: 'featured' | 'price-low' | 'price-high' | 'rating';
}

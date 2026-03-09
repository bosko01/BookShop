import { useEffect, useMemo, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { BookGrid } from '../components/book/BookGrid';
import { Pagination } from '../components/shop/Pagination';
import { ShopToolbar } from '../components/shop/ShopToolbar';
import { SidebarFilters } from '../components/shop/SidebarFilters';
import { filterBooks, getBooks, getCategories } from '../services/bookService';

const PAGE_SIZE = 8;

const ShopPage = () => {
  const [params] = useSearchParams();
  const [selectedCategories, setSelectedCategories] = useState<string[]>([]);
  const [minPrice, setMinPrice] = useState('');
  const [maxPrice, setMaxPrice] = useState('');
  const [year, setYear] = useState('');
  const [sort, setSort] = useState<'featured' | 'price-low' | 'price-high' | 'rating'>('featured');
  const [page, setPage] = useState(1);
  const [books, setBooks] = useState<Awaited<ReturnType<typeof getBooks>>>([]);

  useEffect(() => {
    getBooks().then(setBooks).catch(() => setBooks([]));
  }, []);

  const filtered = useMemo(() => filterBooks(books, {
    categories: selectedCategories,
    minPrice: minPrice ? Number(minPrice) : undefined,
    maxPrice: maxPrice ? Number(maxPrice) : undefined,
    year: year ? Number(year) : undefined,
    sort,
    search: params.get('search') ?? '',
  }), [books, selectedCategories, minPrice, maxPrice, year, sort, params]);

  const paginatedBooks = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.max(1, Math.ceil(filtered.length / PAGE_SIZE));

  return (
    <div className="grid gap-6 lg:grid-cols-[280px_1fr]">
      <SidebarFilters
        categories={getCategories(books)}
        selectedCategories={selectedCategories}
        onCategoryToggle={(category) => {
          setPage(1);
          setSelectedCategories((prev) => prev.includes(category) ? prev.filter((item) => item !== category) : [...prev, category]);
        }}
        minPrice={minPrice}
        maxPrice={maxPrice}
        onMinPriceChange={(value) => { setPage(1); setMinPrice(value); }}
        onMaxPriceChange={(value) => { setPage(1); setMaxPrice(value); }}
        year={year}
        onYearChange={(value) => { setPage(1); setYear(value); }}
      />
      <section>
        <ShopToolbar count={filtered.length} sort={sort} onSortChange={(value) => { setSort(value); setPage(1); }} />
        <BookGrid books={paginatedBooks} />
        <Pagination page={page} totalPages={totalPages} onChange={setPage} />
      </section>
    </div>
  );
};

export default ShopPage;

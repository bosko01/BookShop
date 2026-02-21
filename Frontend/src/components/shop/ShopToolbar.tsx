interface ShopToolbarProps {
  count: number;
  sort: string;
  onSortChange: (value: 'featured' | 'price-low' | 'price-high' | 'rating') => void;
}

export const ShopToolbar = ({ count, sort, onSortChange }: ShopToolbarProps) => (
  <div className="mb-5 flex flex-col gap-3 rounded-2xl bg-white p-4 shadow-md sm:flex-row sm:items-center sm:justify-between">
    <p className="text-sm text-slate-500">Showing {count} results</p>
    <select value={sort} onChange={(e) => onSortChange(e.target.value as 'featured' | 'price-low' | 'price-high' | 'rating')} className="rounded-xl border border-slate-200 px-4 py-2 text-sm">
      <option value="featured">Featured</option>
      <option value="price-low">Price: Low to High</option>
      <option value="price-high">Price: High to Low</option>
      <option value="rating">Top Rated</option>
    </select>
  </div>
);

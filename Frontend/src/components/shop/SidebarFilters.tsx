interface SidebarFiltersProps {
  categories: string[];
  selectedCategories: string[];
  onCategoryToggle: (category: string) => void;
  minPrice: string;
  maxPrice: string;
  onMinPriceChange: (value: string) => void;
  onMaxPriceChange: (value: string) => void;
  year: string;
  onYearChange: (value: string) => void;
}

export const SidebarFilters = ({ categories, selectedCategories, onCategoryToggle, minPrice, maxPrice, onMinPriceChange, onMaxPriceChange, year, onYearChange }: SidebarFiltersProps) => (
  <aside className="space-y-6 rounded-2xl bg-white p-5 shadow-md">
    <div>
      <h3 className="font-semibold text-slate-900">Category</h3>
      <div className="mt-3 space-y-2 text-sm">
        {categories.map((category) => (
          <label key={category} className="flex items-center gap-2 text-slate-600">
            <input type="checkbox" checked={selectedCategories.includes(category)} onChange={() => onCategoryToggle(category)} className="rounded border-slate-300 text-brand-500 focus:ring-orange-200" />
            {category}
          </label>
        ))}
      </div>
    </div>
    <div>
      <h3 className="font-semibold text-slate-900">Price Range</h3>
      <div className="mt-3 grid grid-cols-2 gap-2">
        <input value={minPrice} onChange={(e) => onMinPriceChange(e.target.value)} placeholder="Min" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
        <input value={maxPrice} onChange={(e) => onMaxPriceChange(e.target.value)} placeholder="Max" className="rounded-xl border border-slate-200 px-3 py-2 text-sm" />
      </div>
    </div>
    <div>
      <h3 className="font-semibold text-slate-900">Year</h3>
      <select value={year} onChange={(e) => onYearChange(e.target.value)} className="mt-3 w-full rounded-xl border border-slate-200 px-3 py-2 text-sm">
        <option value="">All</option>
        <option value="2020">2020+</option>
        <option value="2015">2015+</option>
        <option value="2010">2010+</option>
      </select>
    </div>
  </aside>
);

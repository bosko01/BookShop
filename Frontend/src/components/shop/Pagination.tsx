interface PaginationProps {
  page: number;
  totalPages: number;
  onChange: (page: number) => void;
}

export const Pagination = ({ page, totalPages, onChange }: PaginationProps) => (
  <div className="mt-6 flex flex-wrap items-center gap-2">
    {Array.from({ length: totalPages }, (_, i) => i + 1).map((num) => (
      <button
        key={num}
        onClick={() => onChange(num)}
        className={`h-10 w-10 rounded-xl text-sm font-semibold ${page === num ? 'bg-brand-500 text-white' : 'bg-white text-slate-700 shadow-md'}`}
      >
        {num}
      </button>
    ))}
  </div>
);

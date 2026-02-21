interface TabsProps {
  active: 'details' | 'reviews';
  onChange: (value: 'details' | 'reviews') => void;
}

export const Tabs = ({ active, onChange }: TabsProps) => (
  <div className="mb-4 flex gap-2 border-b border-slate-200 pb-3">
    {['details', 'reviews'].map((tab) => (
      <button
        key={tab}
        onClick={() => onChange(tab as 'details' | 'reviews')}
        className={`rounded-xl px-4 py-2 text-sm font-semibold capitalize ${
          active === tab ? 'bg-brand-500 text-white' : 'bg-slate-100 text-slate-700'
        }`}
      >
        {tab}
      </button>
    ))}
  </div>
);

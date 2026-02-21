interface QuantityInputProps {
  value: number;
  onChange: (next: number) => void;
}

export const QuantityInput = ({ value, onChange }: QuantityInputProps) => (
  <div className="flex items-center rounded-xl border border-slate-200">
    <button onClick={() => onChange(Math.max(1, value - 1))} className="px-3 py-1 text-slate-600">-</button>
    <span className="px-3 text-sm font-semibold">{value}</span>
    <button onClick={() => onChange(value + 1)} className="px-3 py-1 text-slate-600">+</button>
  </div>
);

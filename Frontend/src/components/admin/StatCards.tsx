const stats = [
  { label: 'Total Sales', value: '$8,430' },
  { label: 'Orders', value: '312' },
  { label: 'Books', value: '128' },
  { label: 'Customers', value: '1,024' },
];

export const StatCards = () => (
  <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
    {stats.map((stat) => (
      <article key={stat.label} className="rounded-2xl bg-white p-5 shadow-md">
        <p className="text-sm text-slate-500">{stat.label}</p>
        <p className="mt-2 text-2xl font-bold text-slate-900">{stat.value}</p>
      </article>
    ))}
  </div>
);

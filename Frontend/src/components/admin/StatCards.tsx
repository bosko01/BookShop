interface AdminStats {
  totalSales: number;
  orders: number;
  books: number;
  customers: number;
}

export const StatCards = ({ stats }: { stats: AdminStats }) => {
  const cards = [
    { label: 'Total Sales', value: `$${stats.totalSales.toFixed(2)}` },
    { label: 'Orders', value: stats.orders.toString() },
    { label: 'Books', value: stats.books.toString() },
    { label: 'Customers', value: stats.customers.toString() },
  ];

  return (
    <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
      {cards.map((stat) => (
        <article key={stat.label} className="rounded-2xl bg-white p-5 shadow-md">
          <p className="text-sm text-slate-500">{stat.label}</p>
          <p className="mt-2 text-2xl font-bold text-slate-900">{stat.value}</p>
        </article>
      ))}
    </div>
  );
};

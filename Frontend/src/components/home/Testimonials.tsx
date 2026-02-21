const testimonials = [
  { name: 'Emma', quote: 'Beautiful layout and fast checkout. I found exactly what I wanted.' },
  { name: 'Lucas', quote: 'Great curation and fair prices. Highly recommended for book lovers.' },
  { name: 'Sophia', quote: 'The admin panel mockup is clean and easy to use for inventory updates.' },
];

export const Testimonials = () => (
  <section className="space-y-4">
    <h2 className="text-2xl font-bold text-slate-900">What readers say</h2>
    <div className="grid gap-4 md:grid-cols-3">
      {testimonials.map((item) => (
        <article key={item.name} className="rounded-2xl bg-white p-5 shadow-md">
          <p className="text-sm text-slate-600">“{item.quote}”</p>
          <p className="mt-3 font-semibold text-slate-900">{item.name}</p>
        </article>
      ))}
    </div>
  </section>
);

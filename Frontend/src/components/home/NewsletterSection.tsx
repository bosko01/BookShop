export const NewsletterSection = () => (
  <section className="rounded-2xl bg-white p-8 shadow-md">
    <h3 className="text-2xl font-bold text-slate-900">Get updates on new releases</h3>
    <p className="mt-2 text-slate-500">Subscribe for weekly picks, author highlights, and special discounts.</p>
    <div className="mt-5 flex flex-col gap-3 sm:flex-row">
      <input placeholder="Enter your email" className="w-full rounded-xl border border-slate-200 px-4 py-3" />
      <button className="rounded-xl bg-brand-500 px-6 py-3 font-semibold text-white hover:bg-brand-600">Subscribe</button>
    </div>
  </section>
);

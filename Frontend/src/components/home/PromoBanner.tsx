import { Link } from 'react-router-dom';

export const PromoBanner = () => (
  <section className="rounded-2xl bg-gradient-to-r from-slate-800 to-indigo-800 p-8 text-white shadow-md">
    <h3 className="text-2xl font-bold">Save up to 30% on selected tech titles</h3>
    <p className="mt-2 text-slate-200">Level up your skills with books picked by industry experts.</p>
    <Link to="/shop" className="mt-5 inline-flex rounded-xl bg-brand-500 px-5 py-2.5 font-semibold hover:bg-brand-600">Shop Deals</Link>
  </section>
);

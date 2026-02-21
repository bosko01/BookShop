import { Link } from 'react-router-dom';

export const HeroSection = () => (
  <section className="rounded-2xl bg-gradient-to-r from-indigo-900 via-indigo-800 to-blue-800 px-6 py-14 text-white shadow-md sm:px-10">
    <p className="mb-2 text-sm uppercase tracking-widest text-orange-200">New arrivals weekly</p>
    <h1 className="max-w-2xl text-3xl font-bold leading-tight sm:text-5xl">Discover your next favorite book at BookShop.</h1>
    <p className="mt-4 max-w-2xl text-sm text-indigo-100 sm:text-base">From classics to modern bestsellers, find stories that inspire every reader.</p>
    <Link to="/shop" className="mt-8 inline-flex rounded-xl bg-brand-500 px-6 py-3 font-semibold text-white hover:bg-brand-600">Browse Collection</Link>
  </section>
);

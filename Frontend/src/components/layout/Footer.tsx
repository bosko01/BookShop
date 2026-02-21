export const Footer = () => (
  <footer className="mt-16 border-t border-slate-200 bg-white">
    <div className="container-base grid gap-8 py-10 md:grid-cols-3">
      <div>
        <h4 className="text-lg font-bold text-slate-900">BookShop</h4>
        <p className="mt-2 text-sm text-slate-500">Curated books for curious minds.</p>
      </div>
      <div>
        <h5 className="font-semibold text-slate-800">Quick Links</h5>
        <ul className="mt-2 space-y-2 text-sm text-slate-500">
          <li>Shop</li><li>About</li><li>Contact</li>
        </ul>
      </div>
      <div>
        <h5 className="font-semibold text-slate-800">Newsletter</h5>
        <div className="mt-3 flex gap-2">
          <input className="w-full rounded-xl border border-slate-200 px-3 py-2 text-sm" placeholder="Email address" />
          <button className="rounded-xl bg-brand-500 px-4 py-2 text-sm font-semibold text-white hover:bg-brand-600">Join</button>
        </div>
      </div>
    </div>
  </footer>
);

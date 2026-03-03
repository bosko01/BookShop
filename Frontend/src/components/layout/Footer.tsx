export const Footer = () => (
  <footer className="mt-16 border-t border-slate-200 bg-white">
    <div className="container-base grid gap-8 py-10 md:grid-cols-2">
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
    </div>
  </footer>
);

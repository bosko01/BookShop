const LoginPage = () => (
  <div className="overflow-hidden rounded-2xl bg-white shadow-md lg:grid lg:grid-cols-2">
    <section className="p-6 sm:p-10">
      <h1 className="text-3xl font-bold text-slate-900">Sign in</h1>
      <p className="mt-2 text-sm text-slate-500">Welcome back! Please enter your details.</p>
      <form className="mt-6 space-y-4">
        <input type="email" placeholder="Email" className="w-full rounded-xl border border-slate-200 px-4 py-3" />
        <input type="password" placeholder="Password" className="w-full rounded-xl border border-slate-200 px-4 py-3" />
        <div className="flex items-center justify-between text-sm text-slate-500">
          <label className="flex items-center gap-2"><input type="checkbox" /> Remember me</label>
          <a href="#" className="text-brand-500">Forgot password?</a>
        </div>
        <button className="w-full rounded-xl bg-brand-500 py-3 font-semibold text-white hover:bg-brand-600">Sign In</button>
      </form>
    </section>
    <aside className="bg-gradient-to-br from-indigo-900 via-indigo-800 to-blue-700 p-10 text-white">
      <h2 className="text-3xl font-bold">BookShop</h2>
      <p className="mt-4 text-indigo-100">Manage purchases, save your wishlist, and track every order in one place.</p>
    </aside>
  </div>
);

export default LoginPage;

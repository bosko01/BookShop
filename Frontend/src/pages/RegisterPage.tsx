import { FormEvent, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { register } from '../services/authService';

const RegisterPage = () => {
  const navigate = useNavigate();
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const onSubmit = async (event: FormEvent) => {
    event.preventDefault();
    setError('');
    setSuccess('');

    try {
      await register({ firstName, lastName, email, password, role: 0 });
      setSuccess('Uspešno ste se registrovali. Sada možete da se prijavite.');
      setTimeout(() => navigate('/login'), 1200);
    } catch {
      setError('Registracija nije uspela. Proverite podatke i pokušajte ponovo.');
    }
  };

  return (
    <div className="overflow-hidden rounded-2xl bg-white shadow-md lg:grid lg:grid-cols-2">
      <section className="p-6 sm:p-10">
        <h1 className="text-3xl font-bold text-slate-900">Registracija</h1>
        <p className="mt-2 text-sm text-slate-500">Kreirajte nalog da biste mogli da kupujete knjige.</p>
        <form className="mt-6 space-y-4" onSubmit={onSubmit}>
          <input value={firstName} onChange={(e) => setFirstName(e.target.value)} required placeholder="Ime" className="w-full rounded-xl border border-slate-200 px-4 py-3" />
          <input value={lastName} onChange={(e) => setLastName(e.target.value)} required placeholder="Prezime" className="w-full rounded-xl border border-slate-200 px-4 py-3" />
          <input value={email} onChange={(e) => setEmail(e.target.value)} required type="email" placeholder="Email" className="w-full rounded-xl border border-slate-200 px-4 py-3" />
          <input value={password} onChange={(e) => setPassword(e.target.value)} required minLength={6} type="password" placeholder="Lozinka" className="w-full rounded-xl border border-slate-200 px-4 py-3" />
          {error ? <p className="text-sm text-red-500">{error}</p> : null}
          {success ? <p className="text-sm text-emerald-600">{success}</p> : null}
          <button type="submit" className="w-full rounded-xl bg-brand-500 py-3 font-semibold text-white hover:bg-brand-600">Napravi nalog</button>
        </form>
        <p className="mt-4 text-sm text-slate-500">
          Već imate nalog?{' '}
          <Link to="/login" className="font-medium text-brand-500 hover:text-brand-600">Prijavite se</Link>
        </p>
      </section>
      <aside className="bg-gradient-to-br from-indigo-900 via-indigo-800 to-blue-700 p-10 text-white">
        <h2 className="text-3xl font-bold">BookShop</h2>
        <p className="mt-4 text-indigo-100">Registrujte se i uživajte u brzoj kupovini i praćenju porudžbina.</p>
      </aside>
    </div>
  );
};

export default RegisterPage;

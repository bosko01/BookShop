import { Outlet } from 'react-router-dom';
import { Footer } from '../../components/layout/Footer';
import { Header } from '../../components/layout/Header';

export const PublicLayout = () => (
  <div className="min-h-screen bg-slate-50">
    <Header />
    <main className="container-base py-8"><Outlet /></main>
    <Footer />
  </div>
);

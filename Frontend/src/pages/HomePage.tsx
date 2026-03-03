import { FeaturedBooksSection } from '../components/home/FeaturedBooksSection';
import { HeroSection } from '../components/home/HeroSection';
import { PromoBanner } from '../components/home/PromoBanner';
import { Testimonials } from '../components/home/Testimonials';

const HomePage = () => (
  <div className="space-y-10">
    <HeroSection />
    <FeaturedBooksSection />
    <PromoBanner />
    <Testimonials />
  </div>
);

export default HomePage;

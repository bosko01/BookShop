import { FeaturedBooksSection } from '../components/home/FeaturedBooksSection';
import { HeroSection } from '../components/home/HeroSection';
import { NewsletterSection } from '../components/home/NewsletterSection';
import { PromoBanner } from '../components/home/PromoBanner';
import { Testimonials } from '../components/home/Testimonials';

const HomePage = () => (
  <div className="space-y-10">
    <HeroSection />
    <FeaturedBooksSection />
    <PromoBanner />
    <Testimonials />
    <NewsletterSection />
  </div>
);

export default HomePage;

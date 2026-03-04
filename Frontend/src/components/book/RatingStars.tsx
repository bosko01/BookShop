import { Star } from 'lucide-react';

interface RatingStarsProps {
  rating: number;
  reviewsCount?: number;
}

export const RatingStars = ({ rating, reviewsCount }: RatingStarsProps) => (
  <div className="flex items-center gap-1 text-amber-400">
    {[1, 2, 3, 4, 5].map((star) => (
      <Star
        key={star}
        size={16}
        className={star <= Math.round(rating) ? 'fill-amber-400' : 'text-slate-300'}
      />
    ))}
    <span className="ml-1 text-xs font-medium text-slate-500">{rating.toFixed(1)}</span>
    {reviewsCount !== undefined ? (
      <span className="text-xs text-slate-400">({reviewsCount})</span>
    ) : null}
  </div>
);

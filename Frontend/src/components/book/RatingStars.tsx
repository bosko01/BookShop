import { Star } from 'lucide-react';

export const RatingStars = ({ rating }: { rating: number }) => (
  <div className="flex items-center gap-1 text-amber-400">
    {[1, 2, 3, 4, 5].map((star) => (
      <Star
        key={star}
        size={16}
        className={star <= Math.round(rating) ? 'fill-amber-400' : 'text-slate-300'}
      />
    ))}
    <span className="ml-1 text-xs font-medium text-slate-500">{rating.toFixed(1)}</span>
  </div>
);

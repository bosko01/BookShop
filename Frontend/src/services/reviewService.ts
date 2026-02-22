import { request } from './apiClient';

export interface Review {
  id: number;
  bookId: number;
  userId: number;
  rating: number;
  comment: string | null;
}

interface CreateReviewPayload {
  bookId: number;
  userId: number;
  rating: number;
  comment?: string;
}

export const getReviewsByBookId = (bookId: number) => request<Review[]>(`/api/reviews/by-book/${bookId}`);

export const createReview = (payload: CreateReviewPayload) =>
  request<number>('/api/reviews', {
    method: 'POST',
    body: payload,
  });

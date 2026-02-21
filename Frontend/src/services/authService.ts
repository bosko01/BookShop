import { request } from './apiClient';

interface LoginResponse {
  accessToken: string;
  refreshToken: string;
}

interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: number;
}

export const login = (email: string, password: string) =>
  request<LoginResponse>('/api/auth/login', {
    method: 'POST',
    body: { email, password },
  });

export const register = (payload: RegisterRequest) =>
  request<number>('/api/users', {
    method: 'POST',
    body: payload,
  });

export const getRoleFromJwt = (token: string): string | null => {
  try {
    const payloadBase64 = token.split('.')[1];
    if (!payloadBase64) return null;
    const payload = JSON.parse(atob(payloadBase64)) as Record<string, string>;
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null;
  } catch {
    return null;
  }
};

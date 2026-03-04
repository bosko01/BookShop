type RequestOptions = {
  method?: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';
  body?: unknown;
  headers?: Record<string, string>;
  token?: string;
  skipAuthRefresh?: boolean;
};

interface RefreshTokenResponse {
  accessToken: string;
  refreshToken: string;
}

const API_BASE_URL = 'https://localhost:7151';
const ACCESS_TOKEN_KEY = 'bookshop_access_token';
const REFRESH_TOKEN_KEY = 'bookshop_refresh_token';

const refreshAccessToken = async (): Promise<string | null> => {
  const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
  if (!refreshToken) {
    return null;
  }

  const response = await fetch(`${API_BASE_URL}/api/auth/refresh`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ refreshToken }),
  });

  if (!response.ok) {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    return null;
  }

  const data = (await response.json()) as RefreshTokenResponse;
  localStorage.setItem(ACCESS_TOKEN_KEY, data.accessToken);
  localStorage.setItem(REFRESH_TOKEN_KEY, data.refreshToken);
  return data.accessToken;
};

export const request = async <T>(path: string, options: RequestOptions = {}): Promise<T> => {
  const url = `${API_BASE_URL}${path}`;

  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...(options.headers ?? {}),
  };

  const token = options.token ?? localStorage.getItem(ACCESS_TOKEN_KEY);
  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  const response = await fetch(url, {
    method: options.method ?? 'GET',
    headers,
    body: options.body ? JSON.stringify(options.body) : undefined,
  });

  if (response.status === 401 && !options.skipAuthRefresh) {
    const refreshedToken = await refreshAccessToken();
    if (refreshedToken) {
      return request<T>(path, { ...options, token: refreshedToken, skipAuthRefresh: true });
    }
  }

  if (!response.ok) {
    throw new Error(`HTTP ${response.status}`);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
};

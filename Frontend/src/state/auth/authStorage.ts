export const ACCESS_TOKEN_KEY = 'bookshop_access_token';
export const REFRESH_TOKEN_KEY = 'bookshop_refresh_token';

export const getStoredAccessToken = () => localStorage.getItem(ACCESS_TOKEN_KEY);
export const getStoredRefreshToken = () => localStorage.getItem(REFRESH_TOKEN_KEY);

export const storeAuthTokens = (accessToken: string, refreshToken: string) => {
  localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
  localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
};

export const clearAuthTokens = () => {
  localStorage.removeItem(ACCESS_TOKEN_KEY);
  localStorage.removeItem(REFRESH_TOKEN_KEY);
};

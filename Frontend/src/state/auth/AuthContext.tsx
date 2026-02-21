import { ReactNode, createContext, useContext, useMemo, useState } from 'react';
import { getRoleFromJwt, login as loginRequest } from '../../services/authService';

interface AuthState {
  accessToken: string | null;
  role: string | null;
  isAdmin: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

const ACCESS_TOKEN_KEY = 'bookshop_access_token';

const AuthContext = createContext<AuthState | undefined>(undefined);

const readInitialToken = () => localStorage.getItem(ACCESS_TOKEN_KEY);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [accessToken, setAccessToken] = useState<string | null>(readInitialToken);

  const role = useMemo(() => (accessToken ? getRoleFromJwt(accessToken) : null), [accessToken]);

  const login = async (email: string, password: string) => {
    const response = await loginRequest(email, password);
    localStorage.setItem(ACCESS_TOKEN_KEY, response.accessToken);
    setAccessToken(response.accessToken);
  };

  const logout = () => {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    setAccessToken(null);
  };

  return (
    <AuthContext.Provider value={{ accessToken, role, isAdmin: role === 'Admin', login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used inside AuthProvider.');
  }
  return context;
};

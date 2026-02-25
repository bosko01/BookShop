type RequestOptions = {
  method?: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';
  body?: unknown;
  headers?: Record<string, string>;
  token?: string;
};

export const request = async <T>(path: string, options: RequestOptions = {}): Promise<T> => {
  const url = `https://localhost:7151${path}`;

  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...(options.headers ?? {}),
  };

  if (options.token) {
    headers.Authorization = `Bearer ${options.token}`;
  }

  const response = await fetch(url, {
    method: options.method ?? 'GET',
    headers,
    body: options.body ? JSON.stringify(options.body) : undefined,
  });

  if (!response.ok) {
    // možeš dodatno: const text = await response.text();
    throw new Error(`HTTP ${response.status}`);
  }

  // ako endpoint vraća empty body, prilagodi
  return (await response.json()) as T;
};
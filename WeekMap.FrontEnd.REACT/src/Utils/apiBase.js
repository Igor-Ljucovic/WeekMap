export const API_BASE = process.env.REACT_APP_API_URL ?? '';

export const authFetch = (url, options = {}) => {
  const token = window.sessionStorage.getItem('auth_token');
  return fetch(url, {
    ...options,
    headers: {
      ...options.headers,
      ...(token ? { Authorization: `Bearer ${token}` } : {})
    }
  });
};

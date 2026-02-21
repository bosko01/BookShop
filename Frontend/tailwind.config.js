/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        brand: {
          500: '#F97316',
          600: '#EA580C'
        }
      }
    }
  },
  plugins: [],
};

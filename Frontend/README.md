# BookShop Frontend

## Run locally
```bash
npm install
npm run dev
```

## Tailwind setup notes
This project already includes TailwindCSS configuration:
- `tailwind.config.js` with content paths for `index.html` and `src/**/*.{ts,tsx}`.
- `postcss.config.js` with `tailwindcss` + `autoprefixer` plugins.
- `src/index.css` imports Tailwind layers:
  - `@tailwind base;`
  - `@tailwind components;`
  - `@tailwind utilities;`

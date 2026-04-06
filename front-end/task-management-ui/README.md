# Task Management UI

A modern React + Vite frontend for the Task Management Application. Features user authentication, project and task management with a responsive UI powered by Chakra UI.

## 🚀 Quick Start

```bash
npm install
npm run dev
```

Runs on: `http://localhost:5173`

## 📦 Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run lint` - Run ESLint
- `npm run preview` - Preview production build

## 🛠️ Tech Stack

- **React 19** - UI framework
- **Vite** - Build tool with fast HMR
- **Chakra UI** - Component library
- **React Router** - Client-side routing
- **React Query** - Data fetching & caching
- **Axios** - HTTP client
- **React Hot Toast** - Notifications
- **Framer Motion** - Animations

## 📁 Project Structure

```
src/
├── api/           # API endpoints (auth, tasks, projects, users)
├── components/    # Reusable UI components (Modal, Table, Badge, etc.)
├── pages/         # App pages (Login, Register, Projects, ProjectDetails)
├── utils/         # Utility functions
├── App.jsx        # Main app component
└── main.jsx       # Entry point
```

## 🔒 Authentication

Protected routes are managed via the `RequireAuth` component. Users must log in to access the main app.

## 📡 API Integration

The app communicates with the backend API at `https://localhost:7114/api`. HTTP client configuration is in `src/api/httpClient.js`.

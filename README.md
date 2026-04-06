# 📋 Task Management App

A full-stack task management application with a .NET 10 backend API and React + Vite frontend. Features JWT authentication, event-driven architecture for audit logging, and comprehensive test coverage.

## 🎯 Overview

This project demonstrates modern software development practices:
- **Backend**: .NET 10 ASP.NET Core with service-oriented architecture
- **Frontend**: React + Vite for fast, responsive UI
- **Architecture**: Event-driven system with automatic audit trail creation
- **Testing**: 25+ comprehensive unit and integration tests using xUnit and Moq
- **Security**: JWT authentication with BCrypt password hashing

Perfect for learning about event-driven architecture, dependency injection, and full-stack development with .NET and React.

## ⚙️ Installation

### Backend Setup

```bash
cd back-end
dotnet restore
dotnet ef database update
dotnet run
```

Backend runs on: `https://localhost:7114/api`

### Frontend Setup

```bash
cd front-end/task-management-ui
npm install
npm run dev
```

Frontend runs on: `http://localhost:5173`

## 🚀 Usage

### Backend - Example API Calls

**Login:**
```bash
curl -X POST https://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password123"}'
```

**Create Task:**
```bash
curl -X POST https://localhost:5000/api/tasks \
  -H "Authorization: Bearer <JWT_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "My Task",
    "description": "Task description",
    "projectId": "...",
    "userId": "..."
  }'
```

**View Task Audit Log:**
```bash
curl -X GET https://localhost:5000/api/tasklogs/{taskId} \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

### Run Tests

```bash
dotnet test TaskManagementApp.Tests
```

## ✨ Features

- ✅ **User Management** - Registration, authentication
- ✅ **Projects** - Create, organize, and manage projects
- ✅ **Tasks** - Full CRUD with status tracking and assignment
- ✅ **Audit Logging** - Automatic task log creation via events
- ✅ **JWT Authentication** - Secure token-based auth with BCrypt
- ✅ **Event-Driven** - Decoupled services through event publishing
- ✅ **Comprehensive Tests** - 25+ unit tests

## 🛠️ Tech Stack

**Backend:**
- .NET 10 / C# 14.0
- ASP.NET Core
- Entity Framework Core
- SQLite
- xUnit, Moq (testing)

**Frontend:**
- React 18+
- Vite
- Axios / Fetch
- CSS (styling)

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/auth/login` | Login user |
| POST | `/users` | Create user |
| GET | `/users` | Get all users |
| POST | `/projects` | Create project |
| GET | `/projects` | Get all projects |
| POST | `/tasks` | Create task |
| GET | `/tasks` | Get all tasks |
| GET | `/tasklogs/{taskId}` | Get task audit log |

## 📁 Project Structure

```
Task-Management-App/
├── back-end/
│   ├── TaskManagementApp/          # Main API project
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Models/
│   │   ├── Events/
│   │   └── Program.cs
│   └── TaskManagementApp.Tests/    # Test project (25+ tests)
│
└── front-end/
    └── task-management-ui/         # React + Vite app
        ├── src/api/
        ├── src/assets/
        ├── src/components/
        ├── src/pages/
        ├── src/utils/
        ├── App.jsx
        ├── main.jsx
        └── vite.config.js
```

## 🤝 Contributing

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Add tests for new features
3. Ensure all tests pass: `dotnet test`
4. Commit and push your changes
5. Create a pull request

## 📝 License

MIT License  
See `LICENSE` file for details.
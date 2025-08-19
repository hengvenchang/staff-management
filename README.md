# Staff Management System

A full-stack staff management system with a .NET 8 backend (ASP.NET Core Web API) and a React frontend.

---

## Backend

### Overview

The backend is built with ASP.NET Core Web API and uses SQLite for data storage. It provides RESTful endpoints for managing staff records, including CRUD operations, search, and export to Excel/PDF.

### Project Structure

```
Backend/
  StaffManagementApi/
    Controllers/
    Data/
    Dtos/
    Migrations/
    Models/
    Services/
    appsettings.json
    Program.cs
    staff.db
    ...
  StaffManagementApi.Tests/
    StaffServiceTests.cs
    ...
```

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQLite](https://www.sqlite.org/download.html) (optional, DB file is created automatically)

### Setup & Run

1. **Navigate to the backend directory:**
   ```sh
   cd Backend/StaffManagementApi
   ```

2. **Restore dependencies:**
   ```sh
   dotnet restore
   ```

3. **Apply migrations (if needed):**
   ```sh
   dotnet ef database update
   ```

4. **Run the API:**
   ```sh
   dotnet run
   ```

   The API will be available at `https://localhost:5080` (or as configured).

### API Endpoints

- `GET /api/staff` — Get all staff
- `GET /api/staff/{id}` — Get staff by ID
- `POST /api/staff` — Add new staff
- `PUT /api/staff/{id}` — Update staff
- `DELETE /api/staff/{id}` — Delete staff
- `GET /api/staff/search` — Search staff

### Testing

Unit tests are located in `StaffManagementApi.Tests`:

```sh
cd Backend/StaffManagementApi.Tests
dotnet test
```

---

## Frontend

### Overview

The frontend is built with React and consumes the RESTful API provided by the backend. It offers a user-friendly interface for managing staff records.

### Project Structure

```
Frontend/
  public/
  src/
    components/
    pages/
    App.js
    index.js
    ...
```

### Prerequisites

- [Node.js](https://nodejs.org/) (check with `node --version`)
- npm 10.5.0 (check with `npm --version`)

### Setup & Run

1. **Navigate to the frontend directory:**
   ```sh
   cd Frontend
   ```

2. **Install dependencies:**
   ```sh
   npm install
   ```

3. **Run the development server:**
   ```sh
   npm start
   ```

   The frontend will be available at `http://localhost:3000` (or as configured).

### Features

- View, add, update, and delete staff records
- Search staff by various criteria
- Export staff records to Excel or PDF
- Responsive design for mobile and desktop

### Testing

To run tests for the frontend, use:

```sh
npm test
```

---

## Conclusion

This staff management system provides a comprehensive solution for managing staff records with a modern web application stack. The separation of concerns between the frontend and backend, along with the use of RESTful APIs, ensures a scalable and maintainable codebase.

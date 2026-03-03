# 💰 Personal Expense Tracker (Backend API)

**Backend API built with ASP.NET Core**  
This project is a backend-focused portfolio showcasing **Clean Architecture, CQRS, and Domain-Driven Design (DDD)** principles.  

It provides a fully functional API to manage users and personal expenses.

---

## 🚀 Features

### ✅ Backend API
- User authentication with JWT (register/login)  
- CRUD operations for personal expenses (Create, Read, Update, Delete)  
- Fully implemented **domain logic** using DDD (Entities, Value Objects, Domain Events)  
- **Clean Architecture** layers (Domain, Application, Infrastructure, Presentation/API)  
- **CQRS** separation for commands (write) and queries (read)  
- Unit tests for domain and application logic  
- Swagger documentation for testing endpoints  

> Frontend interface is planned, but the API is fully functional and testable via Swagger or Postman.

---

## 🧰 Tech Stack

| Layer / Component | Technology |
|------------------|------------|
| Backend | ASP.NET Core 9 |
| Architecture | Clean Architecture |
| Patterns | CQRS, Domain-Driven Design, MediatR |
| Database | EF Core (or configured via migrations) |
| Authentication | JWT / Token-based |
| Testing | xUnit / unit tests |

---

## 📦 Installation / Local Setup

1. **Clone the repository**
```bash
git clone https://github.com/JAnMorss/Personal-Expense-Tracker.git
cd Personal-Expense-Tracker

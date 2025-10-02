# 📚 Exam Management System

An ASP.NET Core MVC application for managing university exams.  
This system is designed to streamline scheduling, invigilation, attendance, exam distribution, and grading.  

---

## ✨ Features

### 👩‍💼 Roles & Responsibilities
- **Admin**
  - Manage users and roles
  - System-wide configuration  
- **Department**
  - Manage courses
  - Assign lecturers to courses  
- **Scheduler**
  - Schedule exams (date, duration, room)  
- **Question Maker**
  - Upload exam files (questions)  
- **Printer**
  - Access and print exam files  
- **Invigilator**
  - Assigned to rooms during exams  
- **Lecturer**
  - Input and manage student grades for their courses  
- **Student**
  - View exam schedule
  - Attend exams
  - View grades  

### 📖 Core Modules
- **User Management**
  - Roles-based system (`Admin`, `Lecturer`, `Student`, etc.)
- **Course & Exam Management**
  - Create and manage courses
  - Schedule exams with rooms, durations, and invigilators
- **Attendance**
  - Track student presence in each exam
- **Exam File Upload**
  - Question makers can upload exam PDFs
  - Printers can download assigned exam files
- **Grading**
  - Lecturers input grades for students who attended exams

---

## 🛠️ Tech Stack
- **Backend:** ASP.NET Core MVC  
- **Frontend:** Razor Pages, Bootstrap  
- **Database:** SQL Server / EF Core ORM  
- **Language:** C#  

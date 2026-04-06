\# VGC College Management System



ASP.NET Core MVC application for managing students, courses, and academic progress across three branches.



\## How to run locally



1\. Clone the repository

2\. Open the solution in Visual Studio

3\. Update the connection string in `appsettings.json` if needed

4\. Run the following commands in the project folder:

5\. Open browser at `https://localhost:xxxx`



\## Seeded demo accounts



| Role    | Email              | Password     |

|---------|--------------------|--------------|

| Admin   | admin@vgc.ie       | Admin123!    |

| Faculty | faculty@vgc.ie     | Faculty123!  |

| Student | student1@vgc.ie    | Student123!  |

| Student | student2@vgc.ie    | Student123!  |



\## How to run tests

\## Design decisions



\- SQLite not used; LocalDB (SQL Server) used for simplicity on Windows

\- Seed data runs automatically on startup

\- Faculty can only see students enrolled in their assigned courses

\- Students cannot view exam results until Admin releases them (ResultsReleased flag)

\- Authorization enforced server-side on all controllers with \[Authorize(Roles = "...")]


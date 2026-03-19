# Fresh Tomatoes 🍅

Fresh Tomatoes is a simple, stripped-down version of Rotten Tomatoes. It’s a small movie database where users can browse movies and leave ratings.
The goal of the project is to keep things simple and focus on the basics of a full-stack CRUD movie rating app with a clean, user-friendly interface.

[...]

## Tech-stack

Frontend: Angular
Backend: C# ASP.net core
Database: MsSQL

## Architecture

The frontend is built with Angular and is responsible for the user interface and user interactions. It communicates with the backend through a RESTful API.

The backend is an ASP.NET Core Web API written in C#, which handles business logic, data validation, and communication with the database. Data is stored in an MS SQL Server database and accessed using Entity Framework Core.

This separation keeps the frontend and backend loosely coupled, making the application easier to maintain and extend.

## CI/CD Workflows

This project includes automated workflows to maintain code quality and documentation:

### Daily Documentation Updater
**File:** `.github/workflows/daily-doc-updater.md`

Automatically reviews and updates project documentation based on recent code changes and merged pull requests. This workflow:
- Scans merged PRs from the last 24 hours
- Analyzes code changes to identify new features and modifications
- Updates documentation files to reflect the latest codebase state
- Follows the Diátaxis documentation framework (Tutorials, How-to Guides, Reference, Explanation)

Runs daily at 6am UTC or can be triggered manually via workflow dispatch.

### Daily Repo Status
**File:** `.github/workflows/daily-repo-status.md`

Creates daily repository status reports to track project health and activity. This workflow:
- Gathers recent repository activity (issues, PRs, discussions, releases, code changes)
- Generates GitHub issues with productivity insights and community highlights
- Provides project recommendations and actionable next steps
- Helps maintainers track progress and stay informed

Runs daily or can be triggered manually via workflow dispatch.

## Feature plan

### Week 5
*Kick-off week - no features to be planned here*

### Week 6
**Feature 1:** Frontend setup

**Feature 2:** Backend setup

### Week 7
*Winter vacation - nothing planned.*

### Week 8
**Feature 1:** Login system

**Feature 2:** Basic front end interface

### Week 9
**Feature 1:** Rating system

**Feature 2:** [...]

### Week 10
**Feature 1:** [...]

**Feature 2:** [...]

### Week 11
**Feature 1:** [...]

**Feature 2:** [...]

### Week 12
**Feature 1:** [...]

**Feature 2:** [...]

### Week 13
**Feature 1:** [...]

**Feature 2:** [...]

### Week 14
*Easter vacation - nothing planned.*

### Week 15
**Feature 1:** [...]

**Feature 2:** [...]

### Week 16
**Feature 1:** [...]

**Feature 2:** [...]

### Week 17
**Feature 1:** [...]

**Feature 2:** [...]

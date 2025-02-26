# TechXpress Project Structure Documentation

## Solution Overview

TechXpress is structured as a multi-project solution with clear separation of concerns. The solution (`TechXpress.sln`) contains the following key components:

```
TechXpress/
├── Admin/                 # Administrative MVC application
├── Web/                   # User-facing MVC application
├── Business/              # Business logic layer
├── Common/                # Shared utilities and helpers
├── Domain/                # Domain models and entities
├── Infrastructure/        # Data access and external services
├── SQL.sql                # Database scripts
├── docs/                  # Project documentation
└── obj/                   # Compiled objects (auto-generated)
```

## Architectural Approach

Our solution follows a layered architecture with separation between user and administrative interfaces:

### Presentation Layer
- **Web (MVC)**: Customer-facing application with features for end users
- **Admin (MVC)**: Administrative interface with management capabilities

### Application Core
- **Business**: Contains service implementations, business workflows, and application logic
- **Domain**: Contains business entities, interfaces, and domain logic

### Infrastructure
- **Infrastructure**: Implements data access, external service integrations, and cross-cutting concerns
- **Common**: Provides shared utilities, extensions, and helper classes

## Key Design Decisions

### Separate MVC Projects for User and Admin
We maintain separate MVC projects for end-users and administrators to:
- Provide tailored experiences for different user types
- Implement appropriate security boundaries
- Allow independent deployment and scaling
- Enable specialized development teams for each interface

### Layered Dependency Flow
Dependencies flow inward, with Domain at the core:
- Web/Admin → Business → Domain
- Infrastructure → Domain
- All layers may reference Common

## Component Responsibilities

### Web (MVC)
- Public-facing user interface
- User authentication and authorization
- End-user feature implementation
- Responsive design for various devices

### Admin (MVC)
- Administrative dashboard
- User/content management features
- System configuration
- Reporting and monitoring tools

### Business
- Implementation of business workflows
- Service implementations
- Validation logic
- Orchestration of domain operations

### Domain
- Business entities and value objects
- Interface definitions for repositories
- Domain events and logic
- Business rules and constraints

### Infrastructure
- Database operations and ORM configuration
- Repository implementations
- External API clients
- File storage operations
- Email services
- Caching mechanisms


### Common
- Extension methods
- Utility classes
- Shared DTOs
- Constants and enumerations

## Development Guidelines

### Cross-Project Communication
- Projects should only reference their direct dependencies
- Use interfaces defined in Domain for cross-layer communication
- Avoid circular dependencies between projects

### Code Organization
- Follow consistent folder structure within each project
- Group related functionality into feature folders where appropriate
- Use consistent naming conventions across projects

### Database Access
- All database operations should go through the Infrastructure layer
- Use repository pattern for data access
- Keep SQL scripts organized and versioned in SQL.sql

## Team Collaboration

When working on features that span multiple projects:
1. Start with Domain models and interfaces
2. Implement Infrastructure components next
3. Add Business logic services
4. Finally, develop the UI components in Web or Admin

This workflow ensures that dependencies are available when needed and promotes a domain-driven design approach.
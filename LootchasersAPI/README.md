# Running the Project with Docker

This project is containerized using Docker, allowing for consistent and isolated execution environments. Follow the steps below to build and run the project using Docker Compose.

## Prerequisites

- Docker version 20.10 or higher
- Docker Compose version 1.29 or higher

## Setup Instructions

1. Clone the repository and navigate to the project directory.

2. Build and start the services using Docker Compose:

   ```bash
   docker-compose up --build
   ```

3. Access the application at `http://localhost:8080`.

## Configuration

- The application exposes the following ports:
  - `8080`: Main application endpoint
  - `8081`: Secondary service endpoint

- Environment variables can be configured in a `.env` file (if applicable).

## Notes

- Ensure the required .NET version (8.0) is installed in the base image.
- The application runs under a non-root user for enhanced security.

For further details, refer to the project's documentation or contact the development team.
# Stack
-  Database: PostgreSQL
-  Backend: .NET 9 WebAPI, REST API & SignalR. Documentation with Swagger. Authorization with JWT.
-  Frontend: AngularTS 19.2.3, Node: 22.14.0

## Prerequisites
Before you begin, ensure you have the following installed on your system:

- Docker
- Node.js 22.14.0 (for frontend development)
- AngularCLI 19.2.3
- NPM (Node Package Manager)

## Project Structure

- **Backend**: Located in the `/repos/CharityShop.Backend` directory, a .NET 9 WebAPI that uses PostgreSQL as database provider. API communication via REST API and SignalR.
- **Frontend**: Located in the `/repos/CharityShop.Frontend/CharityShop.Frontend` directory, built with AngularTS 19.

## Getting Started

Follow the steps below to set up the project on your local machine.

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/kristeninho/CharityShop.git
cd CharityShop
```

### 2. Backend and Database Setup (Docker)

Make sure Docker is installed and running. Then, follow these steps to set up the backend and database:
#### 2.1 Docker Compose
In the root directory of the project, you should have a docker-compose.yml file that defines the services for the backend, frontend, and PostgreSQL.

Run the following command to build and start the Docker containers:
```docker-compose up --build```

This will:
1) Build and run the backend API container at port ```http://localhost:8080```
2) Set up the PostgreSQL database container with pgAdmin at port ```http://localhost:5050```
```
User: admin@admin.com
Password: admin
```
3) Automatically seed the database when the container starts

### 3. Frontend setup (NPM)
The frontend is an Angular application. Follow these steps to set it up.

#### 3.1 Install frontend dependencies
Navigate to the frontend directory and install the required dependencies:
```
cd CharityShop.Frontend/CharityShop.Frontend
npm install
```
#### 3.2 Run the Frontend Development Server
Once the dependencies are installed, you can run the Angular development server to serve frontend at ```http://localhost:4200```
```
npm start
```
#### NB! Proxy for communicating with dockerized backend via localhost:8080 has been configured for ```npm start``` only.

### 4. Access the application
#### Frontend: Visit http://localhost:4200 in your browser to access the frontend application.
```
Username: user
Password: pass
```
#### Backend API: The backend API should be accessible at http://localhost:8080. Documentation provided in http://localhost:8080/swagger

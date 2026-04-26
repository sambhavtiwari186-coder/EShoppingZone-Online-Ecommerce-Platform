# EShoppingZone Online Ecommerce Platform - Backend

This repository contains the microservices-based backend for the EShoppingZone Ecommerce Platform. Built with .NET 10.0, it follows a robust architectural pattern suitable for scalable ecommerce applications.

## 🚀 Architecture Overview

The system consists of 7 specialized microservices and a central API Gateway:

1.  **Product API**: Manages the product catalog.
2.  **Profile API**: Handles user profiles and authentication.
3.  **Cart API**: Manages shopping carts and wishlists.
4.  **Orders API**: Orchestrates order placement and fulfillment.
5.  **Wallet API**: Handles customer credits and transactions.
6.  **Review API**: Manages product reviews and ratings.
7.  **Notify API**: Handles system notifications (Email/SMS simulation).
8.  **API Gateway**: A YARP-based gateway that unifies all services under a single entry point (Port 5000).

## 🛠️ Tech Stack

*   **Framework**: .NET 10.0 (ASP.NET Core)
*   **Database**: SQLite (with EF Core)
*   **Inter-service Communication**: HttpClientFactory with Polly (Retry & Circuit Breaker)
*   **Observability**: Serilog (Audit logging) & Health Checks
*   **Documentation**: Swagger/OpenAPI 3.0
*   **Containerization**: Docker & Docker Compose

## 📦 How to Run

### Prerequisites
*   [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed.

### Quick Start
1.  Clone the repository.
2.  Open a terminal in the root directory.
3.  Run the following command:

```bash
docker-compose up --build
```

The system will automatically:
*   Build all 8 container images.
*   Setup service-to-service networking.
*   Apply EF Core migrations on startup for all databases.
*   Expose the API Gateway at `http://localhost:5000`.

## 📍 API Endpoints

Once running, you can access the Swagger documentation for any service through the Gateway:

*   **Profile API Docs**: `http://localhost:5000/profile-docs`
*   **Product API Docs**: `http://localhost:5000/product-docs`
*   **Order API Docs**: `http://localhost:5000/order-docs`
*   ...and so on for all services.

### Health Monitoring
Every service exposes a `/health` endpoint for monitoring:
*   Example: `http://localhost:5000/api/Products/health` (via Gateway) or directly on service ports.

## 💾 Data Persistence
Databases are persisted via Docker volumes. SQLite files are stored in the `/app/data` directory within containers to ensure data survives container restarts.

---
**Maintained by Sambhav Tiwari**

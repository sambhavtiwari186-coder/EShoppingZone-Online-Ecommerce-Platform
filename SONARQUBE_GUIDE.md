# SonarQube Integration Guide for EShoppingZone Backend

This guide provides a step-by-step walkthrough to integrate SonarQube for static code analysis in your ASP.NET Core microservices project.

## 1. Prerequisites
- Docker and Docker Compose installed.
- .NET SDK (which you already have).
- Java 17+ (Required by the scanner).
- Internet connection to download Docker images and NuGet packages.

## 2. Infrastructure Setup (SonarQube Server)

We use a dedicated Docker Compose file to run SonarQube and its PostgreSQL database.

**File Created:** `docker-compose.sonarqube.yml`

### Commands to Start SonarQube:
```bash
docker-compose -f docker-compose.sonarqube.yml up -d
```
*(If `docker-compose` is not found, use `docker compose`)*

### Accessing the Dashboard:
1. Open your browser and go to: `http://localhost:9000`
2. **Default Login:**
   - **Username:** `admin`
   - **Password:** `admin`
3. You will be prompted to change the password immediately. Use something secure like `admin123`.

## 3. Scanner Setup

You need the SonarScanner tool for .NET installed globally.

### Installation Command:
```bash
dotnet tool install --global dotnet-sonarscanner
```

### Adding to PATH:
Add the following to your `~/.bash_profile` or `~/.bashrc` to ensure the tool is always available:
```bash
# Add .NET Core SDK tools
export PATH="$PATH:$HOME/.dotnet/tools"
```
Then run `source ~/.bashrc`.

## 4. Running the Analysis

I have created an `analyze.sh` script to automate the process.

### Step 1: Generate a Token
1. Go to SonarQube Dashboard (`http://localhost:9000`).
2. Navigate to **My Account** > **Security**.
3. Generate a new token (e.g., name it "LocalDev").
4. Copy the token.

### Step 2: Run the Script
```bash
./analyze.sh <YOUR_TOKEN>
```

The script performs the following:
1. Starts the scanner "begin" phase with project metadata.
2. Excludes `bin`, `obj`, `Migrations`, and `logs`.
3. Performs a clean build of the solution.
4. Finalizes the scan and uploads results to the server.

## 5. Configuration Details

### Exclusions
The following patterns are excluded to focus only on your business logic:
- `**/bin/**`, `**/obj/**` (Build artifacts)
- `**/Migrations/**` (Entity Framework generated files)
- `**/logs/**` (Runtime logs)
- `**/EShoppingZone-Frontend/**` (Frontend code)

### Solution File
A solution file `EShoppingZone.slnx` has been created to group all microservices for a single unified analysis report.

## 6. Troubleshooting & Common Fixes

### Error: "Java 17 or higher is required"
The SonarScanner for .NET requires Java. On Arch Linux, install it using:
```bash
sudo pacman -S jre17-openjdk
```

### Error: "Max virtual memory areas vm.max_map_count is too low"
SonarQube's Elasticsearch component requires a high `vm.max_map_count`.
**Fix:**
```bash
sudo sysctl -w vm.max_map_count=262144
```
To make it permanent, add `vm.max_map_count=262144` to `/etc/sysctl.d/99-sonarqube.conf`.

### Error: "Database not ready"
If SonarQube fails to start because of the DB, check the logs:
```bash
docker-compose -f docker-compose.sonarqube.yml logs -f
```
Wait a few seconds for PostgreSQL to initialize on the first run.

## 7. File Summary
| File | Purpose |
| :--- | :--- |
| [docker-compose.sonarqube.yml](./docker-compose.sonarqube.yml) | SonarQube infrastructure (Server + DB) |
| [analyze.sh](./analyze.sh) | Bash script to run the .NET analysis |
| [EShoppingZone.slnx](./EShoppingZone.slnx) | Solution file joining all microservices |

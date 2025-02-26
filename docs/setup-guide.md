# Project Setup Guide

This guide outlines the steps to set up, run, and deploy the solution containing multiple projects.

## 📌 Prerequisites

Ensure you have the following installed:
- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download/dotnet)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) (or VS Code with C# extension)
- SQL Server (if using a database)
- Docker (optional, for containerization)
- IIS / Nginx (for deployment)

---

## 🚀 Step 1: Clone the Repository

```sh
git clone <repository-url>
cd <solution-folder>
```

---

## 🔄 Step 2: Restore Dependencies

Run the following command in the **solution root** to restore dependencies for all projects:

```sh
dotnet restore
```

If you need to restore for a specific project:

```sh
dotnet restore Web/Web.csproj
dotnet restore Admin/Admin.csproj
```

---

## 🛠 Step 3: Build the Solution

To build all projects in the solution:

```sh
dotnet build
```

To build in **release mode**:

```sh
dotnet build --configuration Release
```

---

## ⚡ Step 4: Run the Applications

### **Option 1: Run Locally on Different Ports**
Run both MVC applications separately:

```sh
dotnet run --project Web/Web.csproj --urls=http://localhost:5000
dotnet run --project Admin/Admin.csproj --urls=http://localhost:5001
```

- `Web` (Customer-facing app) → [http://localhost:5000](http://localhost:5000)
- `Admin` (Admin Panel) → [http://localhost:5001](http://localhost:5001)

---

## 🏠 Step 5: Database Migration (If applicable)
add migrations

 apply migrations:
```sh
 dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Web
 ```

```sh
 dotnet ef database update --project Infrastructure --startup-project Web
```

stop here
----------------------------------------------------------------------------------------------
---

## 📦 Step 6: Docker Setup (Optional)

If you prefer running the projects inside Docker:

1. Build and run using **Docker Compose**:

```sh
docker-compose up --build
```

2. Access the applications:
    - `Web`: [http://localhost:5000](http://localhost:5000)
    - `Admin`: [http://localhost:5001](http://localhost:5001)

---

## 🌍 Deployment Guide

### **Option 1: Deploy to IIS (Windows Server)**

1. Publish the projects:

```sh
dotnet publish Web/Web.csproj -c Release -o ./publish/Web
dotnet publish Admin/Admin.csproj -c Release -o ./publish/Admin
```

2. Configure IIS:
    - Add two sites in IIS.
    - Bind `Web` to `www.yourdomain.com`
    - Bind `Admin` to `admin.yourdomain.com`

---

### **Option 2: Deploy to Nginx (Linux Server)**

1. Install **Nginx**:

```sh
sudo apt update && sudo apt install nginx
```

2. Configure **Nginx (`/etc/nginx/sites-available/mysite`)**:

```nginx
server {
    listen 80;

    location / {
        proxy_pass http://localhost:5000;
    }

    location /admin {
        proxy_pass http://localhost:5001;
    }
}
```

3. Restart Nginx:

```sh
sudo systemctl restart nginx
```

---

## 🎯 Troubleshooting

### **🔹 Dependencies Not Restoring?**
```sh
dotnet nuget locals all --clear
dotnet restore --force
```

### **🔹 App Crashing on Start?**
Check logs:

```sh
dotnet run --project Web/Web.csproj --verbose
```

---

## ✅ Conclusion

You are now ready to run and deploy the solution! 🚀 If you encounter issues, check the troubleshooting section or consult the team.


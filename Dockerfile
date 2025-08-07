# Stage 1: Build the applications
# We use the .NET SDK image to build all the projects.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all the project files and restore dependencies for each service
# This is done for each of your microservices and the frontend
COPY ["Frontend/Frontend/Frontend.csproj", "Frontend/Frontend/"]
COPY ["Services/AdminService/AdminService.csproj", "Services/AdminService/"]
COPY ["Services/ProjectService/ProjectService.csproj", "Services/ProjectService/"]
COPY ["Services/PhotographyService/PhotographyService.csproj", "Services/PhotographyService/"]
COPY ["Services/VideographyService/VideographyService.csproj", "Services/VideographyService/"]
COPY ["Services/SubscriptionService/SubscriptionService.csproj", "Services/SubscriptionService/"]
COPY ["Services/FirebaseMediaService/FirebaseMediaService.csproj", "Services/FirebaseMediaService/"]
RUN dotnet restore "Frontend/Frontend/Frontend.csproj"
RUN dotnet restore "Services/AdminService/AdminService.csproj"
RUN dotnet restore "Services/ProjectService/ProjectService.csproj"
RUN dotnet restore "Services/PhotographyService/PhotographyService.csproj"
RUN dotnet restore "Services/VideographyService/VideographyService.csproj"
RUN dotnet restore "Services/SubscriptionService/SubscriptionService.csproj"
RUN dotnet restore "Services/FirebaseMediaService/FirebaseMediaService.csproj"

# Copy the rest of the source code
COPY . .

# Publish each application
WORKDIR "/src/Frontend/Frontend"
RUN dotnet publish "Frontend.csproj" -c Release -o /app/publish/frontend

WORKDIR "/src/Services/AdminService"
RUN dotnet publish "AdminService.csproj" -c Release -o /app/publish/adminservice

WORKDIR "/src/Services/ProjectService"
RUN dotnet publish "ProjectService.csproj" -c Release -o /app/publish/projectservice

WORKDIR "/src/Services/PhotographyService"
RUN dotnet publish "PhotographyService.csproj" -c Release -o /app/publish/photographyservice

WORKDIR "/src/Services/VideographyService"
RUN dotnet publish "VideographyService.csproj" -c Release -o /app/publish/videographyservice

WORKDIR "/src/Services/SubscriptionService"
RUN dotnet publish "SubscriptionService.csproj" -c Release -o /app/publish/subscriptionservice

WORKDIR "/src/Services/FirebaseMediaService"
RUN dotnet publish "FirebaseMediaService.csproj" -c Release -o /app/publish/firebasemediaservice


# Stage 2: Create the final runtime images
# We create a separate final image for each service to keep them lightweight.

# --- Frontend Image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS frontend-final
WORKDIR /app
COPY --from=build /app/publish/frontend .
ENTRYPOINT ["dotnet", "Frontend.dll"]

# --- AdminService Image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS adminservice-final
WORKDIR /app
COPY --from=build /app/publish/adminservice .
ENTRYPOINT ["dotnet", "AdminService.dll"]

# --- ProjectService Image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS projectservice-final
WORKDIR /app
COPY --from=build /app/publish/projectservice .
ENTRYPOINT ["dotnet", "ProjectService.dll"]

# --- PhotographyService Image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS photographyservice-final
WORKDIR /app
COPY --from=build /app/publish/photographyservice .
ENTRYPOINT ["dotnet", "PhotographyService.dll"]

# --- VideographyService Image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS videographyservice-final
WORKDIR /app
COPY --from=build /app/publish/videographyservice .
ENTRYPOINT ["dotnet", "VideographyService.dll"]

# --- SubscriptionService Image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS subscriptionservice-final
WORKDIR /app
COPY --from=build /app/publish/subscriptionservice .
ENTRYPOINT ["dotnet", "SubscriptionService.dll"]

# --- FirebaseMediaService Image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS firebasemediaservice-final
WORKDIR /app
COPY --from=build /app/publish/firebasemediaservice .
ENTRYPOINT ["dotnet", "FirebaseMediaService.dll"]

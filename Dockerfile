# Use .NET SDK to build the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# Copy all source code
COPY . .

# Restore & build each service
RUN for project in \
    Services/AdminService/AdminService.csproj \
    Services/ProjectService/ProjectService.csproj \
    Services/PhotographyService/PhotographyService.csproj \
    Services/VideographyService/VideographyService.csproj \
    Services/SubscriptionService/SubscriptionService.csproj \
    Services/FirebaseMediaService/FirebaseMediaService.csproj \
; do dotnet publish $project -c Release -o /app/$(basename $project .csproj); done

# Publish frontend
RUN dotnet publish Frontend/Frontend.csproj -c Release -o /app/Frontend

# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final

WORKDIR /app

# Copy published apps
COPY --from=build /app/AdminService ./AdminService
COPY --from=build /app/ProjectService ./ProjectService
COPY --from=build /app/PhotographyService ./PhotographyService
COPY --from=build /app/VideographyService ./VideographyService
COPY --from=build /app/SubscriptionService ./SubscriptionService
COPY --from=build /app/FirebaseMediaService ./FirebaseMediaService
COPY --from=build /app/Frontend ./Frontend

# Start everything together using a script
COPY start.sh .

RUN chmod +x start.sh

ENTRYPOINT ["./start.sh"]
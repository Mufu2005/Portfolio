#!/bin/bash

# Run all services in background
dotnet ./AdminService/AdminService.dll &
dotnet ./ProjectService/ProjectService.dll &
dotnet ./PhotographyService/PhotographyService.dll &
dotnet ./VideographyService/VideographyService.dll &
dotnet ./SubscriptionService/SubscriptionService.dll &
dotnet ./FirebaseMediaService/FirebaseMediaService.dll &
dotnet ./Frontend/Frontend.dll &

# Wait to keep container alive
wait

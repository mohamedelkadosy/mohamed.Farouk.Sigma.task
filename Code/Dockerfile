FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire solution
COPY . .

# Restore dependencies for all projects
RUN dotnet restore "SigmaCandidateTask/SigmaCandidateTask.API.csproj"

# Build the solution and publish the output
RUN dotnet publish "SigmaCandidateTask/SigmaCandidateTask.API.csproj" -c Release -o /app/publish

# Use the official .NET runtime image for the application itself
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port your app runs on
EXPOSE 80

# Specify the command to run the app
ENTRYPOINT ["dotnet", "SigmaCandidateTask.API.dll"]
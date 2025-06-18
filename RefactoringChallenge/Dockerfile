FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RefactoringChallenge/RefactoringChallenge.csproj", "RefactoringChallenge/"]
COPY ["RefactoringChallenge.Dal/RefactoringChallenge.Dal.csproj", "RefactoringChallenge.Dal/"]
COPY ["RefactoringChallenge.Domain/RefactoringChallenge.Domain.csproj", "RefactoringChallenge.Domain/"]
COPY ["RefactoringChallenge.Services.Abstractions/RefactoringChallenge.Services.Abstractions.csproj", "RefactoringChallenge.Services.Abstractions/"]
COPY ["RefactoringChallenge.Services/RefactoringChallenge.Services.csproj", "RefactoringChallenge.Services/"]
RUN dotnet restore "RefactoringChallenge/RefactoringChallenge.csproj"
COPY . .
WORKDIR "/src/RefactoringChallenge"
RUN dotnet build "RefactoringChallenge.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RefactoringChallenge.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RefactoringChallenge.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Buzile_TicTacToe/Buzile_TicTacToe.csproj", "Buzile_TicTacToe/"]
RUN dotnet restore "Buzile_TicTacToe/Buzile_TicTacToe.csproj"
COPY . .
WORKDIR "/src/Buzile_TicTacToe"
RUN dotnet build "Buzile_TicTacToe.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Buzile_TicTacToe.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Buzile_TicTacToe.dll"]

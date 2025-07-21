FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN dotnet nuget locals all --clear
# Копируем ТОЛЬКО файлы проектов и восстанавливаем зависимости
COPY ["Buzile_TicTacToe/Buzile_TicTacToe.csproj", "Buzile_TicTacToe/"]
COPY ["Buzile_TicTacToe.Tests/Buzile_TicTacToe.Tests/Buzile_TicTacToe.Tests.csproj", "Buzile_TicTacToe.Tests/"]
RUN dotnet restore "Buzile_TicTacToe/Buzile_TicTacToe.csproj" 

# Копируем остальные файлы и собираем
COPY . .
RUN dotnet build "Buzile_TicTacToe/Buzile_TicTacToe.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Buzile_TicTacToe/Buzile_TicTacToe.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Buzile_TicTacToe.dll"]
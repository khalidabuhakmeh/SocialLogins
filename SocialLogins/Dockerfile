FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SocialLogins/SocialLogins.csproj", "SocialLogins/"]
RUN dotnet restore "SocialLogins/SocialLogins.csproj"
COPY . .
WORKDIR "/src/SocialLogins"
RUN dotnet build "SocialLogins.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SocialLogins.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SocialLogins.dll"]

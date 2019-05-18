#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 55937
EXPOSE 44369

FROM microsoft/dotnet:2.2-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Service.Login.API/Service.Login.API.csproj", "Service.Login.API/"]
RUN dotnet restore "Service.Login.API/Service.Login.API.csproj"
COPY . .
WORKDIR "/src/Service.Login.API"
RUN dotnet build "Service.Login.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Service.Login.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Service.Login.API.dll"]
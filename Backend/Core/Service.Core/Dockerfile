#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 55944
EXPOSE 44302

FROM microsoft/dotnet:2.2-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Service.Core/Service.Core.csproj", "Service.Core/"]
RUN dotnet restore "Service.Core/Service.Core.csproj"
COPY . .
WORKDIR "/src/Service.Core"
RUN dotnet build "Service.Core.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Service.Core.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Service.Core.dll"]
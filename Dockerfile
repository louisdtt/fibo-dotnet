FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build 
WORKDIR /src 
COPY . . 
RUN dotnet publish "./src/Fibonacci.Web/Fibonacci.Web.csproj" -c Release -r linux-x64 /p:PublishSingleFile=true -o /publish 

FROM mcr.microsoft.com/dotnet/runtime-deps:6.0 AS final 
WORKDIR /app 
COPY --from=build /publish . 
ENTRYPOINT ["/app/Fibonacci.Web"]

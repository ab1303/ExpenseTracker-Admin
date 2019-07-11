#Builder image
FROM microsoft/dotnet:2.2-sdk AS builder
WORKDIR /src

COPY ./*.sln ./

# Copy the project files
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

RUN dotnet restore

# Copy across the rest of the source files
COPY . .

RUN dotnet build -c Release --no-restore

RUN dotnet publish /src/PartnerUser.Api/PartnerUser.Api.csproj -c Release -o /app --no-restore

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "PartnerUser.Api.dll"]
COPY --from=builder /app .

FROM microsoft/dotnet:2.1.401-sdk-alpine AS build
WORKDIR .
COPY . .
WORKDIR /src/WebUI

RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/
WORKDIR /app
RUN ls -an

FROM nginx:alpine
WORKDIR /app
COPY --from=publish /app/WebUI/dist /usr/share/nginx/html/
WORKDIR /usr/share/nginx/html/
RUN ls -an
COPY --from=build /src/WebUI/config/nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /src/WebUI/config/nginx/mime.types /etc/nginx/mime.types
RUN chmod 0644 /etc/nginx/mime.types
RUN chmod 0644 /etc/nginx/nginx.conf

CMD ["nginx", "-g", "daemon off;"]

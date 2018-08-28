@echo off

rem run cart
start "Cart servies" dotnet run --project src\services\cart

rem run idp
start "idp servies" dotnet run --project src\services\idp

rem run inventory
start "inventory servies" dotnet run --project src\services\inventory

rem run pricing
start "pricing servies" dotnet run --project src\services\pricing

rem run review
start "review servies" dotnet run --project src\services\review

rem run catalog
start "catalog servies" npm start --prefix src\services\catalog

rem run rating
start "rating servies" npm start --prefix src\services\rating

rem run web
start "spa web" npm run dev --prefix src\web
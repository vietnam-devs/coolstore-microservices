# Require !!!
- Because we use `sqlx macros` so we have to put the `.env` with value `DATABASE_URL=postgres://postgres:P@ssw0rd@localhost:5432/postgres` at the root folder of `rust`
  - If not, then you cannot compile the solution :|

# Get starting (for both on vscode remote container and on local)

- Option 1: https://github.com/microsoft/vscode-dev-containers/tree/main/containers/rust-postgres
  - Then you only need remote containers to get starting this project (press F1, and type `remote container`, then open it up)
  - Generating the database schema and data
    Make sure you have `.env` file in the root folder, then add the variable environment as `DATABASE_URL` for it

    ```bash
    DATABASE_URL=postgres://postgres:P@ssw0rd@localhost:5432/inv_db <= the inv_db is the database you want to create it
    ``` 
    Then, we do as below
    ```bash
    > # for example we want to generate schema and seed data for inventory
    > ./sqlx_migrate.sh crates/inventory
    ```
  - Offline for Docker: follow the guidance at https://github.com/launchbadge/sqlx/issues/1223
    - At root, run `sqlx.sh`, and wait for the sqlx-data.json generating
  - Below is just for reference (DON'T WORK):
    - Output offline data (deploy into docker)
      ```bash
      > # inventory
      > SQLX_OFFLINE=true && cargo sqlx prepare --database-url postgres://postgres:P@ssw0rd@127.0.0.1:5432/inv_db -- --bin inventory_api
      > # product catalog
      > SQLX_OFFLINE=true && cargo sqlx prepare --database-url postgres://postgres:P@ssw0rd@127.0.0.1:5432/postgres -- --bin product_catalog_api
      ```
- Option 2: run on your own host with https://rustup.rs/

# Add database migration

- Make sure you have sqlx cli installed
- `cd` into project folder, such as `cd crates/inventory/src`
- Then run `sqlx migrate add -r <your migration name>`

# Credits:
- https://github.com/launchbadge/realworld-axum-sqlx
- https://github.com/thangchung/bff-auth/tree/main/backend/product-api
- https://github.com/thangchung/northwind-rs
- https://github.com/Z4RX/axum_jwt_example
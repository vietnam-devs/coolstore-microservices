# Starting (for both on vscode remote container and on local)

- https://github.com/microsoft/vscode-dev-containers/tree/main/containers/rust-postgres
  - Then you only need remote containers to get starting this project (press F1, and type `remote container`, then open it up)
  - Offline for Docker:
    - Output offline data (deploy into docker)
      ```bash
      > # inventory
      > SQLX_OFFLINE=true && cargo sqlx prepare --database-url postgres://postgres:P@ssw0rd@127.0.0.1:5432/postgres -- --manifest-path inventory/Cargo.toml --bin inventory_api
      > # product catalog
      > SQLX_OFFLINE=true && cargo sqlx prepare --database-url postgres://postgres:P@ssw0rd@127.0.0.1:5432/postgres -- --manifest-path inventory/Cargo.toml --bin product_catalog_api
      ```
- Or run on your own host with https://rustup.rs/

# Require !!!
- Because we use `sqlx macros` so we have to put the `.env` with value `DATABASE_URL=postgres://postgres:P@ssw0rd@localhost:5432/postgres` at the root folder of `rust`
  - If not, then you cannot compile the solution :|

# Database migration

- Make sure you have sqlx cli installed
- `cd` into project folder, such as `cd crates/inventory/src`
- Then run `sqlx migrate add -r <your migration name>`

# Credits:
- https://github.com/launchbadge/realworld-axum-sqlx
- https://github.com/thangchung/bff-auth/tree/main/backend/product-api
- https://github.com/thangchung/northwind-rs
- https://github.com/Z4RX/axum_jwt_example
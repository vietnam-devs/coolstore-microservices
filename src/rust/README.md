# Starting

- https://github.com/microsoft/vscode-dev-containers/tree/main/containers/rust-postgres
  - Then you only need remote containers to get starting this project (press F1, and type `remote container`, then open it up)
  - Inside the `devcontainer`, you need to install `sqlx-cli` by using `cargo install sqlx-cli --no-default-features --features native-tls,postgres`
    - Output offline data (deploy into docker)
      ```bash
      > # inventory
      > SQLX_OFFLINE=true && cargo sqlx prepare --database-url postgres://postgres:P@ssw0rd@127.0.0.1:5432/test -- --manifest-path inventory/Cargo.toml --bin server
      > # product catalog
      > SQLX_OFFLINE=true && cargo sqlx prepare --database-url postgres://postgres:P@ssw0rd@127.0.0.1:5432/test -- --manifest-path inventory/Cargo.toml --bin product_catalog_api
      ```
- Or https://rustup.rs/
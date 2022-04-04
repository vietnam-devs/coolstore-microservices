use std::net::SocketAddr;

use clap::StructOpt;
use common::config;
use common::config::db::DbPool;
use common::logs::PrintlnDrain;
use slog::{info, o, Fuse, Logger};
use tracing_subscriber::EnvFilter;

#[tokio::main]
async fn main() {
    let log = Logger::root(Fuse(PrintlnDrain), o!("slog" => true));

    dotenv::dotenv().ok();

    tracing_subscriber::fmt()
        .with_env_filter(EnvFilter::from_default_env())
        .pretty()
        .init();

    let pg_config = config::env::PgConfig::parse();
    let server_config = config::env::ServerConfig::parse();

    let pg_pool = sqlx::PgPool::retrieve(pg_config.pg_product_catalog_database.as_str()).await;
    sqlx::migrate!("crates/product_catalog/migrations")
        .run(&pg_pool)
        .await
        .expect("cannot do migrate");

    let addr = SocketAddr::from((
        server_config.product_catalog_host,
        server_config.product_catalog_port,
    ));
    tracing::debug!("listening on {}", addr);

    info!(log, "listening on {addr}", addr = addr);

    let server = axum::Server::bind(&addr)
        .serve(product_catalog::app(server_config, pg_config, pg_pool, log).into_make_service());

    if let Err(err) = server.await {
        tracing::error!("server error: {}", err);
    }
}

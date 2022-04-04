use std::net::SocketAddr;

use clap::StructOpt;
use common::config;
use common::config::db::DbPool;
use common::logs::PrintlnDrain;
use slog::{info, o, Fuse, Logger};
use tracing_subscriber::{layer::SubscriberExt, util::SubscriberInitExt};

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    dotenv::dotenv().ok();

    let log = Logger::root(Fuse(PrintlnDrain), o!("slog" => true));

    tracing_subscriber::registry()
        .with(tracing_subscriber::EnvFilter::new(
            std::env::var("RUST_LOG").unwrap_or_else(|_| "inventory=debug,tower_http=debug".into()),
        ))
        .with(tracing_subscriber::fmt::layer())
        .init();

    let pg_config = config::env::PgConfig::parse();
    let server_config = config::env::ServerConfig::parse();

    let pg_pool = sqlx::PgPool::retrieve(pg_config.pg_inventory_database.as_str()).await;
    sqlx::migrate!("crates/inventory/migrations")
        .run(&pg_pool)
        .await
        .expect("cannot do migrate");

    let addr = SocketAddr::from((server_config.inventory_host, server_config.inventory_port));
    tracing::debug!("listening on {}", addr);

    info!(log, "listening on {addr}", addr = addr);

    let server = axum::Server::bind(&addr)
        .serve(inventory::app(server_config, pg_config, pg_pool, log).into_make_service());

    if let Err(err) = server.await {
        tracing::error!("server error: {}", err);
    }

    Ok(())
}

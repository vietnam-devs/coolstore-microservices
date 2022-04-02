use std::sync::Arc;

use axum::{routing::post, Extension, Router};
use common::{
    config::env::{PgConfig, ServerConfig},
    ApiContext,
};
use slog::Logger;
use sqlx::PgPool;
use tower::ServiceBuilder;
use tower_http::{cors::CorsLayer, trace::TraceLayer, ServiceBuilderExt};

mod handlers;

pub fn app(
    server_config: ServerConfig,
    pg_config: PgConfig,
    pg_pool: PgPool,
    log: Logger,
) -> Router {
    let middleware = ServiceBuilder::new()
        .layer(TraceLayer::new_for_http())
        .compression()
        .layer(CorsLayer::permissive())
        .layer(Extension(ApiContext {
            server_config: Arc::new(server_config),
            pg_config: Arc::new(pg_config),
            pg_pool,
            log,
        }))
        .into_inner();

    Router::new()
        .route("/get-inventory-by-id", post(handlers::post_inventory_by_id))
        .route(
            "/get-inventories-by-ids",
            post(handlers::post_inventories_by_ids),
        )
        .layer(middleware)
}

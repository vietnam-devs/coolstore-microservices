use axum::{routing::{post}, AddExtensionLayer, Router};
use slog::Logger;
use sqlx::PgPool;
use tower::ServiceBuilder;
use tower_http::{cors::CorsLayer, trace::TraceLayer, ServiceBuilderExt};

pub mod config;
mod handlers;
pub mod logs;

pub fn app(pg_pool: PgPool, log: Logger) -> Router {
    let middleware = ServiceBuilder::new()
        .layer(TraceLayer::new_for_http())
        .compression()
        .layer(CorsLayer::permissive())
        .layer(AddExtensionLayer::new(log.clone()))
        .layer(AddExtensionLayer::new(pg_pool))
        .into_inner();

    Router::new()
        .route("/get-inventory-by-id", post(handlers::post_inventory_by_id))
        .route("/get-inventories-by-ids", post(handlers::post_inventories_by_ids))
        .layer(middleware)
}

use std::sync::Arc;

use axum::{routing::get, routing::post, Extension, Router};
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
        .route(
            "/api/products/search/:price/:page/:page_size",
            get(handlers::get_product_search),
        )
        .route(
            "/api/products/by-page-and-price/:page/:price",
            get(handlers::get_products_by_page_price),
        )
        .route(
            "/api/products/:id",
            get(handlers::get_product_of_specific_product_query),
        )
        .route("/get-product-by-id", post(handlers::post_product_by_id))
        .layer(middleware)
}

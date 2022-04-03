use axum::extract::Extension;
use common::ApiContext;
use slog::info;
use uuid::Uuid;

pub async fn get_product_search(ctx: Extension<ApiContext>) -> &'static str {
    info!(ctx.log, "GET: get_product_search");

    "GET: get_product_search"
}

pub async fn get_products_by_page_price(ctx: Extension<ApiContext>) -> &'static str {
    info!(ctx.log, "GET: post_products_by_page_price");

    "GET: post_products_by_page_price"
}

pub async fn get_product_of_specific_product_query(ctx: Extension<ApiContext>) -> &'static str {
    info!(ctx.log, "GET: get_product_of_specific_product_query");

    "GET: get_product_of_specific_product_query"
}

pub async fn post_product_by_id(ctx: Extension<ApiContext>) -> &'static str {
    info!(ctx.log, "POST: post_product_by_id");

    "POST: post_product_by_id"
}

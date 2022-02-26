use axum::extract::Extension;
use slog::Logger;
use sqlx::PgPool;

pub async fn post_inventory_by_id(
    Extension(pool): Extension<PgPool>,
    Extension(log): Extension<Logger>,
) -> &'static str {
    //info!(log, "post post_inventory_by_id");

    "post post_inventory_by_id"
}

pub async fn post_inventories_by_ids(
    Extension(pool): Extension<PgPool>,
    Extension(log): Extension<Logger>,
) -> &'static str {
    //info!(log, "post post_inventories_by_ids");

    "post post_inventories_by_ids"
}

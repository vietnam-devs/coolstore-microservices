use axum::extract::Extension;
use common::ApiContext;
use slog::info;

pub async fn post_inventory_by_id(
    ctx: Extension<ApiContext>,
) -> common::Result<axum::Json<i32>> {
    info!(ctx.log, "POST: inventory_by_id");

    let result = sqlx::query!(r#"SELECT $1::INTEGER AS value"#, 2i32)
        .fetch_one(&ctx.pg_pool)
        .await?;

    match result.value {
        Some(value) => Ok(axum::Json(value)),
        None => Err(common::error::Error::NotFound)
    }
}

pub async fn post_inventories_by_ids(
    ctx: Extension<ApiContext>,
) -> &'static str {
    info!(ctx.log, "POST: inventories_by_ids");

    "POST: inventories_by_ids"
}
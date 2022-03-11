use axum::extract::Extension;
use hyper::StatusCode;
use slog::{info, Logger};
use sqlx::PgPool;

pub async fn post_inventory_by_id(
    Extension(pool): Extension<PgPool>,
    Extension(log): Extension<Logger>,
) -> Result<String, (StatusCode, String)> {
    info!(log, "post post_inventory_by_id");
    // "post post_inventory_by_id"

    let result = sqlx::query!("SELECT $1::INTEGER AS value", 2i32)
        .fetch_one(&pool)
        .await;

    print!("{:?}", result);
    //assert_eq!(result.value, Some(2));

    sqlx::query_scalar("select 'hello world from pg'")
        .fetch_one(&pool)
        .await
        .map_err(internal_error)
}

pub async fn post_inventories_by_ids(
    Extension(pool): Extension<PgPool>,
    Extension(log): Extension<Logger>,
) -> &'static str {
    //info!(log, "post post_inventories_by_ids");

    "post post_inventories_by_ids"
}

fn internal_error<E>(err: E) -> (StatusCode, String)
where
    E: std::error::Error,
{
    (StatusCode::INTERNAL_SERVER_ERROR, err.to_string())
}

use axum::extract::Extension;
use common::ApiContext;
use slog::info;
use uuid::Uuid;

#[derive(serde::Deserialize)]
pub struct InventoryRequest {
    pub id: String,
}

#[derive(serde::Deserialize, serde::Serialize)]
pub struct InventoryDto {
    pub id: Uuid,
    pub location: String,
    pub description: String,
    pub website: String,
    pub created: sqlx::types::chrono::NaiveDateTime,
    pub updated: Option<sqlx::types::chrono::NaiveDateTime>,
}

#[derive(serde::Deserialize)]
pub struct InventoriesRequest {
    pub ids: String,
}

pub async fn post_inventory_by_id(
    ctx: Extension<ApiContext>,
    axum::Json(req): axum::Json<InventoryRequest>,
) -> common::Result<axum::Json<InventoryDto>> {
    info!(ctx.log, "POST: inventory_by_id");

    // validate id
    match Uuid::parse_str(req.id.as_str()) {
        Ok(id) => {
            let mut tx = ctx.pg_pool.begin().await?;

            let result = sqlx::query!(
                r#"
                SELECT id, location, description, website, created, updated
                FROM inventory.inventories 
                WHERE id = $1"#,
                id
            )
            .fetch_optional(&mut tx)
            .await?;

            tx.commit().await?;

            match result {
                Some(value) => Ok(axum::Json(InventoryDto {
                    id: value.id,
                    location: value.location,
                    website: value.website,
                    description: value.description,
                    created: value.created,
                    updated: value.updated,
                })),
                None => Err(common::error::Error::NotFound),
            }
        }
        Err(_) => Err(common::error::Error::ValidateFailed),
    }
}

pub async fn post_inventories_by_ids(
    ctx: Extension<ApiContext>,
    axum::Json(req): axum::Json<InventoriesRequest>,
) -> common::Result<axum::Json<Vec<InventoryDto>>> {
    info!(ctx.log, "POST: inventories_by_ids");

    let ids = req.ids.as_str().split(",");
    let mut vec: Vec<Uuid> = vec![];

    // validate ids
    for id in ids {
        let temp = Uuid::parse_str(id).unwrap();
        vec.push(temp);
    }

    // https://github.com/launchbadge/sqlx/blob/master/FAQ.md#how-can-i-do-a-select--where-foo-in--query
    let result = sqlx::query!(
        "SELECT id, location, description, website, created, updated
        FROM inventory.inventories 
        WHERE id = ANY($1)",
        // a bug of the parameter typechecking code requires all array parameters to be slices
        &vec
    )
    .fetch_all(&ctx.pg_pool)
    .await?;

    let mut output = vec![];
    for inv in result {
        output.push(InventoryDto {
            id: inv.id,
            location: inv.location,
            website: inv.website,
            description: inv.description,
            created: inv.created,
            updated: inv.updated,
        })
    }

    Ok(axum::Json(output))
}

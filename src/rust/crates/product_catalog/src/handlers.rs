use axum::extract::Extension;
use common::ApiContext;
use slog::info;
use uuid::Uuid;

#[derive(serde::Deserialize, serde::Serialize)]
#[serde(rename_all = "camelCase")]
pub struct ProductDto {
    pub id: Uuid,
    pub name: String,
    pub price: f64,
    pub image_url: String,
    pub description: String,
    pub category_id: Uuid,
    pub category_name: String,
    pub inventory_id: Uuid,
    pub total: Option<i64>,
}

#[derive(serde::Deserialize, serde::Serialize, Default)]
#[serde(rename_all = "camelCase")]
pub struct ProductDetailDto {
    pub id: Uuid,
    pub name: String,
    pub price: f64,
    pub image_url: String,
    pub description: String,
    pub category_id: Uuid,
    pub category_name: String,
    pub inventory_id: Uuid,
    pub inventory_location: String,
}

#[derive(serde::Deserialize, serde::Serialize)]
#[serde(rename_all = "camelCase")]
pub struct SearchResult {
    pub total: i64,
    pub page: i64,
    pub results: Vec<ProductDto>,
    pub category_tags: Vec<KeyCountPair>,
    pub inventory_tags: Vec<KeyCountPair>,
}

#[derive(serde::Deserialize, serde::Serialize, Clone)]
pub struct KeyCountPair {
    pub key: Uuid,
    pub text: String,
    pub count: i32,
}

#[derive(serde::Deserialize, serde::Serialize, Default)]
pub struct InventoryDto {
    pub id: Uuid,
    pub location: String,
    pub description: String,
    pub website: String,
}

#[derive(serde::Deserialize)]
pub struct GetProductByIdRequest {
    pub id: Uuid,
}

async fn get_products(
    pool: sqlx::Pool<sqlx::Postgres>,
    price: f64,
    page: i64,
    page_size: i64,
) -> common::Result<Vec<ProductDto>> {
    sqlx::query_as!(ProductDto,
        r#"SELECT p.id, p.name, p.price, p.image_url, p.description, p.category_id, c.name category_name, p.inventory_id, count(*) OVER() AS total
        FROM catalog.categories c 
            INNER JOIN catalog.products p ON c.id = p.category_id
        WHERE p.is_deleted = false AND p.price <= $1
        ORDER BY p.name
        OFFSET $2
        LIMIT $3"#,
        price, page, page_size)
    .fetch_all(&pool)
    .await
    .map_err(|e| -> common::error::Error{common::error::Error::Sqlx(e)})
}

async fn get_inventory_by_id(inventory_uri: &str, id: Uuid) -> common::Result<InventoryDto> {
    let resp = reqwest::Client::new()
        .post(format!("{}/get-inventory-by-id", inventory_uri))
        .json(&serde_json::json!({
            "id": id,
        }))
        .send()
        .await
        .unwrap();

    let mut result: InventoryDto = InventoryDto::default();
    if let reqwest::StatusCode::OK = resp.status() {
        if let Ok(parsed) = resp.json::<InventoryDto>().await {
            result = InventoryDto {
                id: parsed.id,
                location: parsed.location,
                website: parsed.website,
                description: parsed.description,
            }
        }
    }

    Ok(result)
}

async fn get_inventory_by_ids(inventory_uri: &str, ids: &str) -> common::Result<Vec<InventoryDto>> {
    let resp = reqwest::Client::new()
        .post(format!("{}/get-inventories-by-ids", inventory_uri))
        .json(&serde_json::json!({
            "ids": ids,
        }))
        .send()
        .await
        .unwrap();

    let mut result: Vec<InventoryDto> = vec![];
    if resp.status() == reqwest::StatusCode::OK {
        if let Ok(parsed) = resp.json::<Vec<InventoryDto>>().await {
            parsed.into_iter().for_each(|inv| {
                result.push(InventoryDto {
                    id: inv.id,
                    location: inv.location,
                    website: inv.website,
                    description: inv.description,
                });
            });
        }
    }

    Ok(result)
}

pub async fn get_product_search(
    ctx: Extension<ApiContext>,
    axum::extract::Path((price, page, page_size)): axum::extract::Path<(f64, i64, i64)>,
) -> common::Result<axum::Json<SearchResult>> {
    info!(ctx.log, "GET: get_product_search");

    let result = get_products(ctx.pg_pool.clone(), price, page, page_size).await?;

    let mut category_tags: Vec<KeyCountPair> = vec![];
    let mut inventory_tags: Vec<KeyCountPair> = vec![];
    let mut total: i64 = -1;

    // todo: remove hard-code for ids
    let inventories_result = get_inventory_by_ids(
        ctx.server_config.inventory_client_uri.as_str(),
    "90c9479e-a11c-4d6d-aaaa-0405b6c0efcd,b8b62196-6369-409d-b709-11c112dd023f,ec186ddf-f430-44ec-84e5-205c93d84f14").await?;
    for inv in inventories_result {
        inventory_tags.push(KeyCountPair {
            key: inv.id,
            text: inv.location,
            count: 0,
        })
    }

    let mut output = vec![];
    for product in result {
        if category_tags.iter().any(|i| i.key == product.category_id) {
            let index = category_tags
                .clone()
                .into_iter()
                .position(|x| x.key == product.category_id)
                .unwrap();
            category_tags[index].count += 1;
        } else {
            category_tags.push(KeyCountPair {
                key: product.category_id,
                text: product.category_name.clone(),
                count: 1,
            });
        }

        if inventory_tags.iter().any(|i| i.key == product.inventory_id) {
            let index = inventory_tags
                .clone()
                .into_iter()
                .position(|x| x.key == product.inventory_id)
                .unwrap();
            inventory_tags[index].count += 1;
        }

        if total <= 0 {
            total = if product.total < Some(0) {
                0
            } else {
                product.total.unwrap()
            };
        }

        output.push(ProductDto {
            id: product.id,
            name: product.name,
            price: product.price,
            image_url: product.image_url,
            description: product.description,
            category_id: product.category_id,
            category_name: product.category_name,
            inventory_id: product.inventory_id,
            total: product.total,
        });
    }

    Ok(axum::Json(SearchResult {
        total,
        page,
        results: output,
        category_tags,
        inventory_tags,
    }))
}

pub async fn get_products_by_page_price(
    ctx: Extension<ApiContext>,
    axum::extract::Path((page, price)): axum::extract::Path<(i64, f64)>,
) -> common::Result<axum::Json<Vec<ProductDetailDto>>> {
    info!(ctx.log, "GET: get_products_by_page_price");
    let result = get_products(ctx.pg_pool.clone(), price, page, 10).await?;
    let mut output = vec![];
    for product in result {
        let inv = get_inventory_by_id(
            ctx.server_config.inventory_client_uri.as_str(),
            product.inventory_id,
        )
        .await?;
        output.push(ProductDetailDto {
            id: product.id,
            name: product.name,
            price: product.price,
            image_url: product.image_url,
            description: product.description,
            category_id: product.category_id,
            category_name: product.category_name,
            inventory_id: product.inventory_id,
            inventory_location: inv.location,
        });
    }

    Ok(axum::Json(output))
}

pub async fn get_product_of_specific_product_query(
    ctx: Extension<ApiContext>,
    axum::extract::Path(id): axum::extract::Path<Uuid>,
) -> common::Result<axum::Json<ProductDetailDto>> {
    info!(ctx.log, "GET: get_product_of_specific_product_query");

    let result = sqlx::query!(
        r#"
        SELECT p.id, p.name, p.price, p.image_url, p.description, p.category_id, c.name category_name, p.inventory_id
        FROM catalog.categories c 
            INNER JOIN catalog.products p ON c.id = p.category_id
        WHERE p.is_deleted = false AND p.id = $1
        "#,
        id
    )
    .fetch_optional(&ctx.pg_pool)
    .await?;

    let mut return_result: ProductDetailDto = ProductDetailDto::default();
    if let Some(product) = result {
        let inv = get_inventory_by_id(
            ctx.server_config.inventory_client_uri.as_str(),
            product.inventory_id,
        )
        .await?;
        return_result = ProductDetailDto {
            id: product.id,
            name: product.name,
            price: product.price,
            image_url: product.image_url,
            description: product.description,
            category_id: product.category_id,
            category_name: product.category_name,
            inventory_id: product.inventory_id,
            inventory_location: inv.location,
        }
    }

    Ok(axum::Json(return_result))
}

pub async fn post_product_by_id(
    ctx: Extension<ApiContext>,
    axum::Json(req): axum::Json<GetProductByIdRequest>,
) -> common::Result<axum::Json<ProductDetailDto>> {
    info!(ctx.log, "POST: post_product_by_id");

    let result = sqlx::query!(
        r#"
        SELECT p.id, p.name, p.price, p.image_url, p.description, p.category_id, c.name category_name, p.inventory_id
        FROM catalog.categories c 
            INNER JOIN catalog.products p ON c.id = p.category_id
        WHERE p.is_deleted = false AND p.id = $1
        "#,
        req.id
    )
    .fetch_optional(&ctx.pg_pool)
    .await?;

    let mut return_result: ProductDetailDto = ProductDetailDto::default();
    if let Some(product) = result {
        let inv = get_inventory_by_id(
            ctx.server_config.inventory_client_uri.as_str(),
            product.inventory_id,
        )
        .await?;
        return_result = ProductDetailDto {
            id: product.id,
            name: product.name,
            price: product.price,
            image_url: product.image_url,
            description: product.description,
            category_id: product.category_id,
            category_name: product.category_name,
            inventory_id: product.inventory_id,
            inventory_location: inv.location,
        }
    }

    Ok(axum::Json(return_result))
}

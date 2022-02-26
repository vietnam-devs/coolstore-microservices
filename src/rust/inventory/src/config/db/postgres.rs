use axum::async_trait;
use clap::Parser;
use sqlx::{postgres::PgPoolOptions, PgPool};

use crate::config::{db::DbPool, env::PgConfig};

#[async_trait]
impl DbPool for PgPool {
    async fn retrieve() -> Self {
        let config = PgConfig::parse();
        let uri = format!(
            "postgres://{}:{}@{}:{}/{}",
            config.pg_user, config.pg_password, config.pg_host, config.pg_port, config.pg_database
        );
        PgPoolOptions::new()
            .connect(&uri)
            .await
            .expect("DB connection was failed")
    }
}

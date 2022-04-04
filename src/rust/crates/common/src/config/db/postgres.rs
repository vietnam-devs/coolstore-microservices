use axum::async_trait;
use clap::Parser;
use sqlx::{postgres::PgPoolOptions, PgPool};

use crate::config::{db::DbPool, env::PgConfig};

#[async_trait]
impl DbPool for PgPool {
    async fn retrieve(pg_database: &str) -> Self {
        let config = PgConfig::parse();
        let uri = format!(
            "postgres://{}:{}@{}:{}/{}",
            config.pg_user, config.pg_password, config.pg_host, config.pg_port, pg_database
        );
        println!("connection string: {}", uri);
        PgPoolOptions::new()
            .connect(&uri)
            .await
            .expect("DB connection was failed")
    }
}

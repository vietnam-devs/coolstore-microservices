use std::sync::Arc;

use config::env::{PgConfig, ServerConfig};
use slog::Logger;
use sqlx::PgPool;

pub mod logs;
pub mod config;
pub mod error;

pub type Result<T, E = error::Error> = std::result::Result<T, E>;

#[derive(Clone)]
pub struct ApiContext {
    pub server_config: Arc<ServerConfig>,
    pub pg_config: Arc<PgConfig>,
    pub log: Logger,
    pub pg_pool: PgPool,
}
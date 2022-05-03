use clap::Parser;
use std::{net::IpAddr};

#[derive(Debug, Parser)]
pub struct ServerConfig {
    #[clap(default_value = "127.0.0.1", env)]
    pub inventory_host: IpAddr,

    #[clap(default_value = "5002", env)]
    pub inventory_port: u16,

    #[clap(default_value = "127.0.0.1", env)]
    pub product_catalog_host: IpAddr,

    #[clap(default_value = "5003", env)]
    pub product_catalog_port: u16,

    #[clap(default_value = "http://localhost:5002", env)]
    pub inventory_client_uri: String,
}

#[derive(Debug, Parser)]
pub struct PgConfig {
  #[clap(default_value = "inv_db", env)]
  pub pg_inventory_database: String,

  #[clap(default_value = "cat_db", env)]
  pub pg_product_catalog_database: String,

  #[clap(default_value = "127.0.0.1", env)]
  pub pg_host: String,

  #[clap(default_value = "5432", env)]
  pub pg_port: u16,

  #[clap(default_value = "postgres", env)]
  pub pg_user: String,

  #[clap(default_value = "P@ssw0rd", env)]
  pub pg_password: String
}

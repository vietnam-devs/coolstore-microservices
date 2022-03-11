use clap::Parser;
use std::{env, net::IpAddr};

#[derive(Debug, Parser)]
pub struct ServerConfig {
    #[clap(default_value = "127.0.0.1", env)]
    pub host: IpAddr,

    #[clap(default_value = "5002", env)]
    pub port: u16,
}

#[derive(Debug, Parser)]
pub struct PgConfig {
  #[clap(default_value = "postgres", env)]
  pub pg_database: String,

  #[clap(default_value = "127.0.0.1", env)]
  pub pg_host: String,

  #[clap(default_value = "5432", env)]
  pub pg_port: u16,

  #[clap(default_value = "postgres", env)]
  pub pg_user: String,

  #[clap(default_value = "P@ssw0rd", env)]
  pub pg_password: String
}

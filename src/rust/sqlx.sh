#!/usr/bin/env bash
rm -rf target/sqlx
touch crates/*/src/*.rs
env -u DATABASE_URL SQLX_OFFLINE=false cargo check --workspace
jq -s '{"db": "PostgreSQL"} + INDEX(.hash)' target/sqlx/query-*.json > sqlx-data.json
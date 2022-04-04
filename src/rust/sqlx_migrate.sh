#!/usr/bin/env bash
sqlx database create
cd $1
sqlx migrate run
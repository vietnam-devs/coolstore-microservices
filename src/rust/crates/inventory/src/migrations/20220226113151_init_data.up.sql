-- Add up migration script here
START TRANSACTION;

CREATE SCHEMA IF NOT EXISTS inventory;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE inventory.inventories (
    id uuid NOT NULL DEFAULT (uuid_generate_v4()),
    location text NOT NULL,
    description text NOT NULL,
    website text NOT NULL,
    created timestamp without time zone NOT NULL DEFAULT (now()),
    updated timestamp without time zone NULL,
    CONSTRAINT pk_inventories PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_inventories_id ON inventory.inventories (id);

COMMIT;
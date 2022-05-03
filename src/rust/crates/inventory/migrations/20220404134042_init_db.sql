-- Add migration script here

-- Add up migration script here
START TRANSACTION;

CREATE SCHEMA IF NOT EXISTS inventory;

-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE inventory.inventories (
    -- id uuid NOT NULL DEFAULT (uuid_generate_v4()),
    id uuid NOT NULL DEFAULT (md5(random()::text || clock_timestamp()::text)::uuid),
    location text NOT NULL,
    description text NOT NULL,
    website text NOT NULL,
    created timestamp without time zone NOT NULL DEFAULT (now()),
    updated timestamp without time zone NULL,
    CONSTRAINT pk_inventories PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_inventories_id ON inventory.inventories (id);

COMMIT;

START TRANSACTION;

INSERT INTO inventory.inventories (id,"location",description,website,created,updated) VALUES
('90c9479e-a11c-4d6d-aaaa-0405b6c0efcd','Vietnam','This store sells electronic gadgets','https://coolstore-vn.com','2020-10-31 15:34:05.144',NULL)
,('b8b62196-6369-409d-b709-11c112dd023f','Seattle','This store sells food and beverage products','https://coolstore-sea.com','2020-10-31 15:35:20.440',NULL)
,('ec186ddf-f430-44ec-84e5-205c93d84f14','San Francisco','This store sells food and beverage products','https://coolstore-san.com','2020-10-31 15:35:50.287',NULL)
;


COMMIT;

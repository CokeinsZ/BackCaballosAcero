﻿CREATE TABLE Roles
(
    id   SERIAL PRIMARY KEY,
    name VARCHAR(32)
);

CREATE TYPE user_status AS ENUM ('Active', 'Not Verified', 'Inactive', 'Banned');
CREATE TYPE card_status AS ENUM ('Active', 'Inactive');
CREATE TYPE post_status AS ENUM ('Available', 'SoldOut');
CREATE TYPE motoInventory_status AS ENUM ('Available', 'Sold', 'Under Customization', 'Ready', 'Delivered');
CREATE TYPE card_type AS ENUM ('Credit', 'Debit');
CREATE TYPE payment_method AS ENUM ('Cash', 'Card');

CREATE TABLE Users
(
    id                      SERIAL PRIMARY KEY,
    name                    VARCHAR(32)         NOT NULL,
    last_name               VARCHAR(32)         NOT NULL,
    identification_document VARCHAR(16) UNIQUE  NOT NULL,
    country                 VARCHAR(32)         NOT NULL DEFAULT 'Colombia',
    city                    VARCHAR(32)         NOT NULL,
    address                 TEXT,
    password                VARCHAR(255)        NOT NULL,
    email                   VARCHAR(255) UNIQUE NOT NULL,
    phone_number            VARCHAR(16) UNIQUE,
    status                  user_status DEFAULT 'Not Verified',
    role_id                 INT DEFAULT 1,
    created_at              TIMESTAMP                    DEFAULT CURRENT_TIMESTAMP,
    updated_at              TIMESTAMP                    DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (role_id) REFERENCES Roles (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE Card
(
    id              SERIAL PRIMARY KEY,
    user_id         INT,
    owner           VARCHAR(32) NOT NULL,
    pan             VARCHAR(255) NOT NULL,
    cvv             VARCHAR(255) NOT NULL,
    type            card_type NOT NULL,
    expiration_date VARCHAR(255) NOT NULL,
    status          card_status DEFAULT 'Active',

    FOREIGN KEY (user_id) REFERENCES Users (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE Security_Codes
(
    id         SERIAL PRIMARY KEY,
    user_id    INT UNIQUE NOT NULL,
    code       VARCHAR(6) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    expires_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP + INTERVAL '5 minutes',

    FOREIGN KEY (user_id) REFERENCES Users (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE Refresh_Tokens
(
    id      SERIAL PRIMARY KEY,
    user_id INT  NOT NULL UNIQUE,
    token   TEXT NOT NULL,

    FOREIGN KEY (user_id) REFERENCES Users (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE Branches
(
    id      SERIAL PRIMARY KEY,
    nit     VARCHAR(10) NOT NULL UNIQUE,
    name    VARCHAR(32) NOT NULL,
    country VARCHAR(32) NOT NULL,
    city    VARCHAR(32) NOT NULL,
    address VARCHAR(32)
);

CREATE TABLE Post
(
    id                       SERIAL PRIMARY KEY,
    branch_id                INT,
    description              TEXT,
    available_customizations JSONB,
    price                    MONEY,
    status                   post_status DEFAULT 'Available',
    created_at               TIMESTAMP   DEFAULT CURRENT_TIMESTAMP,
    updated_at               TIMESTAMP   DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (branch_id) REFERENCES Branches (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE Motorcycles
(
    id         SERIAL PRIMARY KEY,
    brand      VARCHAR(32) NOT NULL,
    model      VARCHAR(32) NOT NULL,
    cc         VARCHAR(3)  NOT NULL,
    color      VARCHAR(32),
    details    JSONB,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Bill
(
    id             SERIAL PRIMARY KEY,
    user_id        INT   NULL,
    amount         MONEY NOT NULL,
    discount       MONEY          DEFAULT 0,
    payment_method payment_method DEFAULT 'Cash',
    created_at     TIMESTAMP      DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (user_id) REFERENCES Users (id)
        ON UPDATE CASCADE
        ON DELETE SET NULL
);

CREATE TABLE MotoInventory
(
    id             SERIAL PRIMARY KEY,
    moto_id        INT NOT NULL,
    branch_id      INT NOT NULL,
    post_id        INT                  DEFAULT NULL,
    bill_id        INT NULL,
    is_new          BOOLEAN              DEFAULT TRUE,
    license_plate  VARCHAR(7)           DEFAULT NULL,
    km             VARCHAR(7)           DEFAULT '0',
    customizations JSONB,
    status         motoInventory_status DEFAULT 'Available',
    created_at     TIMESTAMP            DEFAULT CURRENT_TIMESTAMP,
    updated_at     TIMESTAMP            DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (moto_id) REFERENCES Motorcycles (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    FOREIGN KEY (branch_id) REFERENCES Branches (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    FOREIGN KEY (post_id) REFERENCES Post (id)
        ON UPDATE CASCADE
        ON DELETE SET NULL,

    FOREIGN KEY (bill_id) REFERENCES Bill (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

INSERT INTO Roles (id, name) VALUES (1, 'user'), (2, 'admin'), (3, 'branch');



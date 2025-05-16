CREATE DATABASE caballos_acero_db
    WITH OWNER = caballosacerouser
    ENCODING = 'UTF8'
    LOCALE_PROVIDER = 'libc'
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

CREATE TABLE Roles
(
    id   SERIAL PRIMARY KEY,
    name VARCHAR(32)
);

CREATE TYPE user_status AS ENUM ('Active', 'Not Verified', 'Inactive', 'Banned');
CREATE TYPE card_status AS ENUM ('Active', 'Inactive');
CREATE TYPE post_status AS ENUM ('Available', 'SoldOut');
CREATE TYPE motoInventory_status AS ENUM ('Available', 'Sold', 'Under Customization', 'Ready', 'Delivered');
CREATE TYPE card_type AS ENUM ('Credit', 'Debit');

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
    expiration_date VARCHAR(5) NOT NULL,
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
    name    VARCHAR(32) NOT NULL,
    country VARCHAR(32) NOT NULL,
    city    VARCHAR(32) NOT NULL,
    address VARCHAR(32)
);

CREATE TABLE Post
(
    id         SERIAL PRIMARY KEY,
    branch_id  INT,
    price      MONEY,
    status     post_status,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

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

CREATE TABLE MotoInventory
(
    id             SERIAL PRIMARY KEY,
    moto_id        INT,
    branch_id      INT,
    post_id        INT,
    license_plate  VARCHAR(7),
    km             VARCHAR(7),
    customizations JSONB,
    status         motoInventory_status,
    created_at     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (moto_id) REFERENCES Motorcycles (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    FOREIGN KEY (branch_id) REFERENCES Branches (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    FOREIGN KEY (post_id) REFERENCES Post (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);



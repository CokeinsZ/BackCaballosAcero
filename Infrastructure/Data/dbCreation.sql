CREATE DATABASE caballos_acero_db
    WITH OWNER = caballosacerouser
    ENCODING = 'UTF8'
    LOCALE_PROVIDER = 'libc'
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

CREATE TABLE Roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(32)
);

CREATE TYPE user_status AS ENUM ('Active', 'Not Verified', 'Inactive', 'Banned');

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



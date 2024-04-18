CREATE DATABASE IF NOT EXISTS client;

USE client;

CREATE TABLE IF NOT EXISTS clients (
    id VARCHAR(40) PRIMARY KEY,
    name VARCHAR(80) NOT NULL,
    surname VARCHAR(80) NOT NULL,
    email VARCHAR(80) NOT NULL,
    birthdate DATE NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted BOOL DEFAULT FALSE
);

CREATE DATABASE IF NOT EXISTS product;

USE product;

CREATE TABLE IF NOT EXISTS products (
    id VARCHAR(40) PRIMARY KEY,
    name VARCHAR(80) NOT NULL,
    dest VARCHAR(250) NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    deleted BOOL DEFAULT FALSE
);
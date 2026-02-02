create database "ReportDb";

CREATE SCHEMA IF NOT EXISTS business;
CREATE SCHEMA IF NOT EXISTS inbox;


CREATE TABLE IF NOT EXISTS inbox.transactions
(
    id UUID NOT NULL PRIMARY KEY,
    processedAt TIMESTAMP NULL
);

CREATE TABLE IF NOT EXISTS business.transactions
(
    id UUID NOT NULL PRIMARY KEY,
    occurredAt TIMESTAMP not null,
    value DECIMAL(15,2) NOT NULL,
    description VARCHAR(100) NULL,
    transactionType INT NOT NULL
);

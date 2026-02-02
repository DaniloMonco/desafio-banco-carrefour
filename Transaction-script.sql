create DATABASE "TransactionDb";

CREATE SCHEMA IF NOT EXISTS business;
CREATE SCHEMA IF NOT EXISTS outbox;


CREATE TABLE IF NOT EXISTS outbox.transactionscreated
(
    id UUID NOT NULL PRIMARY KEY,
    occurredonutc TIMESTAMP NOT NULL,
    processedonutc TIMESTAMP NULL,
    payload TEXT NOT NULL
);


CREATE TABLE IF NOT EXISTS business.transactions
(
    transactionid UUID NOT NULL PRIMARY KEY,
    amount DECIMAL(15,2) NOT NULL,
    type INT NOT NULL,
    occurredat TIMESTAMP NOT NULL
);


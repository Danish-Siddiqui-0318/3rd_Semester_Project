create database pharmacy;

use pharmacy;

CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY, 
    name VARCHAR(100) NOT NULL, 
    email VARCHAR(255) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL, 
    role VARCHAR(50) DEFAULT 'user', 
    created_at DATETIME DEFAULT current_timestamp,
    updated_at DATETIME DEFAULT current_timestamp
);

INSERT INTO Users (name, email, password, role) 
VALUES 
('Alice Admin', 'alice.admin@example.com', 'hashed_password1', 'admin'),
('Bob User', 'bob.user@example.com', 'hashed_password2', 'user');


select * from Users;
